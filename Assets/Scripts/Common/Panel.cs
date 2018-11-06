using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

[AddComponentMenu("UI/Custom/Panel")]
public class Panel : Graphic {

	public bool test = false;

	protected override void UpdateMaterial ()
	{
		if (test) {
			base.UpdateMaterial ();
		}
	}
}
