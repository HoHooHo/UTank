using UnityEngine;
using System.Collections;

public class orbitTrail : MonoBehaviour {

	//set up this script to draw a line with four (or more existing points)

	public Vector3 [] shipPositions;
	public int currPosition;
	public float interval = 1f;
	public int startPosition;

	public LineRenderer trajLine;

	void Start () 
	{
		trajLine = GetComponent<LineRenderer>();
		InvokeRepeating("NextPosition", 2, interval);
	}
	
	/*void Update () 
	{
		if(currPosition > shipPositions.Length-1)
		{
			currPosition = 0;
		}
	}*/

	void NextPosition()
	{
		currPosition++;
		if(currPosition > shipPositions.Length-1)
		{
			currPosition = 0;
			startPosition = 0;
		}
		else
		{
			startPosition = currPosition-1;
		}
		//transform.position = shipPositions[currPosition];
		trajLine.SetPosition(0,shipPositions[startPosition]);
		trajLine.SetPosition(1, shipPositions[currPosition]);


	}
}
