using UnityEngine;
using System.Collections;

public class controlTwinkle_final : MonoBehaviour {

	public bool switched;
	public float frequency = 0.2f;
	public Texture2D origAlbedo;
	public Texture2D newAlbedo;

	void Start () 
	{
		InvokeRepeating ("Switch", frequency, frequency);
	}
	
	void Update () 
	{
		if(switched)
		{
			GetComponent<Renderer>().material.SetTexture("_MainTex", newAlbedo);
		}
		else
		{
			GetComponent<Renderer>().material.SetTexture("_MainTex", origAlbedo);
		}
	}

	void Switch ()
	{
		switched = !switched;
	}
}
