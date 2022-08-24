using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonManager<LevelManager>
{
	//[SerializeField]
	//private GameObject[] m_maps;
	//private int m_currentMapNb = 0;
	//[SerializeField]
	//private GameObject[] m_menuDisable;

	private GridLevel m_currentLevel;

    private void Start()
    {
		m_currentLevel = FindObjectOfType<GridLevel>();
    }

    protected override LevelManager GetInstance()
	{
		return this;
	}
	
	public void LoadLevel(GridLevel levelPrefab)
	{
		//foreach (var o in m_menuDisable)
		//	o.SetActive(false);
				
		if (m_currentLevel != null)
		{
			Destroy(m_currentLevel.gameObject);
		}
		m_currentLevel = Instantiate(levelPrefab.gameObject).GetComponent<GridLevel>();

		SpawnPlayers();
		// Instantiate(m_maps[m_currentMapNb]);

		//var spawnPoints = m_maps[m_currentMapNb].GetComponent<GridLevel>().SpawnPoints;
		//int i = 0;

		//PlayerManager.Instance.GetPlayers(out var players);
		//foreach (var p in players)
		//{
		//	p.transform.position = spawnPoints[i++].position;			
		//}

		// if (++m_currentMapNb >= m_maps.Length)
		// 	m_currentMapNb = 0;
	}

	public void SpawnPlayers()
    {
		var spawnPoints = m_currentLevel.SpawnPoints;
		int i = 0;

		PlayerManager.Instance.GetPlayers(out var players);
		foreach (var p in players)
		{
			p.transform.position = spawnPoints[i++].position;
		}
	}
}
