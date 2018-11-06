using UnityEngine;
using System.Collections;

public class cloudAnim_final : MonoBehaviour {
	public float atlasPosition; //the starting vertical UV value - to be randomized.
	public float currOpacity;
	public float fadeSpeed;
	public bool ping; //to pingpong the transparency effect

	public Renderer rend;
	public float [] vValue = {0f, 0.25f, 0.5f, 0.75f};

	void Start () 
	{
		rend = GetComponent<Renderer>();//set startOpacity
		currOpacity = Random.Range (0, 0.95f);
		atlasPosition = vValue[Random.Range(0, 3)]; //this should return a float
		rend.material.SetTextureOffset("_MainTex", new Vector2(0, atlasPosition));
	}
	
	void Update () 
	{
		Color currColor = new Color(1,1,1,currOpacity);
		rend.material.SetColor("_Color", currColor);
		if(ping)
		{
			fadeSpeed = 0.35f;
		}
		else
		{
			fadeSpeed = -0.35f;
		}
		currOpacity += Time.deltaTime * fadeSpeed;
		if(currOpacity > 1f)
		{
			ping = !ping;
		}
		else if (currOpacity < 0f)
		{
			ping = !ping;
			ChangeVPos();
		}
	}

	void ChangeVPos()
	{
		atlasPosition = vValue[Random.Range(0, 3)];
		rend.material.SetTextureOffset("_MainTex", new Vector2(0, atlasPosition));
	}
}
