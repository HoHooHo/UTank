using UnityEngine;
using System.Collections;

public class planetSurf : MonoBehaviour 
{
	public Texture2D [] planetTextures;
	public Texture2D currTexture;
	public int textureIndex;
	public float timer;
	public float switchTime = .2f;

	void Start () 
	{
		//textureIndex = 0;
		//InvokeRepeating ("ChangeTexture", 1f, 1f);
	}

	void Update()
	{
		currTexture = planetTextures[textureIndex];
		timer += Time.deltaTime;
		if (timer > switchTime)
		{
			ChangeTexture ();
		}
		if(textureIndex > planetTextures.Length-1)
		//if(textureIndex > 2)
		{
			textureIndex = 0;
		}
	}

	void ChangeTexture()
	{
		timer = 0f;
		textureIndex++;
		GetComponent<Renderer>().material.mainTexture = currTexture;
	}

	

}
