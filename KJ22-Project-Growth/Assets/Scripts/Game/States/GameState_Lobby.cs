using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

public class GameState_Lobby : GameState
{
	[SerializeField, Required]
	private GameStateDefinition m_loadingState;

	[SerializeField, Required]
	private GameObject m_LobbyUI;

	[SerializeField, Min(0)]
	private float m_exitLobbyDelayInSeconds = 3;
	private float m_currentDelay = -1;

	public override void Enter()
	{
		PlayerInputManager.instance.EnableJoining();
		LobbyManager.Instance.enabled = true;
		m_LobbyUI.SetActive(true);

        LobbyManager.Instance.OnPlayerReadyChanged += OnPlayerReadyChanged;
	}

    private void OnPlayerReadyChanged(int playerReadyCount, int targetCount)
    {
		m_currentDelay = -1;

		if (playerReadyCount == targetCount)
		{
			m_currentDelay = m_exitLobbyDelayInSeconds;
		}
	}
	
	public override void Exit()
	{
		LobbyManager.Instance.OnPlayerReadyChanged -= OnPlayerReadyChanged;
		PlayerInputManager.instance.DisableJoining();
		LobbyManager.Instance.enabled = false;
		m_LobbyUI.SetActive(false);

		PlayerManager.Instance.BlockPlayerInputs(true);
	}

    private void FixedUpdate()
    {
		if (m_currentDelay == -1)
        {
			return;
        }

		m_currentDelay -= Time.fixedDeltaTime;
		if (m_currentDelay <= 0)
        {
			m_currentDelay = -1;
			SetState(m_loadingState);
		}
	}
}
