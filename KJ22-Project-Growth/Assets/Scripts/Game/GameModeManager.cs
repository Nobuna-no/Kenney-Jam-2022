using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NobunAtelier;

public class GameModeManager : SingletonManager<GameModeManager>
{
    [SerializeField]
    private GameModeDefinition m_gameModeDefinition;
    public GameModeDefinition GameModeDefinition => m_gameModeDefinition;

    protected override GameModeManager GetInstance()
    {
        return this;
    }

    public void SetGameMode(GameModeDefinition newGM)
    {
        m_gameModeDefinition = newGM;
    }
}

