using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Event;
using Dk.Asset;
using Dk.Util;

namespace Dk.Loader
{
	public enum eDkLoadMissionStatus
	{
		NOTYET = 0,
		START,
		LOADING,
		LOADCOMPLETE,
		ERROR,
	}

	public enum eDkWWWItemType
	{
		ASEET_BUNDLE,
//		TEXT,
		BYTES,
	}

	public enum eDkLoadProtocal
	{
		FILE = 0,
		WEB,
		NULL,
	}

	public class DkLoaderMission:DkEventDispatch
	{
		public static eDkLoadProtocal defaultProtocal = eDkLoadProtocal.FILE;

		public string path;
		
		public int version;

		public string md5;

		public int len;

		private eDkLoadMissionStatus _status = eDkLoadMissionStatus.NOTYET;

		public eDkWWWItemType type = eDkWWWItemType.ASEET_BUNDLE;
			
		public System.Object obj = null;

		public eDkLoadProtocal protocal = eDkLoadProtocal.NULL;

		public eDkLoadMissionStatus status
		{
			set
			{
				_status = value;
				if(_status == eDkLoadMissionStatus.LOADCOMPLETE)
				{
					loadComplete();
				}
			}

			get
			{
				return _status;
			}
		}

		public bool AppendToLoading()
		{
			return DkLoadManager.Instance.AddLoadMission(this);
		}
	
		protected void loadComplete()
		{
			DkLog.Show("[Load][LoadComplete] :"+this.path +" by "+this.protocal);

			DkResourceLoadItem item = new DkResourceLoadItem();
			item.type = this.type;
			item.obj = this.obj;
			item.loadPath = this.path;

			DkResourceManager.Instance.AddLoadItem(item);

			DkLoadManager.Instance.RemoveMission(this.path);

			this.DispatchEvent(new DkEvent(DkEventDef.LOAD_COMPLETE));
		}
		
		override public void Destroy()
		{
			base.Destroy();
			path = null;
			obj = null;
		}

		override public string ToString()
		{
			string str = " path "+path+" version "+version+ " md5 "+md5+ " len "+ len;
			return str;
		}
		
	}
}