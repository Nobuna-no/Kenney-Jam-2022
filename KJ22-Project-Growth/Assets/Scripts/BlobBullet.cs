using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;
using MoreMountains.Feedbacks;

[RequireComponent(typeof(Rigidbody2D))]
public class BlobBullet : PoolableObject
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
    private Vector2 m_blobSpawnImpulseForce = new Vector2(-3f,3f);
	private TrailRenderer m_trailRenderer;
	private SpriteRenderer m_renderer;
    private Rigidbody2D m_body;
    private void Awake()
    {
        m_body = GetComponent<Rigidbody2D>();
	    m_renderer = GetComponent<SpriteRenderer>();
        m_trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 impactDir = -m_body.velocity.normalized;
        bool hitOtherPlayer = false;

        if (collision.gameObject.layer == PlayerLayer)
        {
            GrowthCore growth = collision.GetComponent<GrowthCore>();
            if(growth == null || growth.Ownership == Owner)
            {
                return;
            }

            growth.ReceiveHit(Owner, impactDir);
            hitOtherPlayer = true;
        }

        GamePool.PhysicsSpawnInfo info = new GamePool.PhysicsSpawnInfo()
        {
            Origin = transform.position,
            AverageDir = impactDir,
            ImpulseForceRange = m_blobSpawnImpulseForce
        };

        PoolableObject[] objects = GamePool.Instance.PhysicsSpawn(m_blobObjectID, 1, info);
        for (int i = 0, c = objects.Length; i < c; ++i)
        {
            GrowthBlob comp = objects[i].GetComponentInChildren<GrowthBlob>();
            comp.Ownership = hitOtherPlayer ? m_owner : EPlayerOwnership.Unknow;
        }

        IsActive = false;        
    }

    protected override void OnActivation()
    {
        m_trailRenderer.Clear();
    }
}
