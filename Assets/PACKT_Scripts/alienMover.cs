using UnityEngine;
using System.Collections;

public class alienMover : MonoBehaviour {
	public Transform target;
	public float strength = 0.5f;

	void LateUpdate () 
	{
		Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
		float str = Mathf.Min (strength * Time.deltaTime, 1f);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, str);

	}

}
