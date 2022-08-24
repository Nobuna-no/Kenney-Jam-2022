using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonManager<PlayerManager>
{
    
	[SerializeField]
	private SO_CharacterSpriteMap m_playerAssets;
	public SO_CharacterSpriteMap PlayerAssets => m_playerAssets;
	
	private Dictionary<EPlayerOwnership, GrowthCore> PlayerAttribution = new Dictionary<EPlayerOwnership, GrowthCore>();
	
	private int currentPlayerCount = 0;
	
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

	public int GetPlayers(out GrowthCore[] out_players)
    {
        out_players = new GrowthCore[PlayerAttribution.Count];

        int i = 0;
        foreach (var p in PlayerAttribution)
        {
            out_players[i++] = p.Value;
        }

        return i;
    }
	
    //public void StartGame()
    //{
    //    LoadMap();
    //}
    
    //private void LoadMap()
    //{
    //    foreach (var o in m_menuDisable)
    //        o.SetActive(false);

    //    Instantiate(m_maps[m_currentMapNb]);

    //    var spawnPoints = m_maps[m_currentMapNb].GetComponent<MapInfo>().SpawnPoints;
    //    int i = 0;
    //    foreach (var p in PlayerAttribution)
    //    {
    //        p.Value.transform.position = spawnPoints[i].position;
    //        i++;
    //    }

    //    if (++m_currentMapNb >= m_maps.Length)
    //        m_currentMapNb = 0;
    //}
}
