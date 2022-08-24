using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NobunAtelier;

[RequireComponent(typeof(MoreMountains.Feedbacks.MMF_Player))]
public abstract class GameState_Loader : StateComponent<GameStateDefinition>
{
    protected abstract GameStateDefinition NextState { get; }

    private MoreMountains.Feedbacks.MMF_Player m_feedback;

    private void Awake()
    {
        m_feedback = GetComponent<MoreMountains.Feedbacks.MMF_Player>();
    }

    public void Load()
    {
        m_feedback.PauseFeedbacks();
        
        Load_internal();

        m_feedback.ResumeFeedbacks();
        SetState(NextState);
    }


    public override void Enter()
    {
        m_feedback.PlayFeedbacks();
    }

    protected abstract void Load_internal();
}
