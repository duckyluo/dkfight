using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFsmReceiver
{
	void OnFsmReceive(IFsmMsg msg);
}

//public interface IFsmSender
//{
//	void SendFsmMsg(IFsmMsg msg);
//}
//
//
//public class FsmObj : IFsmReceiver , IFsmSender
//{
//	public virtual void OnFsmReceive(IFsmMsg msg)
//	{
//	}
//	
//	public virtual void SendFsmMsg(IFsmMsg msg)
//	{
//		FsmMsgManager.SendFsmMsg(msg);
//	}
//}

public class FsmMsgManager
{
	protected static Dictionary<eFsmMsgType,List<IFsmReceiver>> dict = new Dictionary<eFsmMsgType, List<IFsmReceiver>>();
	
	public static void RegistReceiver(eFsmMsgType type , IFsmReceiver receiver)
	{
		List<IFsmReceiver> list = GetReceiver(type);
		if(list.Contains(receiver) == false)
		{
			list.Add(receiver);
		}
	}

	public static void RemoveRegist(eFsmMsgType type , IFsmReceiver receiver)
	{
		List<IFsmReceiver> list = GetReceiver(type);
		list.Remove(receiver);
	}
	
	public static void SendFsmMsg(IFsmMsg msg)
	{
		if(msg is RoleFsmMessage)
		{
			//Debug.Log("["+(msg as RoleFsmMessage).receiveIndex+"][Fsm][Send] "+(msg as RoleFsmMessage).cmdType);
		}

		List<IFsmReceiver> list = GetReceiver(msg.GetMsgType());
		foreach(IFsmReceiver receiver in list)
		{
			receiver.OnFsmReceive(msg);
		}
	}

	public static List<IFsmReceiver> GetReceiver(eFsmMsgType type)
	{
		if(dict.ContainsKey(type) == false)
		{
			List<IFsmReceiver> list = new List<IFsmReceiver>();
			dict.Add(type,list);
		}
		return dict[type];
	}
}
