using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Dk.Asset;

namespace Dk.Loader
{
	public class DkLoadManager:MonoBehaviour
	{
		private Dictionary<string,DkLoaderMission> loadingMap = new Dictionary<string,DkLoaderMission>();

		private static DkLoaderAssist loadAssist = null;

		private static DkLoadManager s_instance = null;
		
		public static DkLoadManager Instance
		{
			get
			{
				if (!s_instance)
				{
					s_instance = GameObject.FindObjectOfType(typeof(DkLoadManager)) as DkLoadManager;
					if (!s_instance)
					{
						GameObject container = new GameObject();
						container.name = "DkLoadManager";
						s_instance = container.AddComponent(typeof(DkLoadManager)) as DkLoadManager;
						if(loadAssist == null)
						{
							loadAssist = container.AddComponent(typeof(DkLoaderAssist)) as DkLoaderAssist;
						}
					}
				}
				
				return s_instance;
			}
		}

		public void Awake()
		{
			if(s_instance == null)
			{
				s_instance = this;
			}
			
			DontDestroyOnLoad(this);
		}

		public void Destroy()
		{
			s_instance = null;
			loadAssist = null;
		}
		
		public eDkDownLoadstatus GetDownStatus()
		{
			if(((int)eDkDownLoadstatus.EMPTY & (int)loadAssist.status) == (int)eDkDownLoadstatus.EMPTY)
			{
				return eDkDownLoadstatus.EMPTY;
			}
			else return eDkDownLoadstatus.LOADING;
		}
		
		public bool AddLoadMission(DkLoaderMission item)
		{
			if(EnableAddMission(item.path))
			{
				loadingMap.Add(item.path,item);
				loadAssist.AddLoadMission(item);
				return true;
			}
			else return false;
		}

		public void RemoveMission(string path)
		{
			if(!loadingMap.ContainsKey(path))
			{
				loadingMap.Remove(path);
			}
		}
				
		public bool EnableAddMission(string path)
		{
			if(loadingMap.ContainsKey(path) ||  DkResourceManager.Instance.ContainsLoadItem(path))
			{
				return false;
			}
			else return true;
		}
	}
}