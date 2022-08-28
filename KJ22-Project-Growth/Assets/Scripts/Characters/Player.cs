using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

using NaughtyAttributes;

[RequireComponent(typeof(PlayerInput), typeof(CharacterGrowthCore), typeof(CharacterMovementAndController))]
public class Player : MonoBehaviour
{
    [Header("Player")]
    [InfoBox("Holds a link to each character components.")]
    [HorizontalLine(1, EColor.Orange)]
    [SerializeField]
	public bool m_lobbyReady = false;
    [SerializeField, ReadOnly]
    private bool m_InitSucessful = false;

    public EPlayerOwnership Ownership => Core.Ownership;

    public PlayerInput Input { get; private set; }
    public CharacterGrowthCore Core { get; private set; }
    public CharacterMovementAndController MovementAndControl { get; private set; }
    public CharacterAbilitiesController Abilities { get; private set; }
    
    private InputActionMap m_actionsMap = null;

    private void Awake()
    {
        Input = GetComponent<PlayerInput>();
        Core = GetComponent<CharacterGrowthCore>();
        MovementAndControl = GetComponent<CharacterMovementAndController>();
        Abilities = GetComponentInChildren<CharacterAbilitiesController>();

        m_InitSucessful = Input && Core && MovementAndControl && Abilities;
     
        if (m_InitSucessful)
        {
            PlayerManager.Instance.AddPlayer(this);
            m_actionsMap = Input.actions.FindActionMap("Player");
        }
    }

    private void OnEnable()
    {
        var startAction = m_actionsMap.FindAction("Start");
        startAction.started += PlayerReady;
    }

    private void OnDisable()
    {
        var startAction = m_actionsMap.FindAction("Start");
        startAction.started -= PlayerReady;
    }

    private void PlayerReady(CallbackContext ccontext)
    {
        LobbyManager.Instance.PlayerReady(this);
    }

    public void BlockInput(bool value)
    {
        if (value)
        {
            Input.DeactivateInput();
        }
        else
        {
            Input.ActivateInput();
        }
    }
}
