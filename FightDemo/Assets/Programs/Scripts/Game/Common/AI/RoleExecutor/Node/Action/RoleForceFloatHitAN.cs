using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleForceFloatHitAN : RoleDoAction
{
	protected THitMessage m_curMsg = null;
	
	public override void Initalize ()
	{
		this.m_name = "ForceFloatHit";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg.GetActionType == eActionType.ForceFloatHit)
		{
			return true;
		}
		else return false;
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		base.Enter(null);

		m_curMsg = GetFrontWaitMsg as THitMessage;
		GetMsgCtrl.AddRunTLMsg(GetFrontWaitMsg);
		GetMsgCtrl.RemoveWaitMsg(GetFrontWaitMsg);
	}


	protected const float m_floatTime = 0.1f;
	protected float m_remainTime = 0f;

	protected override void StartAction()
	{
		GetRunTimeData.ActionType = eActionType.ForceHit;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
		GetRunTimeData.PostureType = ePostureType.Pose_HitFlyFloat;

		UpdateAnimation();

		m_remainTime = GetAniCtrl.GetState(AnimationNameDef.Hit1).length +  m_floatTime;
	}
	
	protected override void DoAction()
	{
		m_remainTime -= TimerManager.Instance.GetDeltaTime;

		if(m_remainTime <= 0 )
		{
			ToFallDown();
		}
	}

	protected override void NextAction()
	{
		base.NextAction();
	}

	protected void ToFallDown()
	{
		TNextMessage msg = new TNextMessage();
		msg.GetCmdType = eCommandType.Cmd_Hit;
		msg.GetActionType = eActionType.JoinFallen;
		msg.GetLookDirection = GetRunTimeData.LookDirection;
		this.GetMsgCtrl.NextTLMsg(msg);

		Exit(null);
	}
	
	protected void UpdateAnimation()
	{
		GetRunTimeData.CurRotation = RoleRationDef.FloatHitRotation;

		if(GetAniCtrl != null)
		{
			GetAniCtrl.Play(AnimationNameDef.Hit1, WrapMode.ClampForever,true,1);
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
	}
}

