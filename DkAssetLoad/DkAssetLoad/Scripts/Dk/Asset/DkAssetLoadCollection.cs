using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Event;
using Dk.Interface;
using Dk.Loader;
using Dk.Util;

namespace Dk.Asset
{
	public class DkAssetLoadCollection : DkEventDispatch , ILoadCollection
	{
		private int totalSize = 0;

		private int curSize = 0;

		private bool finished = false;

		private bool isLoad = false;

		private List<DkAssetLoadRequest> reqList = new List<DkAssetLoadRequest>();

		private Dictionary<string,DkAssetItem> itemMap = new Dictionary<string, DkAssetItem>();

		private List<string> assetPaths = null;

		private DkMissionCollection missionCollection;

		public DkAssetLoadCollection(List<string> assetPaths)
		{
			if(assetPaths != null && assetPaths.Count > 0)
			{
				this.assetPaths = assetPaths;

				foreach(string itemPath in assetPaths)
				{
					if(DkResourceManager.Instance.ContainsAssetItem(itemPath) == false)
					{
						DkAssetItem assetItem = DkAssetListConfig.Instance.GetDkAssetItem(itemPath);
						if(assetItem != null && itemMap.ContainsKey(assetItem.path) == false)
						{
							totalSize++;
							DkAssetItem item = assetItem.Clone();
							itemMap.Add(item.path,item);
						}
					}
				}
			}

			checkLoadFrom();
		}

		public List<string> GetAssetPaths()
		{
			return this.assetPaths;
		}

		private void checkLoadFrom()
		{
			List<DkABItem> loadABList = new List<DkABItem>();
			foreach(KeyValuePair<string,DkAssetItem> pair in itemMap)
			{
				DkAssetItem item = pair.Value;
				if(!DkGlobal.GetSourceDataEnable())
				{
					List<DkABItem> abList = DkABListConfig.Instance.GetLoadDependListBy(item.inABName);

					foreach(DkABItem abItem in abList)
					{
						if(DkResourceManager.Instance.ContainsLoadItem(abItem.GetLoadPath()) == false 
						  && loadABList.Contains(abItem) == false)
						{
							loadABList.Add(abItem);
						}
					}
				}
			}

			if(loadABList.Count > 0)
			{
				loadABList.Sort();
				List<DkLoaderMission> missionList = new List<DkLoaderMission>();
				foreach(DkABItem abItem in loadABList)
				{
					DkLoaderMission mission = new DkLoaderMission();
					mission.path = abItem.GetLoadPath();
					mission.type = eDkWWWItemType.ASEET_BUNDLE;
					missionList.Add(mission);
				}

				missionCollection = new DkMissionCollection(missionList);
			}
		}

		public void StartLoad()
		{
			if(isLoad || finished)
			{
				return;
			}
			isLoad = true;

			if(missionCollection != null)
			{
				DkLog.Show("[DK]Load assetBundle num : "+missionCollection.GetTotalSize());

				Action<DkEvent> action = onABLoadComplete;
				missionCollection.AddEventListen(DkEventDef.LOAD_COMPLETE,action);
				missionCollection.StartLoad();
			}
			else
			{
				StartAssetLoad();
			}
		}

		private void onABLoadComplete(DkEvent evt)
		{
			missionCollection.Destroy();
			StartAssetLoad();
		}
		
		private void StartAssetLoad()
		{
			reqList.Clear();

			foreach(KeyValuePair<string,DkAssetItem> pair in itemMap)
			{
				DkAssetItem item = pair.Value;

				if(DkResourceManager.Instance.GetAssetObject(item.path) == null) 
				{
					DkAssetLoadRequest requst = new DkAssetLoadRequest(item);
					Action<DkEvent> action = onRequestComplete;
					requst.AddEventListen(DkEventDef.LOAD_COMPLETE,action);
					reqList.Add(requst);
				}
				else
				{
					curSize++;
				}
			}

			DkLog.Show("[DK]Load asset num : "+reqList.Count);
			if(reqList.Count > 0)
			{
				foreach(DkAssetLoadRequest request in reqList)
				{
					request.StartLoad();
				}
			}
			else
			{
				checkFinish();
			}
		}

		override public void Destroy()
		{
			if(reqList != null)
			{
				foreach(DkAssetLoadRequest requset in reqList)
				{
					requset.Destroy();
				}
	
				reqList.Clear();
				reqList = null;
			}

			if(itemMap != null)
			{
				foreach(KeyValuePair<string,DkAssetItem> pair in itemMap)
				{
					DkAssetItem item = pair.Value;
					item.Destroy();
				}
				
				itemMap.Clear();
				itemMap = null;
			}

			if(assetPaths != null)
			{
				assetPaths.Clear();
				assetPaths = null;
			}

			base.Destroy();
		}

		private void onRequestComplete(DkEvent e)
		{
			DkAssetLoadRequest request = e.target as DkAssetLoadRequest;
	
			if(reqList != null && reqList.Remove(request))
			{
				curSize++;
			}
	
			request.Destroy();
			checkFinish();
		}

		private void checkFinish()
		{
			if(this.finished == false && reqList != null && reqList.Count == 0)
			{	
				this.finished = true;
				this.DispatchEvent(new DkEvent(DkEventDef.LOAD_COMPLETE));
			}
		}
		
		public int GetTotalSize()
		{
			if(missionCollection != null)
			{
				return missionCollection.GetTotalSize()+this.totalSize;
			}
			else return totalSize;
		}

		public int GetCurSize()
		{
			if(missionCollection != null)
			{
				return missionCollection.GetCurSize()+curSize;
			}
			else return curSize;
		}

		public bool IsFinish()
		{
			return finished;
		}
		
	}
}