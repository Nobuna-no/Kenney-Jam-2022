using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UI_LobbyPlayerReady : MonoBehaviour
{
    private TextMeshProUGUI m_text;
    void Start()
    {
        m_text = GetComponent<TextMeshProUGUI>();
        OnPlayerReadyChanged(0, GameModeManager.Instance.GameModeDefinition.MinPlayerCount);
    }

    // This function is called when the object becomes enabled and active.
    protected void OnEnable()
	{
		LobbyManager.Instance.OnPlayerReadyChanged += OnPlayerReadyChanged;
    }

    // This function is called when the behaviour becomes disabled () or inactive.
    protected void OnDisable()
	{
		LobbyManager.Instance.OnPlayerReadyChanged -= OnPlayerReadyChanged;
	}

    private void OnPlayerReadyChanged(int playerReadyCount, int targetCount)
    {
        m_text.text = GetFormattedString(playerReadyCount, targetCount);
    }

    private string GetFormattedString(int readyCount, int targetCount)
    {
        string color = "green";
        int requiredPlayerCount = targetCount;
        
        if (readyCount < GameModeManager.Instance.GameModeDefinition.MinPlayerCount)
        {
            requiredPlayerCount = GameModeManager.Instance.GameModeDefinition.MinPlayerCount;
            color = "red";
        }
        else if(readyCount < targetCount)
        {
            color = "orange";
        }

        return $"Player Ready <color={color}>{readyCount}</color>/{requiredPlayerCount}";
    }
}
