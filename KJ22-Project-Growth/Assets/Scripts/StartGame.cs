using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartTheGame();
    }

    private void StartTheGame()
    {
        m_playerManager.StartGame();
    }
}
