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
	private const String Ctrl_Transform = "CtrlTransform";
	private const string Ctrl_Animation = "CtrlAnimation";
	private const string Ctrl_Skill = "CtrlSkill";
	private const string Ctrl_Msg = "CtrlMsg";
	private const string Data_Info = "DataInfo";
	private const string Data_Local = "DataLocal";
	private const string Data_RunTime = "DataRunTime";
	private const string BT_Decider = "BtDecider";
	private const string BT_Excutor = "BtExcutor";
	
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

	public RoleCtrlMessage CtrlMsg
	{
		set{this.AddData(Ctrl_Msg,value);}
		get{return GetData<RoleCtrlMessage>(Ctrl_Msg);}
	}

	public RoleCtrlTransform CtrlTransform
	{
		set{this.AddData(Ctrl_Transform,value);}
		get{return GetData<RoleCtrlTransform>(Ctrl_Transform);}
	}
	
	public RoleCtrlAnimation CtrlAnimation
	{
		set{this.AddData(Ctrl_Animation,value);}
		get{return GetData<RoleCtrlAnimation>(Ctrl_Animation);}
	}

	public RoleCtrlSkill CtrlSkill
	{
		set{this.AddData(Ctrl_Skill,value);}
		get{return GetData<RoleCtrlSkill>(Ctrl_Skill);}
	}

	public SceneObjInfo DataInfo
	{
		set{this.AddData(Data_Info,value);}
		get{return GetData<SceneObjInfo>(Data_Info);}
	}

	public RoleDataLocal DataLocal
	{
		set{this.AddData(Data_Local,value);}
		get{return GetData<RoleDataLocal>(Data_Local);}
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
