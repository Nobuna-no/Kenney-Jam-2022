using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonManager<PlayerManager>
{
    private int currentPlayerCount = 0;
    private Dictionary<EPlayerOwnership, GrowthCore> PlayerAttribution = new Dictionary<EPlayerOwnership, GrowthCore>();

	[SerializeField]
	private SO_CharacterSpriteMap m_playerAssets;
	public SO_CharacterSpriteMap PlayerAssets => m_playerAssets;
	
    protected override PlayerManager GetInstance()
    {
        return this;
    }

    // Update is called once per frame
    public EPlayerOwnership GetPlayerOwnership(GrowthCore character)
    {
        if (!PlayerAttribution.ContainsValue(character))
        {
            EPlayerOwnership owner = (EPlayerOwnership)(++currentPlayerCount);
            PlayerAttribution.Add(owner, character);
            return owner;
        }
        else
        {
            return character.Ownership;
        }
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
}
