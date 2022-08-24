using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Respawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_respawnersTransform;
    [SerializeField, Layer]
    private int m_layerToRespawn;

    [SerializeField, MinMaxSlider(-10, 10)]
    private Vector2 RespawnVelocityX;
    [SerializeField, MinMaxSlider(-10, 10)]
    private Vector2 RespawnVelocityY;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == m_layerToRespawn)
        {
            if (collision.attachedRigidbody)
            {
                int target = Random.Range(0, m_respawnersTransform.Length);
                collision.attachedRigidbody.position = m_respawnersTransform[target].position;
                collision.attachedRigidbody.velocity = new Vector2(Random.Range(RespawnVelocityX.x, RespawnVelocityX.y),
                    Random.Range(RespawnVelocityY.x, RespawnVelocityY.y));
            }
        }
    }
}
