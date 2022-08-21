using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;
using MoreMountains.Feedbacks;

[RequireComponent(typeof(Rigidbody2D))]
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

    [SerializeField, Required("m_blobObjectID is missing on BlobBullet.")]
    private PoolObjectID m_blobObjectID;
    
    [SerializeField, MinMaxSlider(-10, 10)]
    private Vector2 BlobSpawnImpulseForce;

    private MMF_Player m_playerFeel;
	private SpriteRenderer m_renderer;
    private Rigidbody2D m_body;
    private void Awake()
    {
        m_body = GetComponent<Rigidbody2D>();
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

        GamePool.PhysicsSpawnInfo info = new GamePool.PhysicsSpawnInfo()
        {
            Origin = transform.position,
            AverageDir = m_body.velocity,
            ImpulseForceRange = BlobSpawnImpulseForce
        };

        GamePool.Instance.PhysicsSpawn(m_blobObjectID, 1, info);
        // m_playerFeel.PlayFeedbacks();

        IsActive = false;
    }
}
