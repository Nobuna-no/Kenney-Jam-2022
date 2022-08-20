using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonManager<PlayerManager>
{
    private int currentPlayerCount = 0;
    private Dictionary<GrowthCore, EPlayerOwnership> PlayerAttribution = new Dictionary<GrowthCore, EPlayerOwnership>();

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
        if (!PlayerAttribution.ContainsKey(character))
        {
            PlayerAttribution.Add(character, (EPlayerOwnership)(++currentPlayerCount));
        }
        
        return PlayerAttribution[character];
    }
}
