using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

/// <summary>
/// Role do stop.
/// </summary>
public class RoleDoMoveStopAN : RoleBaseActionNode
{
	protected TMoveMessage m_curMsg = null;
	
	protected TimeLineMessage m_nextMsg = null;

	public override void Initalize ()
	{
		this.m_name = "DoStop";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg.GetActionType == eActionType.Stop)
		{
			return true;
		}
		else return false;
	}

	protected override void Enter (DkBtInputParam input)
	{
		CheckSelfRule();

		GetRunTimeData.ActionType = eActionType.None;
		GetRunTimeData.MoveMethod = eMoveMethod.None;
		GetRunTimeData.MoveDirection = eMoveDirection.None;
		//GetRunTimeData.LookDirection = eLookDirection.None;
		GetRunTimeData.PostureType = ePostureType.Pose_None;
		
		GetRunTimeData.MoveEnable = true;
		GetRunTimeData.ActiveChStateEnalbe = true;
		GetRunTimeData.PassiveChStateEnalbe = true;
		GetRunTimeData.UseGravity = eUseGravity.Yes;
		GetRunTimeData.IsTrigger = false;

		GetRunTimeData.ForceSpeed = Vector3.zero;
		GetRunTimeData.CurAlpha = 1f;
		GetRunTimeData.CurScale = 1f;
		GetRunTimeData.CurRotation = Vector3.zero;
		
		m_curMsg = GetFrontWaitMsg as TMoveMessage;
		GetMsgCtrl.AddRunTLMsg(GetFrontWaitMsg);
		GetMsgCtrl.RemoveWaitMsg(GetFrontWaitMsg);
		
		m_nextMsg = null;
		
		base.Enter(input);
	}
	
	protected override void Exectue (DkBtInputParam input)
	{
		UpdateCurStatus();
	}
	
	protected void UpdateCurStatus()
	{
		CheckNextMsg();
		UpdateHitMsg();
		UpdateMoveMsg();
		UpdateCurMsg();
	}

	protected void CheckNextMsg()
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

	protected void UpdateHitMsg()
	{
		if (GetMsgCtrl.HitList.Count > 0) 
		{
			GetMsgCtrl.HitList.Clear();// to do
		}
	}
	
	protected void UpdateMoveMsg()
	{
		if(GetMsgCtrl.MoveList.Count > 0)
		{
			GetMsgCtrl.MoveList.Clear();// to do
		}
	}
	
	protected void UpdateCurMsg()
	{
		if(m_nextMsg != null)
		{
			Exit(null);
		}
		else if(GetRunTimeData.ActionType == eActionType.None)
		{
			StartMove();
		}
		else
		{
			DoMove();
		}
	}

	protected void StartMove()
	{
		GetRunTimeData.ActionType = eActionType.Stop;
		GetRunTimeData.MoveMethod = eMoveMethod.Gravity;

		float dis = Vector3.Distance(m_curMsg.GetCurPos,GetRunTimeData.CurPos);
		if(dis == 0)
		{
			GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
			Exit(null);
		}
	}

	protected void DoMove()
	{
		//to do
		Debug.Log("[error]some thing error, des "+ m_curMsg.GetCurPos + " , cur "+GetRunTimeData.CurPos);

		Vector3 motion = m_curMsg.GetCurPos - GetRunTimeData.CurPos;
		GetTransformCtrl.MoveLimit(motion);
		//GetTransformCtrl.MoveTo(m_curMsg.GetCurPos);
		Exit(null);
	}

	protected override void Exit (DkBtInputParam input)
	{
		m_curMsg = null;
		m_nextMsg = null;
		base.Exit (input);
	}

	protected void CheckSelfRule()
	{
		if(GetRoleBBData.DataInfo.team == eSceneTeamType.Me)
		{
			InputManager.KeyJumpEnalbe = true;
		}
	}
}
