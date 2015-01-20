using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dk.Util;
using Dk.Loader;
using Dk.Event;
using System.IO;

namespace Dk.Asset
{
	public class DkPackageManager : DkEventDispatch
	{
		private static string fileName = "data.bytes";
		
		private bool inited = false;

		private bool loaded = false;

		private static DkPackageManager s_instance = null;
		
		public static DkPackageManager Instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = new DkPackageManager();
				}
				return s_instance;
			}
		}

		public static string GetLocalPackageLoadPath()
		{
			return DkLoadPathUtil.GetLocalPackagePath(fileName);
		}
		
		public bool Initialize()
		{
			if(inited == false)
			{
				if(loaded == false)
				{
					loaded = true;

					bool haveCache = DkGlobal.Initalize(); 
					if(haveCache)
					{
						if(DkGlobal.GetSourceDataEnable())
						{
							onInitComplete();
						}
						else
						{
							//to do : check version
							string oldVersion = DkGlobal.GetVersion().Trim();
							string newVersion = DkVersion.version.Trim();
							if(oldVersion == newVersion)
							{
								onInitComplete();
							}
							else
							{
								StartLoad();
							}
						}
					}
					else
					{
						StartLoad();
					}
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
			DkLoaderMission mission = new DkLoaderMission();
			mission.path = GetLocalPackageLoadPath();
			mission.type = eDkWWWItemType.BYTES;
			mission.protocal = eDkLoadProtocal.WEB;

			Action<DkEvent> action = onLoadComplete;
			mission.AddEventListen(DkEventDef.LOAD_COMPLETE,action);
			mission.AppendToLoading();
		}
		
		protected void onLoadComplete(DkEvent evt)
		{
			DkLoaderMission mission = evt.target as DkLoaderMission;
			mission.Destroy();

			DkResourceLoadItem item = DkResourceManager.Instance.GetLoadItem(GetLocalPackageLoadPath());
			if(item != null)
			{
				byte[] bytes = item.obj as byte[];
				unPackData(bytes,DkVersion.version);
			}

			onInitComplete();
		}
		
		public static void unPackData(byte[] data , string version)
		{
			List<byte[]> byteList;
			List<string> pathList;
			DkZipUtil.unZip(data,out byteList, out pathList);
			
			DkLog.Show("[Dk] UnPack : Item count "+byteList.Count);
			int count = byteList.Count;
			
			for(int i = 0 ; i < count ; i++)
			{
				string path = pathList[i];
				byte[] bytes = byteList[i];
				string filePath = Application.persistentDataPath+"/"+path;

				string dir = DkFileUtil.GetDirPathByFull(filePath);
				if(Directory.Exists(dir) == false)
				{
					Directory.CreateDirectory(dir);
				}

				DkFileUtil.WriteFile(filePath,bytes,false);
			}

			DkGlobal.SetVersion(version);
			DkGlobal.SaveData();
		}
	}
}

