using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dk.Util;
using Dk.Loader;
using Dk.Event;

namespace Dk.Asset
{
	public class DkAssetListConfig : DkEventDispatch
	{
		public static string configName = "AllAssetList.bytes";

		private bool inited = false;

		private bool loaded = false;
		
		private Dictionary<string,DkAssetItem> assetDict = new Dictionary<string, DkAssetItem>();

		private static DkAssetListConfig s_instance = null;
		
		public static DkAssetListConfig Instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = new DkAssetListConfig();
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
				StartLoad();
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
					SetConfigContent(text);
				}
				DkLog.Show("[Dk] AssetsConfig item : "+assetDict.Count);
			}

			onInitComplete();
		}

		public void SetConfigContent(string text)
		{
			assetDict.Clear();
			
			string[] list = text.Split(new char[]{'\n'});
			foreach(string line in list)
			{
				if(line.Length > 0)
				{
					DkAssetItem dkAssetItem = new DkAssetItem();
					if(dkAssetItem.Parse(line))
					{
						addIdDict(dkAssetItem);
					}
				}
			}
		}
		
		private void addIdDict(DkAssetItem item)
		{
			if(assetDict.ContainsKey(item.path) == false)
			{
				assetDict.Add(item.path,item);
			}
		}

		public DkAssetItem GetDkAssetItem(string itemPath)
		{
			if(assetDict.ContainsKey(itemPath))
			{
				return assetDict[itemPath];
			}
			else return null;
		}

		public Dictionary<string,DkAssetItem> GetAssetDict()
		{
			return this.assetDict;
		}
	}
}
