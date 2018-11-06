using UnityEngine;
using System.Collections;

public class LuaHelperCS {
	private static LuaHelperCS s_helper = null;

	public static LuaHelperCS Instance{
		get{
			if (s_helper == null) {
				s_helper = new LuaHelperCS ();
			}

			return s_helper;
		}
	}




	private static ResManager m_resManager = null;
	private static SchedulerCS m_scheduler = null;



	public void Init(GameObject gameObject){
		m_resManager = gameObject.AddComponent<ResManager> ();
		m_resManager.Init ("StreamingAssets");
		m_scheduler = gameObject.AddComponent<SchedulerCS> ();
	}

	public void Uninit(){
		m_resManager.clear ();
	}


	public ResManager ResManager{
		get{
			return m_resManager;
		}
	}


	public SchedulerCS Scheduler{
		get{
			return m_scheduler;
		}
	}
}
