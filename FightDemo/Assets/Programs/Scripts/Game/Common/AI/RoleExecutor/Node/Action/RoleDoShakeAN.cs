using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleDoShakeAN : RoleDoAction
{
	TActionMessage m_curMsg = null;

	public override void Initalize ()
	{
		this.m_name = "DoShake";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg != null && GetFrontWaitMsg.GetActionType == eActionType.Shake)
		{
			return true;
		}
		else return false;
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		base.Enter(input);
		
		m_curMsg = GetFrontWaitMsg as TActionMessage;
		GetMsgCtrl.AddRunTLMsg(GetFrontWaitMsg);
		GetMsgCtrl.RemoveWaitMsg(GetFrontWaitMsg);
	}
	
	protected override void StartAction()
	{
		GetRunTimeData.ActionType = eActionType.Shake;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
		GetRunTimeData.MoveMethod = eMoveMethod.Gravity;
		GetRunTimeData.PostureType = ePostureType.Pose_Stand;

		GetRunTimeData.PassiveChStateEnalbe = true;
		GetRunTimeData.UseGravity = eUseGravity.Yes;
		
		UpdateAnimation();

		GetTransformCtrl.GetTweenTool.Shake();
	}
	
	protected override void DoAction()
	{
		if(GetTransformCtrl.GetTweenTool.IsShaking == false)
		{
			Exit(null);
		}
	}
	
	protected override void NextAction ()
	{
		base.NextAction ();
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
		GetTransformCtrl.GetTweenTool.StopShake();
		base.Exit (input);
	}
	
	protected override void CheckSelfRule()
	{
		base.CheckSelfRule();
		InputManager.KeyJumpEnalbe = true;
	}
	
}