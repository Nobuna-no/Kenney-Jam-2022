using System.Collections.Generic;
using UnityEngine;


public abstract class PoolManager<T> : SingletonManager<T>
	where T: class
{
	[System.Serializable]
	protected class PoolObjectConfig
    {
		public PoolableObject poolObject;
		[Tooltip("Size of the initial reserve. Number of inactive object to instantiate by default.")]
		public int reserveSize;
	}

	// Parent object of where the instantiate objects are placed.
	[SerializeField]
	protected Transform m_reserveParent = null;
	[SerializeField]
	private bool m_canForceInstantiateInEmergency = true;
	[SerializeField]
	// All the prefabs type that could be spawned at some point
	protected PoolObjectConfig[] m_objectsDefinition = null;
	// protected List<IPoolableObject> m_pool = new List<IPoolableObject>();
	protected Dictionary<PoolObjectID, List<PoolableObject>> m_objectPoolPerID = new Dictionary<PoolObjectID, List<PoolableObject>>();

	public bool IsPaused { private set; get; }

	public sealed override void StartManager()
	{
		OnPoolManagerStart();
	}

	public sealed override void PauseManager(bool isPaused)
	{
		IsPaused = isPaused;
	}

	public sealed override void ResetManager()
	{
		foreach (var key in m_objectPoolPerID.Keys)
		{
			foreach (var val in m_objectPoolPerID[key])
			{
				val.ResetObject();
			}
		}

		FillReserves();

		OnPoolManagerReset();
	}

	// To override and use as Unity.Start
	protected virtual void OnPoolManagerStart()
	{

	}	

	// Called once all the object has been reset.
	protected virtual void OnPoolManagerReset()
    { }

	// Usefull to initialize the new object and bind method to IPoolableObject.onActivation for instance.
	protected virtual void OnObjectCreation(PoolableObject obj)
    { }

	protected virtual void OnObjectSpawned(PoolableObject obj)
	{ }

	public PoolableObject SpawnObject(PoolObjectID id, Vector3 position)
	{
		if (!m_objectPoolPerID.ContainsKey(id))
		{
			Debug.LogWarning($"Trying to instanciate unknown object of id: {id}. Skipped...");
			return null;
		}

		PoolableObject target = m_objectPoolPerID[id].Find((obj) => { return !obj.IsActive; });

		if (target == null)
		{
			Debug.Log($"Pool overflow for object id: {id}!");

			if (m_canForceInstantiateInEmergency)
			{
				Debug.LogWarning("Instantiating new batch in emergency!");
				m_objectPoolPerID[id].AddRange(InstantiateBatch(m_objectPoolPerID[id][0], m_objectPoolPerID[id].Count / 2));
				return SpawnObject(id, position);
			}

			Debug.LogWarning($"Cannot force instantiate, object of id: {id}. Skipped...");
			return null;
		}

		target.Position = position;
		target.IsActive = true;
		OnObjectSpawned(target);
		return target;
	}

	public void SpawnObject(PoolObjectID id, Vector3 location, float radius, int count)
	{
		for (int i = 0; i < count; ++i)
		{
			SpawnObject(id, GetSpawnPointInRadius(location, radius));
		}
	}

	protected Vector3 GetSpawnPointInRadius(Vector3 location, float radius)
	{
		Vector2 circlePos = Random.insideUnitCircle * radius;
		return new Vector3(circlePos.x, circlePos.y, 0.0f) + location;
	}

	private PoolableObject[] InstantiateBatch(PoolableObject prefab, int count)
	{
		PoolableObject[] out_array = new PoolableObject[count];

		for (int i = 0; i < count; ++i)
		{
			out_array[i] = Instantiate(prefab.gameObject, Vector3.zero, Quaternion.identity, m_reserveParent).GetComponent<PoolableObject>();
			out_array[i].ResetObject();
			OnObjectCreation(out_array[i]);
		}

		return out_array;
	}

	private void FillReserves()
    {
		foreach (var def in m_objectsDefinition)
        {
			PoolObjectID workingID = def.poolObject.ID;
			if (!m_objectPoolPerID.ContainsKey(workingID))
            {
				m_objectPoolPerID.Add(workingID, new List<PoolableObject>(def.reserveSize));
			}

			// it's ok if we have more object.
			int reserveCountTarget = def.reserveSize - m_objectPoolPerID[workingID].Count;			
			if (reserveCountTarget > 0)
            {
				m_objectPoolPerID[workingID].AddRange(InstantiateBatch(def.poolObject, reserveCountTarget));
            }
        }
	}
	
	private void Start()
	{ }

	// Useless, who code this omg...
	/// <summary>
	/// Either calls SpawnObject or activate an object
	/// </summary>
	/// <param name="prefabIndex">Index of the object type</param>
	/// <returns>Whether an object was added</returns>
	//protected virtual bool AddObject(int prefabIndex, Vector3 position)
	//{
	//	if (m_pool.Count <= 0)
	//	{
	//		SpawnObject(prefabIndex, position);
	//		return true;
	//	}

	//	for (int i = 0; i < m_pool.Count; ++i)
	//	{
	//		IPoolableObject crtObj = m_pool[i];
	//		if (crtObj == null || crtObj.PrefabIndex != prefabIndex || crtObj.IsActive)
	//			continue;

	//		crtObj.Position = position;
	//		crtObj.IsActive = true;
	//		return true;
	//	}

	//	return false;
	//}


	//protected void SpawnObject(int index, Vector3 position)
	//{
	//	GameObject prefab = m_objectsDefinition[index].poolObject.gameObject;
	//	if (prefab == null)
	//		return;

	//	GameObject obj = Instantiate(prefab, position, Quaternion.identity, m_folder);

	//	IPoolableObject poolObj = obj.GetComponent<IPoolableObject>();
	//	if (!poolObj)
	//		return;

	//	poolObj.PrefabIndex = index;
	//	m_pool.Add(poolObj);

	//	// onObjectCreation?.Invoke(poolObj);
	//}



}