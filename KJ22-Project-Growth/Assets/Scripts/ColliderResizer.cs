using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

[RequireComponent(typeof(CircleCollider2D))]
public class ColliderResizer : MonoBehaviour, IGrowthResizable
{
	CircleCollider2D m_collider;
	float m_initialSize;
	
	void Start()
	{
		m_collider = GetComponent<CircleCollider2D>();
		m_initialSize = m_collider.radius;
	}
	
	public void UpdateSize(float sizeRatio)
	{
		m_collider.radius = m_initialSize * sizeRatio;	
	}
}
