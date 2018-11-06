using UnityEngine;
using System.Collections;

public class crateNumbers : MonoBehaviour {
	public GameObject [] crateObjects;
	public Texture2D [] crateTextures;
	public int arrayIndex;

	void Start () 
	{
		foreach (GameObject crate in crateObjects)
		{
			crate.GetComponent<Renderer>().material.SetTexture("_DecalTex", crateTextures[arrayIndex]);
			arrayIndex++;
		}
	}

}
