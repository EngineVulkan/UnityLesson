using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : PersistableObject
{
	// Start is called before the first frame update
	int shapeId = int.MinValue;
	Color color;
	MeshRenderer meshRenderer;
	static int colorPropertyId = Shader.PropertyToID("_Color");
	static MaterialPropertyBlock sharedPropertyBlock;
	private void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();
	}
	void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public int ShapeId
	{
		get
		{
			return shapeId;
		}
		set
		{
			if (shapeId == int.MinValue || value != int.MaxValue)
			{
				shapeId = value;
			}
			else
			{
				Debug.Log("");
			}
		}
	}

	public int MateiralId { get; private set; }
	public void SetMaterial(Material material,int materialId)
	{
		meshRenderer.material = material;
		MateiralId = materialId;
	}

	public void SetColor(Color color)
	{
		this.color = color;
		if (sharedPropertyBlock == null)
		{
			sharedPropertyBlock = new MaterialPropertyBlock();
		}

		sharedPropertyBlock.SetColor(colorPropertyId,color);
		//meshRenderer.material.color = color;
		meshRenderer.SetPropertyBlock(sharedPropertyBlock);
	}

	public override void Save(GameDataWriter writer)
	{
		base.Save(writer);
		writer.Write(color);
	}

	public override void Load(GameDataReader reader)
	{
		base.Load(reader);
		SetColor(reader.Version>0?reader.ReadColor():Color.white);
	}
}
