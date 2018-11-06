using UnityEngine;
using System.Collections;

public class Util {

	private static Util s_util = null;

	public static Util Instance {
		get {
			if (s_util == null) {
				s_util = new Util ();
			}
			return s_util;
		}
	}
}
