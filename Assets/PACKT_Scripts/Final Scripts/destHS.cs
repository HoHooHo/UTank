using UnityEngine;
using System.Collections;

public class destHS : MonoBehaviour {

	//public Vector2 [] UVTiles;//this should just be a bool

	public Vector2 [] UVOffsets;
	public int currArrayPos;
	public float interval = 0.1f;

	public Renderer rend;

	public bool UVTileSwitch;
	public float currTile;

	void Start () 
	{
		rend = GetComponent<Renderer>();
		InvokeRepeating("NextStage", 1, interval);
	}
	
	void Update () 
	{
		if(UVTileSwitch)
		{
			currTile = 0.5f;
		}
		else
		{
			currTile = 0.25f;
		}
	}

	void NextStage()
	{
		UVTileSwitch = !UVTileSwitch;
		currArrayPos++;
		if(currArrayPos > UVOffsets.Length-1)
		{
			currArrayPos = 0;
		}
		//rend.Material.UVOffset(UVOffsets[currArrayPos]);
		rend.material.SetTextureOffset("_MainTex", UVOffsets[currArrayPos]);

		//rend.Material.UVTiling(UVTiles[currArrayPos]);
		//rend.material.SetTextureScale("_MainTex", UVTiles[currArrayPos]);
		rend.material.SetTextureScale("_MainTex", new Vector2(currTile, currTile));


	}
}
