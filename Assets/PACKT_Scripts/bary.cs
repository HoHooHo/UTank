using UnityEngine;
using System.Collections;

public class bary : MonoBehaviour {

public Vector2[] GetBarycentricFixed()
{
	Vector2[] uvs =
		new Vector2[4];
	
	const int topLeft = 0;
	const int topRight = 1;
	const int bottomLeft = 2;
	const int bottomRight = 3;
	
	uvs[topLeft] = new Vector2(0,0);
	uvs[topRight] = new Vector2(1,0);
	uvs[bottomLeft] = new Vector2(0,1);
	uvs[bottomRight] = new Vector2(1,1);
	
	return uvs;
}
}
