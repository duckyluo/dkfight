using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Dk.Loader;
using Dk.Event;
using Dk.Asset;
using Dk.Util;

public class Test : MonoBehaviour 
{	
	private List<string> logList = new List<string>();

	private bool showDebug = false;

	private bool isBusy = true;
	
	public void HandleLog(string logString, string stackTrace, LogType type) 
	{
		logList.Add(logString+"\n");
	}
	
	public void Awake()
	{
		DkGlobal.CacheClean();
		DkVersion.version = "1.0.0";
	}
		
	void Start () 
	{
		this.isBusy = true;

		//DkGlobal.SetDebugEnable(false);
		DkLog.Instance.Initalize();

		Debug.Log("PersistentDataPath : "+Application.persistentDataPath);
		Debug.Log("DataPath : "+Application.dataPath);

		loadPackageData();
	}

	void Update () 
	{

	}
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(10,10,90,40),"loadAsset"))
		{
			loadAsset();
		}

		if(GUI.Button(new Rect(110,10,90,40),"showDebug"))
		{
			showDebug = !showDebug;
		}

		if(showDebug)
		{
			string content = GetLogContent();
			GUI.TextArea(new Rect(10,60,800,400),content);
		}
	}

	private void loadPackageData()
	{
		Action<DkEvent> action = onPackageInit;
		DkPackageManager.Instance.AddEventListen(DkEventDef.DATA_INIT,action);
		DkPackageManager.Instance.Initialize();
	}

	private void onPackageInit(DkEvent evt)
	{
		loadAllConfigList();
	}

	private void loadAllConfigList()
	{
		Action<DkEvent> action = onAllConfigInit;
		DkAssetListConfig.Instance.AddEventListen(DkEventDef.DATA_INIT,action);
		DkAssetListConfig.Instance.Initialize();

		DkABListConfig.Instance.AddEventListen(DkEventDef.DATA_INIT,action);
		DkABListConfig.Instance.Initialize();
	}

	private void onAllConfigInit(DkEvent evt)
	{
		if(DkAssetListConfig.Instance.GetInited && DkABListConfig.Instance.GetInited)
		{
			this.isBusy = false;
		}
	}

	private void loadAsset()
	{
		if(!isBusy)
		{
			//List<string> list = new List<string>(){"SourceData/Scenes/test2.unity"};
			List<string> list = new List<string>(){"SourceData/Prefabs/Npc/PC/MyPlayer.prefab","SourceData/Prefabs/Env/Zone1/Zone1.prefab"};
			DkAssetLoadCollection collection = new DkAssetLoadCollection(list);
			Action<DkEvent> action = loadComplete;
			collection.AddEventListen(DkEventDef.LOAD_COMPLETE,action);
			collection.StartLoad();

			//Application.LoadLevelAsync("SourceData/Scenes/test2.unity");
		}
	}
	
	public void loadComplete(DkEvent evt)
	{
		DkAssetLoadCollection collection = evt.target as DkAssetLoadCollection;
		collection.Destroy();

//		Application.LoadLevelAsync("test2");

		UnityEngine.Object obj1 = DkResourceManager.Instance.GetAssetObject("SourceData/Prefabs/Env/Zone1/Zone1.prefab");

		if(obj1 != null)
		{
			GameObject.Instantiate(obj1);
		}

		UnityEngine.Object obj2 = DkResourceManager.Instance.GetAssetObject("SourceData/Prefabs/Npc/PC/MyPlayer.prefab");
		if(obj2 != null)
		{
			GameObject.Instantiate(obj2);
		}

		//Debug.Log(" === "+DkResourceManager.Instance.GetAssetItem("SourceData/Prefabs/Npc/PC/MyPlayer.prefab").assetType);
	}

	public string GetLogContent()
	{
		string content = "";

		List<string> logList = DkLog.GetLogList();
		for(int i = 0 ;  i < logList.Count ; i++)
		{
			if(!string.IsNullOrEmpty(logList[i]))
			{
				content += logList[i];
			}
		}
		return content;
	}

	//
	private void example()
	{

		//======================= Start ====================================//

		//DkGlobal.CacheClean();
		//DkVersion.version = "1.0.0";

//		//step 1:
//		DkPackageManager.Instance.AddEventListen(DkEventDef.DATA_INIT,action);
//		DkPackageManager.Instance.Initialize();
//
//		//step 2:
//		DkABListConfig.Instance.AddEventListen(DkEventDef.DATA_INIT,action);
//		DkABListConfig.Instance.Initialize();
//
//		DkAssetListConfig.Instance.AddEventListen(DkEventDef.DATA_INIT,action);
//		DkAssetListConfig.Instance.Initialize();

		//====================== Start Finish================================//

//		List<string> urlList = new List<string>();
//		DkMissionCollection loadCollection = new DkMissionCollection(urlList,eDkLoadProtocal.WEB);
//		loadCollection.AddEventListen(DkEventDef.LOAD_COMPLETE,Action);
//		loadCollection.StartLoad();
//		DkResourceManager.Instance.GetLoadItem("url"); //bytes[]
//
//		List<string> assetList = new List<string>();
//		DkAssetLoadCollection assetCollection = new DkAssetLoadCollection(assetList);
//		assetCollection.AddEventListen(DkEventDef.LOAD_COMPLETE,Action);
//		assetCollection.StartLoad();
//		DkResourceManager.Instance.GetAssetObject("path");
//
		//======================== Util =========================================//
		//	DkLog.Show("");
		//	DkFileUtil
		//	DkZipUtil
		//	DkCameraUtil
	}


}
