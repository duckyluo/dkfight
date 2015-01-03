using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

/// <summary>
/// Role do move.
/// </summary>
public class RoleDoMoveDirectionAN : RoleBaseActionNode
{
	protected TMoveMessage m_curMsg = null;

	protected TimeLineMessage m_nextMsg = null;

	public override bool Evaluate (DkBtInputParam input)
	{
		TMoveMessage moveMsg = GetFrontWaitMsg as TMoveMessage;
		if(moveMsg != null)
		{
			if(moveMsg.moveMethod == eMoveMethod.Direction)
			{
				return true;
			}
			else return false;
		}
		else
		{
			Debug.LogError("something impossible");
			return false;
		}
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		InputManager.KeyJumpEnalbe = true;

		GetRunTimeData.MoveMethod = eMoveMethod.None;
		GetRunTimeData.MoveDirection = eMoveDirection.None;
		GetRunTimeData.LookDirection = eLookDirection.None;
		GetRunTimeData.PostureType = ePostureType.Pose_None;

		GetRunTimeData.MoveEnable = true;
		GetRunTimeData.ActiveChStateEnalbe = true;
		GetRunTimeData.PassiveChStateEnalbe = true;
		GetRunTimeData.UseGravity = eUseGravity.Yes;
		GetRunTimeData.ForceSpeed = Vector3.zero;

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
			Exit(null);//to do
		}
		else
		{
			if(GetRunTimeData.MoveMethod == eMoveMethod.None)
			{
				GetRunTimeData.MoveMethod = eMoveMethod.Direction;
				GetRunTimeData.MoveDirection = m_curMsg.moveDirection;
				GetRunTimeData.LookDirection = m_curMsg.lookDirection;
				GetRunTimeData.PostureType = ePostureType.Pose_RUN;

				UpdateAnimation();
			}
		}
	}

	protected void UpdateAnimation()
	{
		if(GetAniCtrl != null)
		{
			GetAniCtrl.Play(AnimationNameDef.Run,WrapMode.Loop,false);
		}
	}
	
	protected override void Exit (DkBtInputParam input)
	{
		m_curMsg = null;
		m_nextMsg = null;
		base.Exit (input);
	}
}