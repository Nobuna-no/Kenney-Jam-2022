using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyManager : SingletonManager<LobbyManager>
{    
    private HashSet<Player> m_readyPlayer = new HashSet<Player>(4);

    public delegate void OnPlayerReadyChangedDelegate(int playerReadyCount, int targetPlayerCount);
    public event OnPlayerReadyChangedDelegate OnPlayerReadyChanged;

    protected override LobbyManager GetInstance()
    {
        return this;
    }

    private void OnEnable()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
	{
		var instance = PlayerInputManager.instance;
		if(instance)
		{
			instance.onPlayerJoined -= OnPlayerJoined;
		}
    }

    private void OnPlayerJoined(PlayerInput obj)
    {
        PlayerReady(null);
    }

    public void PlayerReady(Player player)
    {
        if (!enabled)
        {
            return;
        }

        if (player != null)
        {
            if (m_readyPlayer.Contains(player))
            {
                m_readyPlayer.Remove(player);
            }
            else
            {
                m_readyPlayer.Add(player);
            }
        }

        OnPlayerReadyChanged?.Invoke(m_readyPlayer.Count, PlayerInputManager.instance.playerCount);
    }
}
