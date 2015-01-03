using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneFsmReceiver
{
	void OnFsmReceive(ISceneFsmMsg msg);
}

public interface ISceneFsmSender
{
	void SendFsmMsg(ISceneFsmMsg msg);
}


public class SceneFsmObj : ISceneFsmReceiver , ISceneFsmSender
{
	public virtual void OnFsmReceive(ISceneFsmMsg msg)
	{
	}
	
	public virtual void SendFsmMsg(ISceneFsmMsg msg)
	{
		SceneFsmMsgManager.SendFsmMsg(msg);
	}
}

public class SceneFsmMsgManager
{
	protected static Dictionary<eFsmMsgType,List<ISceneFsmReceiver>> dict = new Dictionary<eFsmMsgType, List<ISceneFsmReceiver>>();
	
	public static void RegistReceiver(eFsmMsgType type , ISceneFsmReceiver receiver)
	{
		List<ISceneFsmReceiver> list = GetReceiver(type);
		if(list.Contains(receiver) == false)
		{
			list.Add(receiver);
		}
	}

	public static void RemoveRegist(eFsmMsgType type , ISceneFsmReceiver receiver)
	{
		List<ISceneFsmReceiver> list = GetReceiver(type);
		list.Remove(receiver);
	}
	
	public static void SendFsmMsg(ISceneFsmMsg msg)
	{
		if(msg is RoleFsmMessage)
		{
			Debug.Log("["+(msg as RoleFsmMessage).targetId+"][Fsm][Send] "+(msg as RoleFsmMessage).cmdType);
		}

		List<ISceneFsmReceiver> list = GetReceiver(msg.GetMsgType());
		foreach(ISceneFsmReceiver receiver in list)
		{
			receiver.OnFsmReceive(msg);
		}
	}

	public static List<ISceneFsmReceiver> GetReceiver(eFsmMsgType type)
	{
		if(dict.ContainsKey(type) == false)
		{
			List<ISceneFsmReceiver> list = new List<ISceneFsmReceiver>();
			dict.Add(type,list);
		}
		return dict[type];
	}
}



