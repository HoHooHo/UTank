using UnityEngine;
using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AssetBundleInfo{
	public AssetBundle m_bundle;
	public int m_referencedCount;

	public AssetBundleInfo(AssetBundle ab){
		m_bundle = ab;
		m_referencedCount = 0;
	}
}

public class LoadAssetRequest{
	public Type m_assetType;
	public string[] m_assetNames;
	public LuaFunction m_luaFunc;
	public Action<UnityEngine.Object[]> m_sharpFunc;
}

public class ResManager : MonoBehaviour {
	string[] m_allManifest = null;
	AssetBundleManifest m_assetBundleManifest = null;
	private Dictionary<string, string[]> m_dependencies = new Dictionary<string, string[]> ();

	private Dictionary<string, AssetBundleInfo> m_loadedAssetBundles = new Dictionary<string, AssetBundleInfo>();
	private Dictionary<string, List<LoadAssetRequest>> m_loadRequests = new Dictionary<string, List<LoadAssetRequest>> ();

	public void Init(string manifestName, Action initOk = null, LuaFunction luaInitOk = null){
		LoadAsset<AssetBundleManifest> (manifestName, new string[] { "AssetBundleManifest" }, delegate(UnityEngine.Object[] objs) {
			if (objs.Length > 0) {
				m_assetBundleManifest = objs [0] as AssetBundleManifest;
				m_allManifest = m_assetBundleManifest.GetAllAssetBundles ();
			}

			if (initOk != null)
				initOk ();

			if (luaInitOk != null) {
				luaInitOk.Call ();
				luaInitOk.Dispose ();
			}
		});
	}

	public bool Retain(string abName){
		abName = GetRealAssetPath(abName);
		AssetBundleInfo abInfo = null;
		if (m_loadedAssetBundles.TryGetValue (abName, out abInfo)) {
			abInfo.m_referencedCount++;
			return true;
		}

		return false;
	}

	private void ReleaseDepend(string abName){
		string[] dependencies = null;
		if(m_dependencies.TryGetValue(abName, out dependencies)){
			foreach (string dependABName in dependencies) {
				Release(dependABName);
			}
		}
	}

	public bool Release(string abName){
		abName = GetRealAssetPath(abName);
		AssetBundleInfo abInfo = null;
		if(m_loadedAssetBundles.TryGetValue(abName, out abInfo)){
			if (--abInfo.m_referencedCount <= 0 && !m_loadRequests.ContainsKey(abName)) {
				ReleaseDepend (abName);
				abInfo.m_bundle.Unload (false);
				m_loadedAssetBundles.Remove (abName);
				m_dependencies.Remove (abName);
			}

			Debug.LogWarning ("=== Name: " + abName + "  ReferencedCount: " + abInfo.m_referencedCount);

			return true;
		}

		return false;
	}


	private string GetRealAssetPath(string abName) {
		/*
		abName = abName.ToLower ();
		if(!abName.EndsWith(AppConst.ExtName)){
			abName += AppConst.ExtName;
		}

		if (abName.Contains ("/")) {
			return abName;
		}

		foreach(string manifest in m_allManifest){
			if (manifest.EndsWith (abName)) {
				return manifest;
			}
		}

		Debug.LogError ("GetRealAssetPath Error--->>> " + abName);
		*/

		return abName;
	}

	private AssetBundleInfo GetLoadedAssetBundle(string abName){
		AssetBundleInfo abInfo = null;

		m_loadedAssetBundles.TryGetValue (abName, out abInfo);

		return abInfo;
	}

	private IEnumerator OnLoadAssetBundle(string abName, Type type){
		WWW www = null;
		string persistentPath= FileUtil.Instance.StoragePath + abName;

		if (File.Exists (persistentPath)) {
			Debug.Log ("AB Path = " + persistentPath);
			www = new WWW (FileUtil.Instance.GetURL(persistentPath));
		} else {
			string streamingPath= FileUtil.Instance.AppPath + abName;

			Debug.Log ("AB Path = " + streamingPath);
			string mm = FileUtil.Instance.GetURL (streamingPath);
			www = new WWW (FileUtil.Instance.GetURL(streamingPath));
		}

		/*
		#if UNITY_EDITOR
		AssetBundleInfo abInfo = new AssetBundleInfo(Resources.LoadAssetAtPath(abName, type));
		m_loadedAssetBundles.Add(abName, abInfo);
		#else*/
		//WWW www = new WWW (abName);
		yield return www;
		AssetBundle assetObj = www.assetBundle;
		if (assetObj != null) {
			m_loadedAssetBundles.Add (abName, new AssetBundleInfo (www.assetBundle));
		}

		www.Dispose ();

		//#endif
	}

	private IEnumerator OnLoadAsset<T>(string abName) where T : UnityEngine.Object{
		AssetBundleInfo bundleInfo = GetLoadedAssetBundle (abName);
		if (bundleInfo == null) {
			yield return StartCoroutine (OnLoadAssetBundle(abName, typeof(T)));

			bundleInfo = GetLoadedAssetBundle (abName);
			if (bundleInfo == null) {
				m_loadRequests.Remove (abName);
				Debug.LogError ("OnLoadAsset --->>> " + abName);
				yield break;
			}
		}

		List<LoadAssetRequest> list = null;
		if (!m_loadRequests.TryGetValue (abName, out list)) {
			m_loadRequests.Remove (abName);
			yield break;
		}

		//foreach(LoadAssetRequest lar in list){
		for(int m = 0; m < list.Count; m++){
			LoadAssetRequest lar = list [m];
			string[] assetNames = lar.m_assetNames;

			List<UnityEngine.Object> result = new List<UnityEngine.Object> ();

			AssetBundle bundle = bundleInfo.m_bundle;

			for (int i = 0; i < assetNames.Length; i++) {
				string assetName = assetNames [i];

				AssetBundleRequest request = bundle.LoadAssetAsync (assetName, lar.m_assetType);
				yield return request;

				result.Add (request.asset);
			}

			if (lar.m_sharpFunc != null) {
				lar.m_sharpFunc (result.ToArray());
				lar.m_sharpFunc = null;
			}

			if (lar.m_luaFunc != null) {
				lar.m_luaFunc.BeginPCall ();
				lar.m_luaFunc.Push (result.ToArray ());
				lar.m_luaFunc.PCall ();
				lar.m_luaFunc.EndPCall ();
				lar.m_luaFunc.Dispose ();
				lar.m_luaFunc = null;
			}

			bundleInfo.m_referencedCount++;
		}

		Debug.LogWarning ("=== Name: " + abName + "  ReferencedCount: " + bundleInfo.m_referencedCount);

		m_loadRequests.Remove (abName);
	}

	private void RealLoadAsset<T>(string abName, string[] assetNames, Action<UnityEngine.Object[]> sharpFunc = null, LuaFunction LuaFunc = null) where T : UnityEngine.Object{
		LoadAssetRequest request = new LoadAssetRequest ();
		request.m_assetType = typeof(T);
		request.m_assetNames = assetNames;
		request.m_sharpFunc = sharpFunc;
		request.m_luaFunc = LuaFunc;

		List<LoadAssetRequest> list = null;
		if(m_loadRequests.TryGetValue(abName, out list)){
			list.Add(request);
		}else{
			list = new List<LoadAssetRequest> ();
			list.Add (request);
			m_loadRequests.Add (abName, list);

			StartCoroutine (OnLoadAsset<T>(abName));
		}
	}

	private void LoadAsset<T>(string abName, string[] assetNames, Action<UnityEngine.Object[]> sharpFunc = null, LuaFunction LuaFunc = null) where T : UnityEngine.Object{
		abName = GetRealAssetPath(abName);

		if (typeof(T) == typeof(AssetBundleManifest)) {
			RealLoadAsset<AssetBundleManifest> (abName, assetNames, sharpFunc, LuaFunc);
		} else {
			string[] dependencies = m_assetBundleManifest.GetAllDependencies (abName);
			if (dependencies.Length > 0) {
				int complete = 0;
				foreach (string dependABName in dependencies) {
					LoadAsset<GameObject> (GetRealAssetPath(dependABName), new string[] { }, delegate {
						if (++complete >= dependencies.Length) {
							m_dependencies.Add(abName, dependencies);
							RealLoadAsset<GameObject> (abName, assetNames, sharpFunc, LuaFunc);
						}
					});
				}
			} else {
				RealLoadAsset<GameObject> (abName, assetNames, sharpFunc, LuaFunc);
			}
		}
	}

	public void LoadPrefab(string abName, string assetName, Action<UnityEngine.Object[]> func = null){
		LoadAsset<GameObject> (abName, new string[] {assetName}, func);
	}

	public void LoadPrefab(string abName, string[] assetNames, Action<UnityEngine.Object[]> func = null){
		LoadAsset<GameObject> (abName, assetNames, func);
	}

	public void LoadPrefab(string abName, string[] assetNames, LuaFunction func = null){
		LoadAsset<GameObject> (abName, assetNames, null, func);
	}

	public void UnloadPrefab(string abName){
		Release (abName);
	}

	public void clear(){

	}
}
