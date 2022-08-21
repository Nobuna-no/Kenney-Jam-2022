using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[System.Serializable]
public class TextEvent : UnityEvent<string> { }

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    private float m_roundDurationInSeconds = 60;
    private float m_currentTime = 0;
    [ReadOnly]
    private string textToDisplay;

    public UnityEvent OnRoundBegin;
    public TextEvent OnTimeUpdate;
    public UnityEvent OnRoundEnd;

    [Button("Start Round")]
    public void StartRound()
    {
        m_currentTime = m_roundDurationInSeconds;
        OnRoundBegin?.Invoke();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_currentTime -= Time.fixedDeltaTime;

        if (m_currentTime <= 0)
        {            
            OnRoundEnd?.Invoke();
        }
        else
        {
            textToDisplay = m_currentTime.ToString("#.#");
            OnTimeUpdate?.Invoke(textToDisplay);
        }
    }
}
