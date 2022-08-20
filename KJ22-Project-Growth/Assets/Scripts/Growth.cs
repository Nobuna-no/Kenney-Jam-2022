using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[System.Serializable]
public class FloatEvent : UnityEvent<float> {}

public class Growth : MonoBehaviour
{
	[Header("Growth - Main Feature")]
	[HorizontalLine(1, EColor.Orange)]
	
	[SerializeField]
	private int m_initialFat = 30;
	[SerializeField]
	private int m_hitFatLose = 3;
	
	[Tooltip("- t(1): Initial size")]
	[SerializeField]
	[CurveRange(0, 0.5f, 10, 10, EColor.Orange)]
	private AnimationCurve m_SizeRatioLimits;
	private float m_currentSizeRatio;
	
	[SerializeField]
	private UnityEvent OnDamageReceived;
	[SerializeField]
	private FloatEvent OnSizeRatioChanged;
	
	[HorizontalLine(1, EColor.Orange)]
	[SerializeField, ProgressBar("Energy", 100, EColor.Green)]
	private int m_currentFat = 30;
	
	public float GetSizeRatio()
	{
		return m_SizeRatioLimits.Evaluate((float)m_currentFat / m_initialFat);
	}
	
	public void AddFat(int quantity)
	{
		m_currentFat += quantity;
		OnSizeRatioChanged?.Invoke(GetSizeRatio());
	}
	
	public void ConsumeFat(int quantity)
	{
		m_currentFat -= quantity;
		OnSizeRatioChanged?.Invoke(GetSizeRatio());
	}
	
	[Button("Inflict Damage", EButtonEnableMode.Playmode)] 
	public void ReceiveHit()
	{
		m_currentFat -= m_hitFatLose;	
		OnDamageReceived?.Invoke();
		OnSizeRatioChanged?.Invoke(GetSizeRatio());
	}
	
	private void Start()
	{
		OnSizeRatioChanged?.Invoke(GetSizeRatio());
	}
	
	[Button("Add Energy", EButtonEnableMode.Playmode)] 
	private void Debug_AddEnergy()
	{
		AddFat(1);
	}
}
