using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

/// <summary>
/// Role do move.
/// </summary>
public class RoleDoMoveDirectionAN : RoleDoAction
{
	protected TMoveMessage m_curMsg = null;

	public override void Initalize ()
	{
		this.m_name = "DoMove";
		base.Initalize ();
	}

	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg.GetActionType == eActionType.Move)
		{
			return true;
		}
		else return false;
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		base.Enter(input);

		m_curMsg = GetFrontWaitMsg as TMoveMessage;
		GetMsgCtrl.AddRunTLMsg(GetFrontWaitMsg);
		GetMsgCtrl.RemoveWaitMsg(GetFrontWaitMsg);
	}
	
	protected float m_XSpeed = 0f;

	protected override void StartAction()
	{
		GetRunTimeData.ActionType = eActionType.Move;
		GetRunTimeData.MoveMethod = eMoveMethod.Direction;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
		GetRunTimeData.MoveDirection = CharacterUtil.GetMoveDirectionByLook(m_curMsg.GetLookDirection);

		GetRunTimeData.MoveEnable = true;
		GetRunTimeData.ActiveChStateEnalbe = true;
		GetRunTimeData.PassiveChStateEnalbe = true;
		GetRunTimeData.UseGravity = eUseGravity.Yes;

		UpdateAnimation();

		m_XSpeed = GetXSpeed();
	}

	protected override void DoAction()
	{
		if(GetRunTimeData.PostureType == ePostureType.Pose_None)
		{
			GetRunTimeData.PostureType = ePostureType.Pose_RUN;

			GetRunTimeData.ForceSpeed = new Vector3(m_XSpeed,0,0);
		}
	}

	protected override void NextAction ()
	{
		base.NextAction ();
	}

	private float GetXSpeed()
	{
		float xSpeed = 0;
		if(GetRunTimeData.MoveDirection == eMoveDirection.Right)
		{
			xSpeed = RoleSpeedDef.RunXSpeed;
		}
		else
		{
			xSpeed = -RoleSpeedDef.RunXSpeed;
		}
		return xSpeed;
	}

	protected void UpdateAnimation()
	{
		if(GetAniCtrl != null)
		{
			GetAniCtrl.Play(AnimationNameDef.Run,WrapMode.Loop,false,1f);
		}
	}
	
	protected override void Exit (DkBtInputParam input)
	{
		m_curMsg = null;
		base.Exit (input);
	}

	protected override void CheckSelfRule()
	{
		base.CheckSelfRule();
		InputManager.KeyJumpEnalbe = true;
	}
}