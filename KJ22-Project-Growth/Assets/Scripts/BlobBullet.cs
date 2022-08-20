using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;
using MoreMountains.Feedbacks;

[RequireComponent(typeof(MMF_Player))]
public class BlobBullet : IPoolableObject
{
	private EPlayerOwnership m_owner;
	public EPlayerOwnership Owner
	{
		get => m_owner;
		set 
		{
			m_renderer.sprite = PlayerManager.Instance.PlayerAssets.GetPlayerBulletSprite(value);
			m_owner = value;
		}
	}

    [SerializeField, Layer]
    private int PlayerLayer;

    private MMF_Player m_playerFeel;
	private SpriteRenderer m_renderer;

    private void Awake()
    {
	    m_playerFeel = GetComponent<MMF_Player>();
	    m_renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == PlayerLayer)
        {
            GrowthCore growth = collision.GetComponent<GrowthCore>();
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
