using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[DisallowMultipleComponent]
public class PersistableObject : MonoBehaviour
{
	// Start is called before the first frame update
	int loadingLevelBuildIndex;
	private void Awake()
	{
		if (Application.isEditor)
		{
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene level = SceneManager.GetSceneAt(i);
				if (level.name.Contains("Level "))
				{
					loadingLevelBuildIndex = level.buildIndex;
					SceneManager.SetActiveScene(level);
					return;
				}
			}
		}
		StartCoroutine(LoadLevel(1));
	}
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public IEnumerator LoadLevel(int levelBuildIndex)
	{
		enabled = false;
		if (loadingLevelBuildIndex > 0)
		{
			SceneManager.UnloadSceneAsync(levelBuildIndex);
		}
		yield return SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive);
		SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
		loadingLevelBuildIndex = levelBuildIndex;
		enabled = true;
	}

	public virtual void Save(GameDataWriter writer)
	{
		writer.Write(transform.localPosition);
		writer.Write(transform.localRotation);
		writer.Write(transform.localScale);
	}


	public virtual void Load(GameDataReader reader)
	{
		transform.localPosition = reader.ReadVector3();
		transform.localRotation = reader.ReadQuaternion();
		transform.localScale = reader.ReadVector3();
	}
}
