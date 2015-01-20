using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Dk.Util;

namespace Dk.Loader
{
	public enum eDkDownLoadstatus
	{
		NO = 0,
		LOADING = 1,
		COMPLETE = 2,
		EMPTY = 4,
	}

	public class DkLoaderAssist : MonoBehaviour
	{
		public eDkDownLoadstatus status = eDkDownLoadstatus.NO;

		private List<DkLoaderMission> loadList = new List<DkLoaderMission>();
		private DkLoaderMission curLoadMission = null;
		
		public void Awake()
		{
			if(status == eDkDownLoadstatus.NO)
			{
				status = eDkDownLoadstatus.EMPTY;
			}

			DontDestroyOnLoad(this);
		}
		
		public void AddLoadMission(DkLoaderMission item)
		{
			if(item.protocal == eDkLoadProtocal.NULL)
			{
				item.protocal = DkLoaderMission.defaultProtocal;
			}

			loadList.Add(item);
			if(status == eDkDownLoadstatus.EMPTY || status == eDkDownLoadstatus.NO)
			{
				status = eDkDownLoadstatus.LOADING;
			}
		}

		public void Update()
		{
			if(status == eDkDownLoadstatus.LOADING)
			{
				doLoading();
			}
		}

		private void doLoading()
		{
			if(loadList != null && loadList.Count > 0)
			{
				if(curLoadMission == null)
				{
					curLoadMission = loadList[0];
					if(curLoadMission.status == eDkLoadMissionStatus.NOTYET)
					{
						curLoadMission.status = eDkLoadMissionStatus.START;
						if(curLoadMission.protocal == eDkLoadProtocal.FILE)
						{
							loadByFile();
						}
						else
						{
							loadByWWW();
						}
					}
					else
					{
						Debug.Log("there is something wrong");
					}
				}
				else if(curLoadMission.status == eDkLoadMissionStatus.START)
				{
					curLoadMission.status = eDkLoadMissionStatus.LOADING;
				}
				else if(curLoadMission.status == eDkLoadMissionStatus.LOADCOMPLETE)
				{
					removeCurItem();
				}
				else if(curLoadMission.status == eDkLoadMissionStatus.ERROR)
				{
					removeCurItem();
				}
			}
			else
			{
				status = eDkDownLoadstatus.EMPTY;
			}
		}

		protected void loadByFile()
		{
			string url = curLoadMission.path;
			int version = curLoadMission.version;

			try
			{
				switch(curLoadMission.type)
				{
				case eDkWWWItemType.ASEET_BUNDLE:
					curLoadMission.obj = AssetBundle.CreateFromFile(url);
					break;
				case eDkWWWItemType.BYTES:
					curLoadMission.obj = DkFileUtil.ReadFileToBytes(url,false);
					break;
				}
			}
			catch(Exception e)
			{
				Debug.Log("[Error] :"+e.Message+"\n"+e.StackTrace);
			}
			finally
			{
				if(curLoadMission.obj != null)
				{
					curLoadMission.status = eDkLoadMissionStatus.LOADCOMPLETE;
				}
				else
				{
					Debug.Log("[Error][file load] failed " + " , url: " +url);
				}
			}
		}

		protected void loadByWWW()
		{
			StartCoroutine(startLoadCurItem());
		}

		IEnumerator startLoadCurItem()
		{
			string url = curLoadMission.path;
			int version = curLoadMission.version;

			if(!url.Contains("http://") && !url.Contains("https://") && !url.Contains("ftp://"))
			{
				if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
				{
					if(url.Contains(Application.persistentDataPath) || url.Contains(Application.dataPath))
					{
						url = "file:///"+url;
					}
				}
				else if(Application.platform == RuntimePlatform.Android)
				{
					if(url.Contains(Application.persistentDataPath))
					{
						url = "file://"+url;
					}
				}
				else if(Application.platform == RuntimePlatform.IPhonePlayer)
				{
					if(url.Contains(Application.persistentDataPath) || url.Contains(Application.dataPath))
					{
						url = "file://"+url;
					}
				}

			}

			WWW www = new WWW(url);

			yield return www;
			
			if(www.error == null)
			{
				switch(curLoadMission.type)
				{
				case eDkWWWItemType.ASEET_BUNDLE:
					curLoadMission.obj = www.assetBundle;
					break;
				case eDkWWWItemType.BYTES:
					curLoadMission.obj = www.bytes;
					break;
				}
				curLoadMission.status = eDkLoadMissionStatus.LOADCOMPLETE;
			}
			else
			{
				Debug.Log("[Error][www load] "+www.error + " , url: " +url);
				curLoadMission.status = eDkLoadMissionStatus.ERROR;
			}

			www.Dispose();
		}
		
		private void removeCurItem()
		{
			curLoadMission.Destroy();
			if(loadList != null)
			{
				loadList.Remove(curLoadMission);
			}
			curLoadMission = null;
		}
	}

}
