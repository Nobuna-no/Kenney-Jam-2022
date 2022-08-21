using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonManager<PlayerManager>
{
    private int currentPlayerCount = 0;
    private Dictionary<EPlayerOwnership, GrowthCore> PlayerAttribution = new Dictionary<EPlayerOwnership, GrowthCore>();
    [SerializeField]
    private Transform m_mapSpawnPoint;
    [SerializeField]
    private GameObject[] m_maps;
    private int m_currentMapNb = 0;
    [SerializeField]
    private GameObject m_menuCanvas;

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

    public void StartGame()
    {
        LoadMap();
        m_menuCanvas.SetActive(false);
    }
    
    private void LoadMap()
    {
        StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        Instantiate(m_maps[m_currentMapNb], m_mapSpawnPoint);

        yield return new WaitForSeconds(1.5f);

        var spawnPoints = m_maps[m_currentMapNb].GetComponent<MapInfo>().SpawnPoints;
        int i = 0;
        foreach (var p in PlayerAttribution)
        {
            p.Value.transform.position = spawnPoints[i].position;
            i++;
        }

        if (++m_currentMapNb >= m_maps.Length)
            m_currentMapNb = 0;
    }
}
