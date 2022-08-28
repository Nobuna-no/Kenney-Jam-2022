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

	public GridLevel NextLevelToLoad;
	private GridLevel m_currentLevel;

	private void Start()
    {
		m_currentLevel = FindObjectOfType<GridLevel>();
    }

    protected override LevelManager GetInstance()
	{
		return this;
	}

	public void LoadLevel(GridLevel levelToLoad = null)
	{
		if (levelToLoad == null && NextLevelToLoad == null)
        {
			Debug.LogError("Trying to load a level but both @levelToLoad and @NextLevelToLoad is null");
        }
				
		if (m_currentLevel != null)
		{
			Destroy(m_currentLevel.gameObject);
		}
		m_currentLevel = Instantiate(levelToLoad ? levelToLoad.gameObject : NextLevelToLoad.gameObject).GetComponent<GridLevel>();

		PlacePlayers();
	}

	public void PlacePlayers()
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
