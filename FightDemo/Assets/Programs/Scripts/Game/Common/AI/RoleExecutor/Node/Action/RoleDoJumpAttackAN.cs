using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;
using Dk.Event;

public class RoleDoJumpAttackAN : RoleBaseActionNode
{
	protected TAttackMessage m_curMsg = null;
	
	protected TimeLineMessage m_nextMsg = null;
	
	public override void Initalize ()
	{
		this.m_name = "DoJumpAttack";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg.GetActionType == eActionType.JumpAttack)
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
		GetRunTimeData.PostureType = ePostureType.Pose_Attack;
		
		GetRunTimeData.MoveEnable = false;
		GetRunTimeData.ActiveChStateEnalbe = false;
		GetRunTimeData.PassiveChStateEnalbe = false;
		GetRunTimeData.UseGravity = eUseGravity.No;
		GetRunTimeData.IsTrigger = true;

		GetRunTimeData.ForceSpeed = Vector3.zero;
		GetRunTimeData.CurAlpha = 1f;
		GetRunTimeData.CurScale = 1f;
		GetRunTimeData.CurRotation = Vector3.zero;
		
		m_curMsg = GetFrontWaitMsg as TAttackMessage;
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
			Debug.Log("============================= JumpAttack Break!!");
			GetSkillCtrl.FinishSkillProcess();
			Exit(null);
		}
		else if(GetRunTimeData.ActionType == eActionType.None)
		{
			StartAttack();
		}
		else
		{
			DoAttack();
		}
	}
	
	private void StartAttack()
	{
		GetRunTimeData.ActionType = eActionType.JumpAttack;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
		GetSkillCtrl.StartSkillProcess(eSkillKey.JumpAttack,m_curMsg.skillIndex);
	}
	
	private void DoAttack()
	{
		if(GetSkillCtrl.CurSkillStatus == eProcessStatus.Run)
		{
			GetSkillCtrl.UpdateSkillProcess();
			
			if(GetSkillCtrl.CurSkillStatus != eProcessStatus.Run)
			{
				ToJumpDown();
			}
		}
	}

	protected void ToJumpDown()
	{
		TMoveMessage moveMsg = new TMoveMessage();
		moveMsg.GetCmdType = eCommandType.Cmd_Move;
		moveMsg.GetActionType = eActionType.JumpDown;
		this.GetMsgCtrl.WaitList.Insert(0,moveMsg);
		
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
			InputManager.KeyJumpEnalbe = false;
		}
	}
}

