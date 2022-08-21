using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class GrowthBlob : MonoBehaviour
{
    [Header("Growth Blob")]
    [InfoBox("On trigger with a player, add 1 energy to player and disable itself.")]
    [HorizontalLine(1, EColor.Orange)]

    [SerializeField, Required("Grotwh blob need to know it's main collider")]
    private Collider2D m_collider;
    [SerializeField, Required("Grotwh blob need to know it's main rigidbody")]
    private Rigidbody2D m_body;
    [SerializeField, Required("Grotwh blob need to know its renderer.")]
    private SpriteRenderer m_renderer;
    
    [SerializeField]
    private bool m_canBeCollected = false;

    [SerializeField]
    private EPlayerOwnership m_ownership = EPlayerOwnership.None;
    public EPlayerOwnership Ownership
    {
        get => m_ownership;
        set
        {
            if (value == EPlayerOwnership.None)
            {
                m_canBeCollected = false;
            }
            else
            {
                m_canBeCollected = true;
                m_currentSpeed = Random.Range(m_speedTowardOwner.x, m_speedTowardOwner.y);

                m_ownership = value;
                m_renderer.sprite = PlayerManager.Instance.PlayerAssets.GetHeartSprite(value);
                m_collider.enabled = m_ownership != EPlayerOwnership.Unknow ? false : true;
            }
        }
    }

    [SerializeField, Layer]
    private int m_playerLayer;
    [SerializeField, MinMaxSlider(1, 10)]
    private Vector2 m_speedTowardOwner = new Vector2(3, 6);
    private float m_currentSpeed;
    private void FixedUpdate()
    {
        if (m_ownership != EPlayerOwnership.None && m_ownership != EPlayerOwnership.Unknow)
        {
            m_body.velocity = PlayerManager.Instance.GetDirectionToPlayer(m_ownership, m_body.position) * m_currentSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!m_canBeCollected)
        {
            return;
        }

        if (collision.gameObject.layer == m_playerLayer)
        {
            // If unknow owner, just collect energy
            if (m_ownership != EPlayerOwnership.Unknow)
            {
                // else check ownership
                GrowthCore growth = collision.GetComponent<GrowthCore>();
                if (growth == null || growth.Ownership != m_ownership)
                {
                    return;
                }
            }

            GrowthCore gc = collision.gameObject.GetComponent<GrowthCore>();
            gc.AddEnergy(1);

            transform.parent.gameObject.SetActive(false);
            Ownership = EPlayerOwnership.None;
        }
    }
}
