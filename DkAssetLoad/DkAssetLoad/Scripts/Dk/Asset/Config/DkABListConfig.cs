using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dk.Util;
using Dk.Loader;
using Dk.Event;

namespace Dk.Asset
{
	public class DkABListConfig : DkEventDispatch
	{
		public static string configName = "AllABList.bytes";
		
		private bool inited = false;

		private bool loaded = false;

		private Dictionary<string,DkABItem> guidDict = new Dictionary<string, DkABItem>();
		
		private static DkABListConfig s_instance = null;
		
		public static DkABListConfig Instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = new DkABListConfig();
				}
				
				return s_instance;
			}
		}
		
		public void SetConfigName(string name)
		{
			configName = name;
		}
		
		public static string GetConfigLoadPath()
		{
			return DkLoadPathUtil.GetConfigPath(configName);
		}
		
		public bool Initialize()
		{
			if(inited == false)
			{
				if(DkGlobal.GetSourceDataEnable())
				{
					onInitComplete();
				}
				else
				{
					StartLoad();
				}
			}
			else
			{
				onInitComplete();
			}

			return this.inited;
		}
		
		private void onInitComplete()
		{
			if(inited == false)
			{
				inited = true;
				this.DispatchEvent(new DkEvent(DkEventDef.DATA_INIT));
			}
		}
		
		public bool GetInited
		{
			get
			{
				return inited;
			}
		}
		
		private void StartLoad()
		{
			if(loaded == false)
			{
				loaded = true;
				guidDict.Clear();
				DkLoaderMission mission = new DkLoaderMission();
				mission.path = GetConfigLoadPath();
				mission.type = eDkWWWItemType.BYTES;
				mission.protocal = eDkLoadProtocal.WEB;
				
				Action<DkEvent> action = onLoadComplete;
				mission.AddEventListen(DkEventDef.LOAD_COMPLETE,action);
				mission.AppendToLoading();
			}
		}
		
		protected void onLoadComplete(DkEvent evt)
		{
			DkLoaderMission mission = evt.target as DkLoaderMission;
			mission.Destroy();

			DkResourceLoadItem item = DkResourceManager.Instance.GetLoadItem(GetConfigLoadPath());
			if(item != null)
			{
				byte[] bytes = item.obj as byte[];
				string text = System.Text.Encoding.Default.GetString(DkZipUtil.unGZip(bytes));
				if(text.Length > 0)
				{
					string[] list = text.Split(new char[]{'\n'});
					foreach(string line in list)
					{
						if(line.Length > 0)
						{
							DkABItem dkABItem = new DkABItem();
							if(dkABItem.Parse(line))
							{
								addDict(dkABItem);
							}
						}
					}
				}
				DkLog.Show("[Dk] ABConfig item : "+guidDict.Count);
			}
			onInitComplete();
		}

		private void addDict(DkABItem item)
		{
			if(guidDict.ContainsKey(item.GetABName) == false)
			{
				guidDict.Add(item.GetABName,item);
			}
		}

		public DkABItem GetABItem(string abName)
		{
			return guidDict[abName];
		}

		public List<DkABItem> GetLoadDependListBy(string mainGuid)
		{
			List<DkABItem> loadlist = new List<DkABItem>();
			if(guidDict.ContainsKey(mainGuid))
			{
				DkABItem item = GetABItem(mainGuid);
				loadlist.Add(item);
				List<string> dependsList = item.GetDependList();
				foreach(string guid in dependsList)
				{
					DkABItem dependItem = GetABItem(guid);
					if(dependItem != null)
					{
						loadlist.Add(dependItem);
					}
				}
			}
			loadlist.Sort();
			return loadlist;
		}
	}
}

