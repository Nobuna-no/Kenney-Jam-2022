using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[System.Serializable]
public class TextEvent : UnityEvent<string> { }

public class RoundManager : NobunAtelier.StateComponent<GameStateDefinition>
{
    [SerializeField]
    private float m_roundDurationInSeconds = 60;
	private float m_currentTime = -1;
    [ReadOnly]
    private string textToDisplay;

    public UnityEvent OnRoundBegin;
    public TextEvent OnTimeUpdate;
    public UnityEvent OnRoundEnd;

	public override void Enter()
	{
		GameBegin();
	}

    protected void GameBegin()
	{
		PlayerManager.Instance.BlockPlayerInputs(false);
        StartRound();
    }

    protected void GameOver()
    {
        OnRoundEnd?.Invoke();
    }

    [Button("Start Round")]
    public void StartRound()
    {
        m_currentTime = m_roundDurationInSeconds;
        OnRoundBegin?.Invoke();
    }

    // Update is called once per frame
    void FixedUpdate()
	{
		if(m_currentTime == -1f)
		{
			return;
		}
		
        m_currentTime -= Time.fixedDeltaTime;

        if (m_currentTime <= 0)
        {            
        	m_currentTime = -1f;
            GameOver();
        }
        else
        {
            textToDisplay = ((int)m_currentTime).ToString();
            OnTimeUpdate?.Invoke(textToDisplay);
        }
    }
}
