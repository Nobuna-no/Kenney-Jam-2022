using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : NobunAtelier.StateMachineComponent<GameStateDefinition>
{
    [Header("---- Kenney Jam 2022 State Machine ----")]
    [SerializeField]
    private GameStateDefinition m_loadingGameState;
    [SerializeField]
    private GameStateDefinition m_loadingMenuState;

    [SerializeField]
    private bool m_displayStatesDebug = false;
    private Vector2 scrollPos;
    
    [SerializeField]
    private float m_startDelay = 1f;

    protected override void Start()
    {
        Application.targetFrameRate = 60;
        base.Start();
        StartCoroutine(StartWithDelay_Coroutine());
    }

    private void Update()
    {
        Tick(Time.deltaTime);
    }

    public void RestartGame()
    {
        Debug.Assert(m_loadingGameState);
        SetState(m_loadingGameState);
    }

    public void BackToMenu()
    {
        Debug.Assert(m_loadingMenuState);
        SetState(m_loadingMenuState);
    }

    IEnumerator StartWithDelay_Coroutine()
    {
        yield return new WaitForSeconds(m_startDelay);
        Enter();
    }
}
