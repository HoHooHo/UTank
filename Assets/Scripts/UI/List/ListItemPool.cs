using System;
using System.Collections.Generic;
using UnityEngine;


public class ListItemPool
{
	private Queue<RectTransform> _pool;
	public ListItemPool ()
	{
		_pool = new Queue<RectTransform> ();
	}

	public RectTransform DequeueReusableItem(RectTransform itemPrefab)
	{
		if (_pool.Count == 0) {
			RectTransform itemTransform = RectTransform.Instantiate (itemPrefab) as RectTransform;
            itemTransform.name = itemPrefab.name;
			return itemTransform;
		} else {
			return _pool.Dequeue ();
		}
	}

	public void EnqueueReusableItem(RectTransform itemTransform)
	{
		_pool.Enqueue(itemTransform);
	}
}


