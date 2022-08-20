﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
	// [SerializeField, NaughtyAttributes.Required("Missing m_PlayerTransform.")]
	// private Transform m_PlayerTransform;
    [SerializeField, NaughtyAttributes.Required("Missing m_cursorPivotTransform.")]
    private Transform m_cursorPivotTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
	{
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        m_cursorPivotTransform.up = new Vector2(MousePosition.x - m_cursorPivotTransform.position.x,
            MousePosition.y - m_cursorPivotTransform.position.y).normalized;
    }
}