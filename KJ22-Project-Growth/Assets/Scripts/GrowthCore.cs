using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[System.Serializable]
public class FloatEvent : UnityEvent<float> {}

public class GrowthCore : MonoBehaviour
{
	[Header("Growth Core")]
	[InfoBox("Handles energy conversion for the main feature.")]
	[HorizontalLine(1, EColor.Orange)]

	[SerializeField]
	private EPlayerOwnership m_Ownership;
	public EPlayerOwnership Ownership => m_Ownership;
		
	[SerializeField, Required()]
	private SpriteRenderer m_coreRenderer;
	
	[SerializeField]
	private int m_initialEnergy = 50;
	[SerializeField]
	private int m_hitEnergyLose = 3;
	
	[Tooltip("- t(1): Initial size")]
	[SerializeField]
	[CurveRange(0, 0.5f, 2, 10, EColor.Orange)]
	private AnimationCurve m_EnergyToSizeCurve;

	[HorizontalLine(1, EColor.Orange)]
	[SerializeField, Required("m_blobObjectID is missing on BlobBullet.")]
	private PoolObjectID m_blobObjectID;
	[SerializeField, MinMaxSlider(-10, 10)]
	private Vector2 m_blobSpawnImpulseForce = new Vector2(-1f, 1f);

	[SerializeField]
	private UnityEvent OnDamageReceived;
	[SerializeField]
	private FloatEvent OnSizeRatioChanged;
	
	[HorizontalLine(1, EColor.Orange)]
	[SerializeField, ProgressBar("Energy", 100, EColor.Green)]
	private int m_currentEnergy = 50;
	public int CurrentEnergy => m_currentEnergy;

	public float GetSizeRatio()
	{
		return m_EnergyToSizeCurve.Evaluate((float)m_currentEnergy / m_initialEnergy);
	}
	
	public void AddEnergy(int quantity)
	{
		m_currentEnergy += quantity;
		OnSizeRatioChanged?.Invoke(GetSizeRatio());
	}
	
	public void ConsumeEnergy(int quantity)
	{
		m_currentEnergy -= quantity;
		OnSizeRatioChanged?.Invoke(GetSizeRatio());
	}
	
	[Button("Inflict Damage", EButtonEnableMode.Playmode)] 
	public void Debug_InflictDamage()
    {
		ReceiveHit(EPlayerOwnership.Unknow, Vector2.up);
	}

	public void ReceiveHit(EPlayerOwnership attacker, Vector2 fromDir)
	{
		m_currentEnergy -= m_hitEnergyLose;

		GamePool.PhysicsSpawnInfo info = new GamePool.PhysicsSpawnInfo
		{
			AverageDir = fromDir,
			ImpulseForceRange = m_blobSpawnImpulseForce,
			Origin = transform.position
		};

		PoolableObject[] objects = GamePool.Instance.PhysicsSpawn(m_blobObjectID, m_hitEnergyLose, info);

		for (int i = 0, c = objects.Length; i < c; ++i)
        {
			GrowthBlob comp = objects[i].GetComponentInChildren<GrowthBlob>();
			comp.Ownership = attacker;
		}

		OnDamageReceived?.Invoke();
		OnSizeRatioChanged?.Invoke(GetSizeRatio());
	}
	
	private void Start()
	{
		m_currentEnergy = m_initialEnergy;
		OnSizeRatioChanged?.Invoke(GetSizeRatio());
	}

    private void OnEnable()
    {
	    m_Ownership = PlayerManager.Instance.GetPlayerOwnership(this);
	    m_coreRenderer.sprite = PlayerManager.Instance.PlayerAssets.GetPlayerSprite(m_Ownership);
	}

    [Button("Add Energy", EButtonEnableMode.Playmode)] 
	private void Debug_AddEnergy()
	{
		AddEnergy(1);
	}
}
