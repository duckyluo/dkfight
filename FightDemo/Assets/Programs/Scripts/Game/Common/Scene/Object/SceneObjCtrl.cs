using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneObjCtrl : IFsmReceiver
{
	private Dictionary<int,SceneObj> objDict = new Dictionary<int, SceneObj>();

	public SceneObjCtrl()
	{
		FsmMsgManager.RegistReceiver(eFsmMsgType.Role,this);
	}
	
	public void OnFsmReceive (IFsmMsg msg)
	{
		if(msg.GetMsgType() == eFsmMsgType.Role)
		{
			int index = (msg as RoleFsmMessage).receiveIndex;

			//Debug.Log("role ctrl receive msg , target is "+index);
			if(index >= 0)
			{
				SceneObj obj = objDict[index];
				obj.OnFsmReceive(msg);
			}

		}
	}

	public SceneObj AddNewItem(SceneObjInfo info)
	{
		if(objDict.ContainsKey(info.index) == false)
		{
			SceneObj item = null;
			switch(info.type)
			{
			case eSceneObjType.Role:
				item = new SceneRoleObj(info);
				break;
			}
			if(item != null)
			{
				objDict.Add(info.index,item);
			}
			return item;
		}
		else return null;
	}
	
	public void ClearAllObj()
	{
		foreach(KeyValuePair<int,SceneObj> pair in objDict)
		{
			SceneObj item = pair.Value;
			item.Destroy();
		}
		objDict.Clear();
	}
	
	public void Update()
	{
		foreach(KeyValuePair<int,SceneObj> pair in objDict)
		{
			SceneObj obj = pair.Value;
			obj.Update();
		}
	}
}
