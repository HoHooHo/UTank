using UnityEngine;
using System.Collections;

public class storm : MonoBehaviour {

	public float speed = 100f;

	void Start () {
	
	}
	
	void Update () 
	{
		transform.RotateAround(transform.position, transform.forward, Time.deltaTime * speed);
	}
}
