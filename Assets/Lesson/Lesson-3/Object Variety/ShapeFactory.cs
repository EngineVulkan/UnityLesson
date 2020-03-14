using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class ShapeFactory:ScriptableObject
{
	[SerializeField]
	Shape[] prefabs;

	[SerializeField]
	Material[] materials;

	[SerializeField]
	bool recyle;

	List<Shape>[] pools;
	Scene poolScenes;
	

	public Shape Get(int shapeId=0,int materialId=0)
	{
		Shape instance;
		if (recyle)
		{
			if (pools == null)
			{
				CreatPools();
			}
			List<Shape> pool = pools[shapeId];
			int lastIndex = pool.Count - 1;
			if (lastIndex >= 0)
			{
				instance = pool[lastIndex];
				instance.gameObject.SetActive(true);
				pool.RemoveAt(lastIndex);
			}
			else
			{
				instance = Instantiate(prefabs[shapeId]);
				instance.ShapeId = shapeId;
			}
		}
		else
		{
			instance = Instantiate(prefabs[shapeId]);
			instance.ShapeId = shapeId;
			SceneManager.MoveGameObjectToScene(instance.gameObject,poolScenes);
		}
		instance.SetMaterial(materials[materialId],materialId);
		return instance;
	}

	public void Reclaim(Shape shapeToRecycle)
	{
		if (recyle)
		{
			if (pools == null)
			{
				CreatPools();
			}
			pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);

			shapeToRecycle.gameObject.SetActive(false);
		}
		else
		{
			Destroy(shapeToRecycle.gameObject);
		}
	}

	public Shape GetRandom()
	{
		return Get(Random.Range(0,prefabs.Length),Random.Range(0,materials.Length));
		
	}

	void CreatPools()
	{
		pools = new List<Shape>[prefabs.Length];
		for (int i = 0; i < pools.Length; i++)
		{
			pools[i] = new List<Shape>();
		}
		if (Application.isEditor)
		{
			poolScenes = SceneManager.GetSceneByName(name);
			if (poolScenes.isLoaded)
			{
				GameObject[] rootGameObject = poolScenes.GetRootGameObjects();
				for (int i = 0; i < rootGameObject.Length; i++)
				{
					Shape poolShape = rootGameObject[i].GetComponent<Shape>();
					if (!poolShape.gameObject.activeSelf)
					{
						pools[poolShape.ShapeId].Add(poolShape);
					}
				}
				return;
			}
		}
		poolScenes = SceneManager.CreateScene(name);
	}

	
}
