using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleForceHitAN : RoleBaseActionNode
{
	protected THitMessage m_curMsg = null;
	
	protected TimeLineMessage m_nextMsg = null;

	public override void Initalize ()
	{
		this.m_name = "ForceHit";
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
		//GetRunTimeData.LookDirection = eLookDirection.None;
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
		
		m_curMsg = GetFrontWaitMsg as THitMessage;
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
		m_timeCount += TimerManager.Instance.GetDeltaTime;

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
					//m_timeCount = 0;

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
			StartHit();
		}
		else
		{
			DoHit();
		}
	}

	protected float m_durationTime = 0f;
	protected float m_timeCount = 0f;
	protected void StartHit()
	{
		GetRunTimeData.ActionType = eActionType.ForceHit;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;

		m_durationTime = GetAniCtrl.GetState(AnimationNameDef.Hit1).length;
		m_timeCount = 0;

		UpdateAnimation();
	}
	
	protected void DoHit()
	{
		if(m_timeCount == 0)
		{
			UpdateAnimation();
		}
		else if(m_timeCount >= m_durationTime)
		{
			Exit(null);
		}
	}

	protected void UpdateAnimation()
	{
		if(GetAniCtrl != null)
		{
			GetAniCtrl.Play(AnimationNameDef.Hit1, WrapMode.ClampForever,true,1);
		}
	}

	protected override void Exit (DkBtInputParam input)
	{
		m_curMsg = null;
		m_nextMsg = null;
		m_durationTime = 0f;
		m_timeCount = 0f;
		base.Exit (input);
	}

	protected void CheckSelfRule()
	{
		if(GetRoleBBData.DataInfo.team == eSceneTeamType.Me)
		{
			InputManager.KeyJumpEnalbe = false;
		}
	}
}
