using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

/// <summary>
/// Role do idle.
/// </summary>
public class RoleDoIdleAN : RoleDoAction
{
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
		base.Enter(input);
	}
	
	protected override void StartAction()
	{
		GetRunTimeData.ActionType = eActionType.Idle;
		GetRunTimeData.MoveMethod = eMoveMethod.Gravity;
		GetRunTimeData.PostureType = ePostureType.Pose_Stand;

		GetRunTimeData.MoveEnable = true;
		GetRunTimeData.ActiveChStateEnalbe = true;
		GetRunTimeData.PassiveChStateEnalbe = true;
		GetRunTimeData.UseGravity = eUseGravity.Yes;

		UpdateAnimation();
	}

	protected override void DoAction()
	{

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
		base.Exit (input);
	}

	protected override void CheckSelfRule()
	{
		base.CheckSelfRule();
		InputManager.KeyJumpEnalbe = true;
	}

}