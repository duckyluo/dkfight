using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Event;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleBlackBoard : DkBlackboard
{
	private const string Scene_Obj = "SceneObj";
	private const string Prefab_Main = "PrefabMain";
	private const string Prefab_Model = "PrefaModel";
	private const String Ctrl_Move = "CtrlMove";
	private const string Ctrl_Animation = "CtrlAnimation";
	private const string Ctrl_Msg = "CtrlMsg";
	private const string Data_Info = "DataInfo";
	private const string Data_Base = "DataBase";
	private const string Data_RunTime = "DataRunTime";
	private const string BT_Decider = "BtDecider";
	private const string BT_Excutor = "BtExcutor";
	//private const string Comp_Animation = "CompAnimation";

//	public SceneObj SceneObject
//	{
//		set{this.AddData(Scene_Obj,value);}
//		get{return GetData<SceneObj>(Scene_Obj);}
//	}

	public GameObject PrefabMain
	{
		set{this.AddData(Prefab_Main,value);}
		get{return GetData<GameObject>(Prefab_Main);}
	}

	public GameObject PrefabModel
	{
		set{this.AddData(Prefab_Model,value);}
		get{return GetData<GameObject>(Prefab_Model);}
	}

	public RoleCtrlMove CtrlMove
	{
		set{this.AddData(Ctrl_Move,value);}
		get{return GetData<RoleCtrlMove>(Ctrl_Move);}
	}

	public RoleCtrlMsg CtrlMsg
	{
		set{this.AddData(Ctrl_Msg,value);}
		get{return GetData<RoleCtrlMsg>(Ctrl_Msg);}
	}
	
	public RoleCtrlAnimation CtrlAnimation
	{
		set{this.AddData(Ctrl_Animation,value);}
		get{return GetData<RoleCtrlAnimation>(Ctrl_Animation);}
	}
	
	public SceneObjInfo DataInfo
	{
		set{this.AddData(Data_Info,value);}
		get{return GetData<SceneObjInfo>(Data_Info);}
	}

	public RoleDataBase DataBase
	{
		set{this.AddData(Data_Base,value);}
		get{return GetData<RoleDataBase>(Data_Base);}
	}
	
	public RoleDataRunTime DataRunTime
	{
		set{this.AddData(Data_RunTime,value);}
		get{return GetData<RoleDataRunTime>(Data_RunTime);}
	}
	
	public DkBehaviourTree TreeDecider
	{
		set{this.AddData(BT_Decider,value);}
		get{return GetData<DkBehaviourTree>(BT_Decider);}
	}

	public DkBehaviourTree TreeExecutor
	{
		set{this.AddData(BT_Excutor,value);}
		get{return GetData<DkBehaviourTree>(BT_Excutor);}
	}

//	public Animation CompAnimation
//	{
//		set{this.AddData(Comp_Animation,value);}
//		get{return GetData<Animation>(Comp_Animation);}
//	}
}
