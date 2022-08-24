using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using NaughtyAttributes;



public class StartGame : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager;

	[SerializeField]
	private UnityEvent OnGameStart;
	
	[SerializeField, Layer]
	private int m_playerLayer;
	
	bool started = false;
	
    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (started)
		{
			return;
		}
		
		if (collision.gameObject.layer == m_playerLayer)
		{
			StartTheGame();
		}
    }

    private void StartTheGame()
    {
	    // m_playerManager.StartGame();
	    OnGameStart?.Invoke();
	    started = true;
    }
}
