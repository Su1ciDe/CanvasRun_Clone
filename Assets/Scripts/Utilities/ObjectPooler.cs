using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Utilities
{
	public class ObjectPooler : Singleton<ObjectPooler>
	{
		[Serializable]
		public class Pool
		{
			[Tooltip("Give a tag to the pool to call")]
			public string Tag;
			[Tooltip("The prefab to be pooled")]
			public GameObject Prefab;
			[Tooltip("The size (count) of the pool")]
			public int Size;
		}

		[SerializeField] private List<Pool> pools = new List<Pool>();
		private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

		private void Awake()
		{
			InitPool();
		}

		private void OnEnable()
		{
			LevelManager.OnLevelUnload += OnLevelUnload;
		}

		private void OnDisable()
		{
			LevelManager.OnLevelUnload -= OnLevelUnload;
		}

		private void InitPool()
		{
			foreach (var pool in pools)
				AddToPool(pool.Tag, pool.Prefab, pool.Size);
		}

		/// <summary>
		/// Creates a new pool with defined tag and object
		/// </summary>
		/// <param name="poolTag">Tag for spawning objects</param>
		/// <param name="prefab">Object to be pooled</param>
		/// <param name="count">Count of the pool</param>
		public void AddToPool(string poolTag, GameObject prefab, int count)
		{
			if (poolDictionary.ContainsKey(poolTag))
			{
				Debug.LogWarning(gameObject.name + ": \"" + poolTag + "\" Tag has already exists! Skipped.");
				return;
			}

			var queue = new Queue<GameObject>();
			for (int i = 0; i < count; i++)
			{
				var obj = Instantiate(prefab, transform);
				obj.SetActive(false);
				queue.Enqueue(obj);
			}

			poolDictionary.Add(poolTag, queue);
		}

		private void OnLevelUnload() => DisableAllPooledObjects();

		private void DisableAllPooledObjects()
		{
			foreach (var pool in poolDictionary.Values)
			{
				foreach (var go in pool)
				{
					go.transform.SetParent(transform);
					go.gameObject.SetActive(false);
				}
			}
		}

		/// <summary>
		/// Spawns the pooled object to given position
		/// </summary>
		/// <param name="poolTag">Tag of the object to be spawned</param>
		/// <param name="position">Set the world position of the object</param>
		/// <returns>The object found matching the tag specified</returns>
		public GameObject Spawn(string poolTag, Vector3 position)
		{
			var obj = SpawnFromPool(poolTag);

			obj.transform.position = position;
			return obj;
		}

		/// <summary>
		/// Spawns the pooled object to given position and parents the object to given Transform
		/// </summary>
		/// <param name="poolTag">Tag of the object to be spawned</param>
		/// <param name="position">Set the world position of the object</param>
		/// <param name="parent">Parent that will be assigned to the object</param>
		/// <returns>The object found matching the tag specified</returns>
		public GameObject Spawn(string poolTag, Vector3 position, Transform parent, bool worldPosition = true)
		{
			var obj = SpawnFromPool(poolTag);

			if (worldPosition)
				obj.transform.position = position;
			else
				obj.transform.localPosition = position;

			obj.transform.forward = parent.forward;
			obj.transform.SetParent(parent);
			return obj;
		}

		private GameObject SpawnFromPool(string poolTag)
		{
			if (!poolDictionary.ContainsKey(poolTag)) return null;

			var obj = poolDictionary[poolTag].Dequeue();
			obj.SetActive(false);
			obj.SetActive(true);
			poolDictionary[poolTag].Enqueue(obj);
			return obj;
		}
	}
}