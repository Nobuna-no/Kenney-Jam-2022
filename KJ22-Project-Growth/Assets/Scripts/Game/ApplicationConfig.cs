using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationConfig : MonoBehaviour
{
	[SerializeField]
	private int m_targetFramerate = 60;
	
    // Start is called before the first frame update
    void Start()
    {
	    Application.targetFrameRate = m_targetFramerate;
    }
}
