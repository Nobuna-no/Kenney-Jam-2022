using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GamePool : PoolManager<GamePool>
{
	[Header("Game Pool")]
	[InfoBox("Handle poolable object.")]
	[HorizontalLine(1, EColor.Orange)]
	[SerializeField]
	private float m_spawnRange = 0.5f;
	[SerializeField, Range(0, 1)]
	private float m_physicsSpawningAverageDirection = 0.5f;

	protected override GamePool GetInstance()
    {
        return this;
    }

    private void Start()
    {
        ResetManager();
    }
    
	public struct PhysicsSpawnInfo
	{
		public Vector2 Origin;
		public Vector2 AverageDir;
		public Vector2 ImpulseForceRange;
	}
	
	public PoolableObject[] PhysicsSpawn(PoolObjectID objectID, int numberToSpawn, PhysicsSpawnInfo info)
	{
		PoolableObject[] objects = new PoolableObject[numberToSpawn];

		Vector2 normalCW = PerpendicularClockwise(info.AverageDir) * m_spawnRange;
		Vector2 normalCCW = PerpendicularCounterClockwise(info.AverageDir) * m_spawnRange;

		Vector2 spawnPoint;
		for(int i = 0; i < numberToSpawn; ++i)
		{
			Debug.DrawRay(info.Origin, normalCW, Color.red, 2);
			Debug.DrawRay(info.Origin, normalCCW, Color.blue, 2);
			Vector2 dir = new Vector2(Random.Range(normalCW.x, normalCCW.x),
				Random.Range(normalCW.y, normalCCW.y));
			spawnPoint = info.Origin + dir;

			objects[i] = SpawnObject(objectID, spawnPoint);
			
			Debug.DrawRay(info.Origin, dir, Color.green, 2);
			Rigidbody2D rb = objects[i].GetComponent<Rigidbody2D>();
			if(rb)
			{
				rb.velocity = dir * Random.Range(info.ImpulseForceRange.x, info.ImpulseForceRange.y);
			}
		}

		return objects;
	}

	public Vector2 PerpendicularClockwise(Vector2 vector2)
	{
		return new Vector2(vector2.y, -vector2.x) + (vector2 * m_physicsSpawningAverageDirection);
	}

	public Vector2 PerpendicularCounterClockwise(Vector2 vector2)
	{
		return new Vector2(-vector2.y, vector2.x) + (vector2 * m_physicsSpawningAverageDirection);
	}
}
