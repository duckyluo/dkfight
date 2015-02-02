using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;
using Dk.Event;

public class RoleDoAction : RoleBaseActionNode
{
	protected TimeLineMessage m_nextMsg = null;
		
	protected override void Enter (DkBtInputParam input)
	{
		if(GetRoleBBData.DataInfo.team == eSceneTeamType.Me)
		{
			CheckSelfRule();
		}
		
		GetRunTimeData.ActionType = eActionType.None;
		GetRunTimeData.MoveMethod = eMoveMethod.None;
		GetRunTimeData.MoveDirection = eMoveDirection.None;
		GetRunTimeData.PostureType = ePostureType.Pose_None;
		
		GetRunTimeData.MoveEnable = false;
		GetRunTimeData.ActiveChStateEnalbe = false;
		GetRunTimeData.PassiveChStateEnalbe = false;
		GetRunTimeData.UseGravity = eUseGravity.No;
		GetRunTimeData.IsTrigger = false;
		
		GetRunTimeData.ForceSpeed = Vector3.zero;
		GetRunTimeData.CurAlpha = 1f;
		GetRunTimeData.CurScale = 1f;
		GetRunTimeData.CurRotation = Vector3.zero;

		m_nextMsg = null;
		
		base.Enter(input);
	}
	
	protected override void Exectue (DkBtInputParam input)
	{
		UpdateCurStatus();
	}

	protected override void Exit (DkBtInputParam input)
	{
		m_nextMsg = null;
		base.Exit (input);
	}
	
	protected void UpdateCurStatus()
	{
		UpdateNextMsg();
		UpdateHitMsg();
		UpdateMoveMsg();
		UpdateCurMsg();
	}
	
	virtual protected void UpdateNextMsg()
	{
		while(GetFrontWaitMsg != null)
		{
			TimeLineMessage waitMsg = GetFrontWaitMsg;
			if(waitMsg.GetCmdType == eCommandType.Cmd_Hit)
			{
				if(waitMsg.GetActionType == eActionType.Not_Use)
				{
					GetMsgCtrl.AddRunTLMsg(waitMsg);
					GetMsgCtrl.RemoveWaitMsg(waitMsg);
					continue;
				}
				else
				{
					m_nextMsg = waitMsg;
					break;
				}
			}
			else
			{
				m_nextMsg = waitMsg;
				break;
			}
		}
	}
	
	virtual protected void UpdateHitMsg()
	{
		if (GetMsgCtrl.HitList.Count > 0) 
		{
			GetMsgCtrl.HitList.Clear();// to do
		}
	}
	
	virtual protected void UpdateMoveMsg()
	{
		if(GetMsgCtrl.MoveList.Count > 0)
		{
			GetMsgCtrl.MoveList.Clear();// to do
		}
	}
	
	virtual protected void UpdateCurMsg()
	{
		if(m_nextMsg != null)
		{
			NextAction();
		}
		else if(GetRunTimeData.ActionType == eActionType.None)
		{
			StartAction();
		}
		else
		{
			DoAction();
		}
	}

	virtual protected void StartAction()
	{

	}

	virtual protected void DoAction()
	{

	}

	virtual protected void NextAction()
	{
		Exit(null);
	}

	virtual	protected void CheckSelfRule()
	{
		InputManager.KeyJumpEnalbe = false;
	}	
}
