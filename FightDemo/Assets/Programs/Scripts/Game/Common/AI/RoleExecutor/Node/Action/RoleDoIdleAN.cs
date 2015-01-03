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

	public override bool Evaluate (DkBtInputParam input)
	{
		return true;
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		InputManager.KeyJumpEnalbe = true;

		GetRunTimeData.MoveMethod = eMoveMethod.None;
		GetRunTimeData.MoveDirection = eMoveDirection.None;
		GetRunTimeData.PostureType = ePostureType.Pose_Idle;

		GetRunTimeData.MoveEnable = true;
		GetRunTimeData.ActiveChStateEnalbe = true;
		GetRunTimeData.PassiveChStateEnalbe = true;
		GetRunTimeData.UseGravity = eUseGravity.Yes;

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
				if((waitMsg as THitMessage).damageForce == eDamageForce.None)
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
		if(m_nextMsg == null)
		{
			UpdateAnimation();
		}
		else
		{
			Exit(null);
		}
	}
	
	protected void UpdateAnimation()
	{
		if(GetAniCtrl != null)
		{
			GetAniCtrl.Play(AnimationNameDef.Idle,WrapMode.Loop,false);
		}
	}

	protected override void Exit (DkBtInputParam input)
	{
		m_nextMsg = null;
		base.Exit (input);
	}

}