using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;
using MoreMountains.Feedbacks;

[RequireComponent(typeof(MMF_Player))]
public class BlobBullet : IPoolableObject
{
    public EPlayer Owner;

    [SerializeField, Layer]
    private int PlayerLayer;

    private MMF_Player m_playerFeel;

    private void Awake()
    {
        m_playerFeel = GetComponent<MMF_Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == PlayerLayer)
        {
            GrowthComponent growth = collision.GetComponent<GrowthComponent>();
            if(growth == null || growth.Ownership == Owner)
            {
                return;
            }

            growth.ReceiveHit();
        }

        m_playerFeel.PlayFeedbacks();

        IsActive = false;
    }
}
