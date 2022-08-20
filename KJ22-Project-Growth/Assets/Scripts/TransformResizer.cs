using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class TransformResizer : MonoBehaviour, IGrowthResizable
{
	Transform m_Transform;
	Vector3 m_initialSize;
	
	void Start()
	{
		m_Transform = GetComponent<Transform>();
		m_initialSize = m_Transform.localScale;
	}
	
	public void UpdateSize(float sizeRatio)
	{
		m_Transform.localScale = m_initialSize * sizeRatio;	
	}
}
