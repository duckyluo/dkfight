using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleBaseParalleNode : DkBtParalleNode
{
	protected bool m_isDebug = true;

	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetRoleBBData == null)
		{
			Debug.Log("[error][Evaluate] RoleBBData is null  ");
			return false;
		}
		return base.Evaluate (input);
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		if(m_isDebug)
		{
			Debug.Log("["+GetRoleBBData.DataInfo.index+"]["+this.m_name+"] Enter");
		}
		base.Enter(input);
	}
	
	protected override void Exit (DkBtInputParam input)
	{
		if(m_isDebug)
		{
			Debug.Log("["+GetRoleBBData.DataInfo.index+"]["+this.m_name+"] Exit");
		}
		base.Exit (input);
	}
	
	public override void Finish ()
	{
		if(m_isDebug)
		{
			Debug.Log("["+GetRoleBBData.DataInfo.index+"]["+this.m_name+"] Finish");
		}
		base.Finish ();
	}
	
	protected RoleBlackBoard GetRoleBBData
	{
		get{return this.BT.BBData as RoleBlackBoard;}
	}
	
	protected RoleDataRunTime GetRunTimeData
	{
		get{return GetRoleBBData.DataRunTime;}
	}

	protected RoleDataLocal GetLocalData
	{
		get{return GetRoleBBData.DataLocal;}
	}
	
	protected RoleCtrlMessage GetMsgCtrl
	{
		get{return GetRoleBBData.CtrlMsg;}
	}
	
	protected RoleCtrlTransform GetTransformCtrl
	{
		get{return GetRoleBBData.CtrlTransform;}
	}
	
	protected RoleCtrlAnimation GetAniCtrl
	{
		get{return GetRoleBBData.CtrlAnimation;}
	}

	protected TimeLineMessage GetFrontWaitMsg
	{
		get
		{
			if(GetRoleBBData != null && GetMsgCtrl != null )
			{
				return GetMsgCtrl.GetWaitTLMsgFront();
			}
			else return null;
		}
	}
	
}
