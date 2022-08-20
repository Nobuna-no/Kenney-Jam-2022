using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class GrowthBlob : MonoBehaviour
{
    [Header("Growth Blob")]
    [InfoBox("On trigger with a player, add 1 energy to player and disable itself.")]
    [HorizontalLine(1, EColor.Orange)]

    [SerializeField, Layer]
    private int m_playerLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == m_playerLayer)
        {
            GrowthCore gc = collision.gameObject.GetComponent<GrowthCore>();
            gc.AddEnergy(1);

            transform.parent.gameObject.SetActive(false);
        }
    }
}
