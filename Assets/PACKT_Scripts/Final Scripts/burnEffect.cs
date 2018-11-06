using UnityEngine;
using System.Collections;

public class burnEffect : MonoBehaviour {

	public float cutoffValue = 0.95f;
	public float cutoffMin = 0.5f;
	public Transform burnPrefab;
	public bool burning;
	public float burnRate = 0.005f; 
	public Transform newBurnQuad;

	void Update () 
	{
		if (burning) 
		{
			Renderer burnRenderer = newBurnQuad.GetComponent <MeshRenderer> ();
			burnRenderer.material.SetFloat ("_Cutoff", cutoffValue);
			if (burnRate < 1f)
			{
				burnRate += 0.001f * Time.deltaTime;
			}
			if(cutoffValue >= cutoffMin)
			{
				cutoffValue -= burnRate; 
			}
			else
			{
				cutoffValue = cutoffMin;
				burning = false;
			}

		}

	}

	void OnCollisionEnter (Collision collision) 
	{
		ContactPoint contact = collision.contacts[0];
		Vector3 pos = contact.point;
		GetComponent<Rigidbody>().detectCollisions = false;
		GetComponent<MeshRenderer>().enabled = false;
		newBurnQuad = Instantiate (burnPrefab, pos, Quaternion.Euler(90f, 0f, 0f))as Transform;
		burning = true;
	}

}
