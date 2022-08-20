using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteResizer : MonoBehaviour, IGrowthResizable
{
	Transform m_transform;
	Vector3 m_initialSize;
	
	void Start()
	{
		m_transform = GetComponent<SpriteRenderer>().transform;
		m_initialSize = m_transform.localScale;
	}
	
	public void UpdateSize(float sizeRatio)
	{
		m_transform.localScale = m_initialSize * sizeRatio;	
	}
}
