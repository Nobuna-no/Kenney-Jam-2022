using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class WeaponController : MonoBehaviour
{
    [Header("Growth Blob")]
    [InfoBox("Handle Cursor and weaponry.")]
    [HorizontalLine(1, EColor.Orange)]

    // [SerializeField, NaughtyAttributes.Required("Missing m_PlayerTransform.")]
    // private Transform m_PlayerTransform;
    [SerializeField, NaughtyAttributes.Required("Missing m_cursorPivotTransform on WeaponController.")]
    private Transform m_cursorPivotTransform;

    [SerializeField, Required("Missing m_growthComponent on WeaponController.")]
    private GrowthComponent m_growthComponent;

    [SerializeField]
    private float m_bulletPerSecond = 5;
    private float m_currentShootCD = 0;
    [SerializeField, Required("Missing m_bulletPrefab")]
    private BlobBullet m_bulletPrefab;

    [SerializeField]
    private float m_bulletSpeed = 5;

    [SerializeField]
    private PoolObjectID m_poolableObjectID;

    private bool m_wantToShoot = false;

    public void StartShooting()
    {
        m_wantToShoot = true;
    }

    public void StopShooting()
    {
        m_wantToShoot = false;
    }

    // Update is called once per frame
    void Update()
	{
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        m_cursorPivotTransform.up = new Vector2(MousePosition.x - m_cursorPivotTransform.position.x,
            MousePosition.y - m_cursorPivotTransform.position.y).normalized;

        if (Input.GetMouseButtonDown(0))
        {
            StartShooting();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopShooting();
        }
    }

    private void FixedUpdate()
    {
        m_currentShootCD -= Time.fixedDeltaTime;

        if (m_wantToShoot)
        {
            PistolShoot(m_cursorPivotTransform.up);
        }
    }

    private void PistolShoot(Vector2 dir)
    {        
        if (m_growthComponent.CurrentEnergy <= 1)
        {
            return;
        }

        if (m_currentShootCD <= 0f)
        {
            IPoolableObject temp = GamePool.Instance.SpawnObject(m_poolableObjectID, this.transform.position + this.transform.up * Mathf.Sign(this.transform.localScale.x) * 1.1f);
            BlobBullet bullet = temp.GetComponent<BlobBullet>();

            bullet.transform.up = dir;
            bullet.GetComponent<Rigidbody2D>().velocity = dir * m_bulletSpeed;
            Debug.LogWarning("I need to know the owner!!! EPlayer");
            // bullet.Owner = 

            m_growthComponent.ConsumeEnergy(1);
            m_currentShootCD = 1 / m_bulletPerSecond;
        }
    }
}
