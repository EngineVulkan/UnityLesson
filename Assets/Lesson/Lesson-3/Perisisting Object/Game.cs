using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Game : PersistableObject
{

	public float CreationSpeed { get; set; }
	public float DestructionSpeed { get; set; }
	public ShapeFactory shapeFactory;
	public PersistentStorage storage;
	public KeyCode createKey = KeyCode.C;
	public KeyCode newGameKey = KeyCode.N;
	public KeyCode savekey = KeyCode.S;
	public KeyCode loadKey = KeyCode.L;
	public KeyCode DestroyKey = KeyCode.X;
	public int levelCount;


	float creationProgress, destructionProgress;
	List<PersistableObject> objects;
	List<Shape> shapes;
	string savePath;

	const int saveVersion = 2;
    int loadingLevelBuildIndex;

    private void Awake()
	{
		objects = new List<PersistableObject>();
		shapes = new List<Shape>();

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
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(createKey))
		{
			CreateShape();
		}
		else if (Input.GetKeyDown(newGameKey))
		{
			BeginNewGame();
		}
		else if (Input.GetKeyDown(savekey))
		{
			storage.Save(this, saveVersion);
			//Save();
		}
		else if (Input.GetKeyDown(loadKey))
		{
			BeginNewGame();
			storage.Load(this);
			//Load();
		}
		else if (Input.GetKeyDown(DestroyKey))
		{
			DestroyShape();
		}
		else
		{
			for (int i = 1; i <= levelCount; i++)
			{
				if (Input.GetKeyDown(KeyCode.Alpha0 + i))
				{
					BeginNewGame();
					StartCoroutine(LoadLevel(i));
					return;
				}
			}
		}
		creationProgress += Time.deltaTime * CreationSpeed;
		while (creationProgress >= 1f)
		{
			creationProgress -= 1f;
			CreateShape();
		}
		destructionProgress += Time.deltaTime * DestructionSpeed;
		while (destructionProgress >= 1f)
		{
			destructionProgress -= 1f;
			DestroyShape();
		}
    }

	void BeginNewGame()
	{
		for (int i = 0; i < objects.Count; i++)
		{
			Destroy(objects[i].gameObject);
		}
		objects.Clear();
	}

	void CreateObject()
	{
		Shape o = shapeFactory.GetRandom();
		Transform t = o.transform;
		t.localPosition = Random.insideUnitSphere * 5f;
		t.localRotation = Random.rotation;
		t.localScale = Vector3.one * Random.Range(0.1f,1f);

		objects.Add(o);
	}

	void CreateShape()
	{
		Shape instance = shapeFactory.GetRandom();
		Transform t = instance.transform;
		t.localPosition = Random.insideUnitSphere * 5f;
		t.localRotation = Random.rotation;
		t.localScale = Vector3.one * Random.Range(0.1f, 1f);
		instance.SetColor(Random.ColorHSV(0f,1f,0.5f,1f,0.25f,1f,1f,1f));

		shapes.Add(instance);

	}

	void DestroyShape()
	{
		if (shapes.Count > 0)
		{
			int index = Random.Range(0,shapes.Count);
			//Destroy(shapes[index].gameObject);
			shapeFactory.Reclaim(shapes[index]);
			shapes.RemoveAt(index);
		}
	}

	public override void Save(GameDataWriter writer)
	{
		writer.Write(shapes.Count);
		writer.Write(loadingLevelBuildIndex);
		for (int i = 0; i < shapes.Count; i++)
		{
			writer.Write(shapes[i].ShapeId);
			shapes[i].Save(writer);
		}
	}

	public override void Load(GameDataReader reader)
	{
		int version = reader.ReadInt();
		int count = version <= 0 ? -version : reader.ReadInt();
		int materialId = version > 0 ? reader.ReadInt() : 0;
		StartCoroutine(LoadLevel(version < 2 ? 1 : reader.ReadInt()));
		for (int i = 0; i < count; i++)
		{
			int shapeId = reader.ReadInt();
			Shape instance = shapeFactory.Get(shapeId,materialId);
			instance.Load(reader);
			objects.Add(instance);
		}
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
}
