using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
	public GameObject objectToPool;
	public int amountToPool;
	public bool shouldExpand = true;

	public ObjectPoolItem(GameObject objToPool, int amt, bool exp = true)
	{
		objectToPool = objToPool;
		amountToPool = Mathf.Max(amt, 2);
		shouldExpand = exp;
	}
}

public class ObjectPooler : MonoBehaviour
{
	public static ObjectPooler Instance;
	/// <summary>
	/// These are the prefabs to pool.
	/// </summary>
	public List<ObjectPoolItem> itemsToPool;

	/// <summary>
	/// Just ALL objects (active and inactive held by ObjectPooler class).
	/// </summary>
	public List<List<GameObject>> pooledObjectsList;
	/// <summary>
	/// This is helping list which will be added to pooledObjectsList afterwards.
	/// Only used in ObjectPoolItemToPooledObject().
	/// </summary>
	public List<GameObject> pooledObjects;
	/// <summary>
	/// Holds an index of last active item as far as I understood.
	/// </summary>
	List<int> positions;

	void Awake()
	{
		Instance = this;

		pooledObjectsList = new List<List<GameObject>>();
		pooledObjects = new List<GameObject>();
		positions = new List<int>();

		for (int i = 0; i < itemsToPool.Count; i++)
		{
			ObjectPoolItemToPooledObject(i);
		}
	}

	void OnDestroy()
	{
		Instance = null;
	}


	public GameObject GetPooledObject(int index)
	{
		int curSize = pooledObjectsList[index].Count;
		for (int i = positions[index] + 1; i < positions[index] + curSize; i++)
		{
			if (!pooledObjectsList[index][i % curSize].activeInHierarchy)
			{
				positions[index] = i % curSize;
				return pooledObjectsList[index][i % curSize];
			}
		}

		if (itemsToPool[index].shouldExpand)
		{
			GameObject obj = Instantiate(itemsToPool[index].objectToPool);
			obj.SetActive(false);
			obj.transform.parent = this.transform;
			pooledObjectsList[index].Add(obj);

			return obj;
		}
		return null;
	}

	public List<GameObject> GetAllPooledObjects(int index)
	{
		return pooledObjectsList[index];
	}


	public int AddObject(GameObject GO, int amt = 3, bool exp = true)
	{
		ObjectPoolItem item = new ObjectPoolItem(GO, amt, exp);
		int currLen = itemsToPool.Count;
		itemsToPool.Add(item);
		ObjectPoolItemToPooledObject(currLen);
		return currLen;
	}


	void ObjectPoolItemToPooledObject(int index)
	{
		ObjectPoolItem item = itemsToPool[index];

		pooledObjects = new List<GameObject>();
		
		//Here we instantiate all objects and deactivate.
		for (int i = 0; i < item.amountToPool; i++)
		{
			GameObject obj = Instantiate(item.objectToPool);
			obj.SetActive(false);
			obj.transform.parent = this.transform;
			pooledObjects.Add(obj);
		}


		pooledObjectsList.Add(pooledObjects);
		positions.Add(0);
	}
}
