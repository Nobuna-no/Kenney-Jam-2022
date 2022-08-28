using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NobunAtelier;
using NaughtyAttributes;

public class GameState_LoadingGame : GameState_Loader
{
    protected override GameStateDefinition NextState => m_inGameState;
    [SerializeField]
    private GameStateDefinition m_inGameState;

    [SerializeField, Required("Grid required in LoadingGame State")]
    private SO_PrefabLevel m_grids;

    protected override void Load_internal()
    {
        // Load Game
        LevelManager.Instance.LoadLevel(m_grids.GetRandomLevel());
    }
}
