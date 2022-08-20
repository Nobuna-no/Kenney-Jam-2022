using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[System.Serializable]
public class FloatEvent : UnityEvent<float> {}

public class GrowthComponent : MonoBehaviour
{
	[Header("Growth - Main Feature")]
	[HorizontalLine(1, EColor.Orange)]

	[SerializeField]
	private EPlayer m_Ownership;
	public EPlayer Ownership => m_Ownership;
	[SerializeField]
	private int m_initialEnergy = 50;
	[SerializeField]
	private int m_hitEnergyLose = 3;
	
	[Tooltip("- t(1): Initial size")]
	[SerializeField]
	[CurveRange(0, 0.5f, 2, 10, EColor.Orange)]
	private AnimationCurve m_EnergyToSizeCurve;
	
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
	public void ReceiveHit()
	{
		m_currentEnergy -= m_hitEnergyLose;	
		OnDamageReceived?.Invoke();
		OnSizeRatioChanged?.Invoke(GetSizeRatio());
	}
	
	private void Start()
	{
		m_currentEnergy = m_initialEnergy;
		OnSizeRatioChanged?.Invoke(GetSizeRatio());
	}
	
	[Button("Add Energy", EButtonEnableMode.Playmode)] 
	private void Debug_AddEnergy()
	{
		AddEnergy(1);
	}
}
