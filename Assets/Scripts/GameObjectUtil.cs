﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectUtil  
{
	private static Dictionary<RecycleGameObject, ObjectPool> pools = new Dictionary<RecycleGameObject, ObjectPool>();
	
	public static GameObject Instantiate (GameObject prefab, Vector3 pos)
	{
		GameObject instance = null;

		var recycleScript = prefab.GetComponent<RecycleGameObject> ();
		if (recycleScript != null) 
		{
			var pool = GetObjectPool (recycleScript);
			instance = pool.NextObject (pos).gameObject;
		} 
		else 
		{
			instance = GameObject.Instantiate (prefab);
			instance.transform.position = pos;
		}
		return instance;
	}

	public static void Destroy (GameObject gameObject)
	{
		var recyleGameObject = gameObject.GetComponent <RecycleGameObject> ();
		if (recyleGameObject != null) 
		{
			recyleGameObject.ShutDown ();
		} 
		else 
		{
			GameObject.Destroy (gameObject);
		}
	}

	public static ObjectPool GetObjectPool (RecycleGameObject reference)
	{
		ObjectPool pool = null;

		if (pools.ContainsKey (reference)) 
		{
			pool = pools [reference];	
		} 
		else 
		{
			var poolContainer = new GameObject (reference.gameObject.name + "ObjectPool");
			pool = poolContainer.AddComponent<ObjectPool>();
			pool.prefab = reference;
			pools.Add (reference,pool);
		}

		return pool;
	}
}