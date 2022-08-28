
using UnityEngine;


[CreateAssetMenu(fileName = "[GameMode]", menuName = "KJ2022/GameMode")]
public class GameModeDefinition : ScriptableObject
{
	[SerializeField, Range(1, 4)]
    private int m_minPlayerCount = 2;
    public int MinPlayerCount => m_minPlayerCount;

    [SerializeField, Min(1)]
    private float m_durationInSeconds = 60;
    public float DurationInSeconds => m_durationInSeconds;

    [SerializeField, Min(0)]
    private float m_startCountdown = 2;
    public float StartCountdown => m_startCountdown;
}
