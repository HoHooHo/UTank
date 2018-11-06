using UnityEngine;
using System.Collections;

public class controlTwinkle01 : MonoBehaviour {

	public bool switched;
	public float frequency = 0.2f;
	public Texture2D origEmit;
	public Texture2D newEmit;

	void Start () 
	{
		InvokeRepeating ("Switch", frequency, frequency);
	}
	
	void Update () 
	{
		if(switched)
		{
			GetComponent<Renderer>().material.SetTexture("_EmissionMap", newEmit);
		}
		else
		{
			GetComponent<Renderer>().material.SetTexture("_EmissionMap", origEmit);
		}
	}

	void Switch ()
	{
		switched = !switched;
	}
}
