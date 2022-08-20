using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

[RequireComponent(typeof(Transform))]
public class TransformResizer : MonoBehaviour, IGrowthResizable
{
	Transform m_Transform;
	Vector3 m_initialSize;
	
	void Awake()
	{
		m_Transform = GetComponent<Transform>();
		m_initialSize = m_Transform.localScale;
	}
	
	public void UpdateSize(float sizeRatio)
	{
		m_Transform.localScale = m_initialSize * sizeRatio;	
	}
}
