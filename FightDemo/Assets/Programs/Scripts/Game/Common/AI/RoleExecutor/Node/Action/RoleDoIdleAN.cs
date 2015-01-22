using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

/// <summary>
/// Role do idle.
/// </summary>
public class RoleDoIdleAN : RoleBaseActionNode
{
	protected TimeLineMessage m_nextMsg = null;

	public override void Initalize ()
	{
		this.m_name = "DoIdle";
		base.Initalize ();
	}

	public override bool Evaluate (DkBtInputParam input)
	{
		return true;
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		CheckSelfRule();

		GetRunTimeData.ActionType = eActionType.None;
		GetRunTimeData.MoveMethod = eMoveMethod.None;
		GetRunTimeData.MoveDirection = eMoveDirection.None;
		GetRunTimeData.PostureType = ePostureType.Pose_None;

		GetRunTimeData.MoveEnable = true;
		GetRunTimeData.ActiveChStateEnalbe = true;
		GetRunTimeData.PassiveChStateEnalbe = true;
		GetRunTimeData.UseGravity = eUseGravity.Yes;
		GetRunTimeData.IsTrigger = false;

		GetRunTimeData.ForceSpeed = Vector3.zero;
		GetRunTimeData.CurAlpha = 1f;
		GetRunTimeData.CurScale = 1f;

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
		UpdateCurMsg();
	}

	protected void CheckNextMsg()
	{
		while (GetFrontWaitMsg != null) 
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
		if (this.GetMsgCtrl.HitList.Count > 0) 
		{
			// to do
			this.GetMsgCtrl.HitList.Clear();
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
			StartIdle();
		}
		else
		{
			DoIdle();
		}

	}

	protected void StartIdle()
	{
		GetRunTimeData.ActionType = eActionType.Idle;
		GetRunTimeData.MoveMethod = eMoveMethod.Gravity;
		GetRunTimeData.PostureType = ePostureType.Pose_Stand;

		UpdateAnimation();
	}

	protected void DoIdle()
	{

	}
	
	protected void UpdateAnimation()
	{
		if(GetAniCtrl != null)
		{
			GetAniCtrl.Play(AnimationNameDef.Idle,WrapMode.Loop,false,1f);
		}
	}

	protected override void Exit (DkBtInputParam input)
	{
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