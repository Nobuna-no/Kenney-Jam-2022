using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent (typeof(SpriteRenderer))]
public class WeaponMuzzleCheck : MonoBehaviour
{
    [Header("Weapon Muzzle Check")]
    [InfoBox("Used by WeaponController to know if it is allow to shoot or not.")]
    [HorizontalLine(1, EColor.Orange)]

	[SerializeField, Layer]
    private int m_terrainLayer;

    [SerializeField]
    private Color m_activeColor = Color.white;
    [SerializeField]
    private Color m_disableColor = Color.black;

    private HashSet<GameObject> m_activeSet = new HashSet<GameObject>();
    private SpriteRenderer m_renderer;
    public bool CanShoot { get; private set; } = true;

    private void Awake()
    {
        m_renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == m_terrainLayer)
        {
            if (!m_activeSet.Contains(collision.gameObject))
            {
                m_activeSet.Add(collision.gameObject);
            }
            m_renderer.color = m_disableColor;
            CanShoot = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == m_terrainLayer)
        {
            if (m_activeSet.Contains(collision.gameObject))
            {
                m_activeSet.Remove(collision.gameObject);
            }
        }

        if (m_activeSet.Count == 0)
        {
            m_renderer.color = m_activeColor;
            CanShoot = true;
        }
    }
}
