using UnityEngine;
using System.Collections;

public class textAnim : MonoBehaviour {

	Vector2 uvOffset = Vector2.zero;
	//public string textureName = "_MainTex";
	public Vector2 uvAnimSpeed = new Vector2( 0.0f, 0.1f );

	void LateUpdate () 
	{
		uvOffset += (uvAnimSpeed * Time.deltaTime);
		//if( GetComponent<Renderer>().enabled )
		//{
			GetComponent<Renderer>().material.SetTextureOffset("_MainTex", uvOffset );
		//}

	}
}
