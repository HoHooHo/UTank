using UnityEngine;
using System.Collections;

public class alienDrops : MonoBehaviour {

	public Transform dropPrefab;
	public Transform [] dropPoints;
	public Transform currDropPoint;
	public Vector2 timeRange;
	public float dropTime;
	//public bool drop;
	public bool firstPoint; //false by default

	void Start () 
	{
		dropTime = Random.Range (timeRange.x, timeRange.y);
		InvokeRepeating("DropNow", dropTime, dropTime);
	}
	
	void Update () 
	{
		if(firstPoint)
		{
			currDropPoint = dropPoints[0];
		}
		else
		{
			currDropPoint = dropPoints[1];
		}
	}
	void DropNow()
	{
		Instantiate(dropPrefab, currDropPoint.position, currDropPoint.rotation);
		dropTime = Random.Range (timeRange.x, timeRange.y);
		firstPoint = !firstPoint;
	}
}
