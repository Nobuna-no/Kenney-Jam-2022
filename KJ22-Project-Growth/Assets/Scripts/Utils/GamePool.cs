using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePool : PoolManager<GamePool>
{
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
	
	public void PhysicsSpawn(PoolObjectID objectID, int numberToSpawn, PhysicsSpawnInfo info)
	{
		Vector2 normalCW = PerpendicularClockwise(info.AverageDir);
		Vector2 normalCCW = PerpendicularCounterClockwise(info.AverageDir);

		Vector2 spawnPoint;
		for(int i = 0; i < numberToSpawn; ++i)
		{
			Vector2 dir = new Vector2(Random.Range(normalCW.x, normalCCW.x),
				Random.Range(normalCW.y, normalCCW.y));
			spawnPoint = info.Origin + dir;
			IPoolableObject obj = SpawnObject(objectID, spawnPoint);
			Debug.DrawRay(info.Origin, dir, Color.yellow, 1);
			
			Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
			if(rb)
			{
				rb.velocity = dir * Random.Range(info.ImpulseForceRange.x, info.ImpulseForceRange.y);
			}
		}
	}
	
	public static Vector2 PerpendicularClockwise(Vector2 vector2)
	{
		return new Vector2(vector2.y, -vector2.x);
	}

	public static Vector2 PerpendicularCounterClockwise(Vector2 vector2)
	{
		return new Vector2(-vector2.y, vector2.x);
	}
}
