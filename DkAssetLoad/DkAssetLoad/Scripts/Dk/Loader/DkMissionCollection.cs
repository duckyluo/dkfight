using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Event;
using Dk.Interface;

namespace Dk.Loader
{
	public class DkMissionCollection : DkEventDispatch , ILoadCollection
	{
		private int totalSize = 0;
		
		private int curSize = 0;
		
		private bool finished = false;

		private List<DkLoaderMission> itemList = new List<DkLoaderMission>();

		public DkMissionCollection(List<string> pathlist , eDkLoadProtocal protocal)
		{
			if(pathlist != null && pathlist.Count > 0)
			{
				foreach(string path in pathlist)
				{
					if(!string.IsNullOrEmpty(path) && DkLoadManager.Instance.EnableAddMission(path))
					{
						totalSize++;

						DkLoaderMission item = new DkLoaderMission();
						item.type = eDkWWWItemType.BYTES;
						item.path = path;
						item.protocal = protocal;
						itemList.Add(item);
					}
				}
			}
		}

		public DkMissionCollection(List<DkLoaderMission> list)
		{
			if(list != null && list.Count > 0)
			{
				foreach(DkLoaderMission item in list)
				{
					if(DkLoadManager.Instance.EnableAddMission(item.path))
					{
						totalSize++;
						itemList.Add(item);
					}
				}
			}
		}
		
		public void StartLoad()
		{
			if(itemList != null && itemList.Count > 0)
			{
				List<DkLoaderMission> list = new List<DkLoaderMission>(itemList);
				
				foreach(DkLoaderMission item in list)
				{
					Action<DkEvent> action = onItemLoadComplete;
					item.AddEventListen(DkEventDef.LOAD_COMPLETE,action);
					if(DkLoadManager.Instance.AddLoadMission(item) == false)
					{
						itemList.Remove(item);
						curSize++;
					}
				}
			}
		}
		
		override public void Destroy()
		{
			if(itemList != null && itemList.Count > 0)
			{
				foreach(DkLoaderMission mission in itemList)
				{
					mission.Destroy();
				}
				
				itemList.Clear();
			}

			itemList = null;
			base.Destroy();
		}
		
		public void onItemLoadComplete(DkEvent e)
		{
			DkLoaderMission mission = e.target as DkLoaderMission;

			if(itemList != null && itemList.Remove(mission))
			{
				curSize++;
			}

			checkList();
		}

		public int GetTotalSize()
		{
			return totalSize;
		}
		
		public int GetCurSize()
		{
			return curSize;
		}
		
		public bool IsFinish()
		{
			return finished;
		}
		
		protected void checkList()
		{
			if(itemList != null && itemList.Count == 0)
			{	
				OnFinish();
			}
		}

		protected void OnFinish()
		{
			this.finished = true;
			this.DispatchEvent(new DkEvent(DkEventDef.LOAD_COMPLETE));
		}
	}

}