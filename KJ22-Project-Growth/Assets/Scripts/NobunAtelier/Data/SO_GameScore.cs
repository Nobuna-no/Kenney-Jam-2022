using System;
using UnityEngine;

[CreateAssetMenu(fileName = "[GameScore]", menuName = "Nobun'Atelier/GameScore")]
public class SO_GameScore : ScriptableObject
{
	[NaughtyAttributes.ReadOnly, SerializeField]
	private int value = 0;

	public int Value => value;

	public event Action<int> OnValueChanged = null;

	public void Increment()
	{
		++value;
		OnValueChanged?.Invoke(value);
	}

	public void Reset()
	{
		value = 0;
	}
}
