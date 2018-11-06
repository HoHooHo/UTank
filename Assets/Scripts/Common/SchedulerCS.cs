using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

public class SchedulerInfo{
	public LuaFunction m_luaFunc;
	public float m_interval;
	public int m_repeat;
	public float m_delay;

	public float m_cInterval;
	public int m_cRepeat;
	public float m_cDelay;

	public SchedulerInfo(LuaFunction func, float interval, int repeat, float delay){
		m_luaFunc = func;
		m_interval = interval;
		m_repeat = repeat;
		m_delay = delay;

		m_cInterval = 0;
		m_cRepeat = 0;
		m_cDelay = 0;
	}
}

public class SchedulerCS : MonoBehaviour {

	private Dictionary<GameObject, List<SchedulerInfo>> m_data = new Dictionary<GameObject, List<SchedulerInfo>>();

	private Dictionary<GameObject, List<SchedulerInfo>> m_willRemove = new Dictionary<GameObject, List<SchedulerInfo>>();


	void CheckRemove(){
		foreach (var dic in m_willRemove) {
			GameObject go = dic.Key;
			List<SchedulerInfo> list = dic.Value;

			List<SchedulerInfo> dataList = null;
			if (m_data.TryGetValue (go, out dataList)) {

				foreach (var info in list) {
					dataList.Remove (info);
				}

				if (dataList.Count == 0) {
					m_data.Remove (go);
				}
			}
		}

		m_willRemove.Clear ();
	}

	void TryCall(SchedulerInfo info, GameObject go){
		if (!go.activeInHierarchy)
			return;

		if (info.m_cDelay < info.m_delay) {
			info.m_cDelay += Time.deltaTime;
		}else if (info.m_cInterval >= info.m_interval) {
			info.m_cInterval = 0;
			info.m_luaFunc.Call ();
			if (info.m_repeat > -1 && ++info.m_cRepeat >= info.m_repeat) {
				Unschedule (info, go);
			}
		} else {
			info.m_cInterval += Time.deltaTime;
		}
	}

	void Update () {
		CheckRemove ();

		foreach (var dic in m_data) {
			GameObject go = dic.Key;
			List<SchedulerInfo> list = dic.Value;

			foreach (SchedulerInfo info in list) {
				TryCall (info, go);
			}
		}
	}


	public SchedulerInfo Schedule(LuaFunction func, GameObject go, float interval, int repeat, float delay){
		if (func == null)
			return null;

		List<SchedulerInfo> list = null;
		if(!m_data.TryGetValue(go, out list)){
			list = new List<SchedulerInfo> ();
			m_data.Add (go, list);
		}

		SchedulerInfo info = new SchedulerInfo (func, interval, repeat, delay);

		list.Add (info);

		return info;
	}

	public SchedulerInfo ScheduleOnce(LuaFunction func, GameObject go, float delay){
		return Schedule (func, go, 0, 0, delay);
	}

	public void Unschedule(SchedulerInfo info, GameObject go){
		List<SchedulerInfo> list = null;
		if (!m_willRemove.TryGetValue (go, out list)) {
			list = new List<SchedulerInfo> ();
			m_willRemove.Add (go, list);
		}

		list.Add (info);
	}

	public void UnscheduleByTarget(GameObject go){

		List<SchedulerInfo> list = null;
		if (!m_data.TryGetValue (go, out list)) {
			return;
		}


		List<SchedulerInfo> rList = null;
		if (!m_willRemove.TryGetValue (go, out rList)) {
			rList = new List<SchedulerInfo> ();
			m_willRemove.Add (go, rList);
		}

		foreach (var info in list) {
			rList.Add (info);
		}
	}

	public void UnscheduleAll(){
		foreach (var dic in m_data) {
			GameObject go = dic.Key;
			List<SchedulerInfo> list = dic.Value;

			List<SchedulerInfo> rList = null;
			if (!m_willRemove.TryGetValue (go, out rList)) {
				rList = new List<SchedulerInfo> ();
				m_willRemove.Add (go, rList);
			}

			foreach (var info in list) {
				rList.Add (info);
			}
		}
	}
}
