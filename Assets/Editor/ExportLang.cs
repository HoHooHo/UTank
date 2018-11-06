using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class ExportLang {
	private static string UIPrefabPath = Application.dataPath + "/Prefabs";

	private static string OutFile = Application.dataPath + "/Lua/Lang.lua";

	private static List<string> list = null;

	[MenuItem("Lang/Export Lang")]
	static void Export(){
		list = new List<string>();

		LoadDirectoryPrefab (new DirectoryInfo(UIPrefabPath));

		if (File.Exists (OutFile)) {
			File.Delete (OutFile);
		}

		writeLua ();
	}

	public static void LoadDirectoryPrefab(DirectoryInfo info){
		if (!info.Exists)
			return;

		FileInfo[] fileInfos = info.GetFiles ("*.prefab", SearchOption.AllDirectories);

		foreach (FileInfo file in fileInfos) {
			string path = file.FullName;
			path = path.Replace ("\\", "/");
			int index = path.IndexOf ("Assets/");

			string assetPath = path.Substring (index);

			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject> (assetPath);
			GameObject go = GameObject.Instantiate (prefab);
			SearchPrefabString (go.transform, assetPath, "");

			GameObject.DestroyImmediate (go);
		}
	}

	public static void SearchPrefabString(Transform root, string file, string widget){
		foreach(Transform child in root){
			Text label = child.GetComponent<Text> ();

			if (label != null) {
				string text = label.text;
				text = text.Replace ("\n", "\\n");

				if(!string.IsNullOrEmpty(text) && !list.Contains(text)){
					list.Add (text);
				}

				LangHelperCS helper = child.GetComponent<LangHelperCS> ();
				if (helper == null) {
					Debug.LogWarning ("WARNNING ===>>> " + "Widget [" + widget + "/" + child.name + "] in File [" + file + "] has not LangHelper Component");
				}
			}

			if (child.childCount > 0) {
				SearchPrefabString (child, file, widget + "/" + child.name);
			}
		}
	}

	public static void writeLua(){

		StreamWriter sw;
		FileInfo info = new FileInfo (OutFile);

		if (!info.Exists) {
			sw = info.CreateText ();
		} else {
			sw = info.AppendText ();
		}

		foreach (string text in list) {
			sw.WriteLine ("Lang[\"" + text + "\"] = \"" + text + "\"");
		}


		sw.Close ();
		sw.Dispose ();
	}
}
