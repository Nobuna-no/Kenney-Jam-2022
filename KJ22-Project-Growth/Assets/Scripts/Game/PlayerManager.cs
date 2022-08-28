using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : SingletonManager<PlayerManager>
{
    
	[SerializeField]
	private SO_CharacterSpriteMap m_playerAssets;
	public SO_CharacterSpriteMap PlayerAssets => m_playerAssets;
	
	private Dictionary<EPlayerOwnership, Player> PlayerAttribution = new Dictionary<EPlayerOwnership, Player>();
	
	private int currentPlayerCount = 0;
    
    private PlayerInputManager m_playerInputManager;

    protected override PlayerManager GetInstance()
    {
        return this;
    }

    public void AddPlayer(Player character)
    {
        if (!PlayerAttribution.ContainsValue(character))
        {
            EPlayerOwnership owner = (EPlayerOwnership)(++currentPlayerCount);
            PlayerAttribution.Add(owner, character);
        }
    }

    // Update is called once per frame
    public EPlayerOwnership GetPlayerOwnership(Player character)
    {        
        if (PlayerAttribution.ContainsValue(character))
        {
            foreach (var p in PlayerAttribution)
            {
                if(p.Value == character)
                {
                    return p.Key;
                }
            }
        }

        return EPlayerOwnership.Unknow;
    }

    public Vector2 GetDirectionToPlayer(EPlayerOwnership owner, Vector3 from)
    {
        if(!PlayerAttribution.ContainsKey(owner))
        {
            return Vector2.zero;
        }

        Transform target = PlayerAttribution[owner].transform;
        return (target.position - from).normalized;
    }

	public int GetPlayers(out Player[] out_players)
    {
        out_players = new Player[PlayerAttribution.Count];

        int i = 0;
        foreach (var p in PlayerAttribution)
        {
            out_players[i++] = p.Value;
        }

        return i;
    }

    public void BlockPlayerInputs(bool value)
    {
        foreach (var p in PlayerAttribution)
        {
            p.Value.BlockInput(value);
        }
    }
}
