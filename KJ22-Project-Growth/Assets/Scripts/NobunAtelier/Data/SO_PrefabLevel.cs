using UnityEngine;

[CreateAssetMenu(fileName = "[PrefabLevels]", menuName = "Nobun'Atelier/PrefabLevels")]
public class SO_PrefabLevel : ScriptableObject
{
	[SerializeField] 
	private GridLevel[] scenes = null;

	public GridLevel GetRandomLevel()
	{
		int index = Random.Range(0, scenes.Length);
		return scenes[index];
	}
}
