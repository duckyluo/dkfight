using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleJoinLieAN : RoleDoAction
{
	protected TNextMessage m_curMsg = null;
	
	public override void Initalize ()
	{
		this.m_name = "JoinLie";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg != null && GetFrontWaitMsg.GetActionType == eActionType.JoinLie)
		{
			return true;
		}
		else return false;
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		base.Enter(null);
		
		m_curMsg = GetFrontWaitMsg as TNextMessage;
		GetMsgCtrl.AddRunTLMsg(GetFrontWaitMsg);
		GetMsgCtrl.RemoveWaitMsg(GetFrontWaitMsg);
	}

	protected const float LieTime = 0.5f;
	protected float m_remainTime = 0f;
	
	protected override void StartAction()
	{
		GetRunTimeData.ActionType = eActionType.JoinLie;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
		GetRunTimeData.PostureType = ePostureType.Pose_Lie;

		m_remainTime = LieTime;

		UpdateAnimation();
		
		//m_remainTime = GetAniCtrl.GetState(AnimationNameDef.Hit1).length +  m_floatTime;
	}
	
	protected override void DoAction()
	{
		m_remainTime -= TimerManager.Instance.GetDeltaTime;
		
		if(m_remainTime <= 0 )
		{
			Exit(null);
		}
	}
	
	protected override void NextAction()
	{
		base.NextAction();
	}

	protected void UpdateAnimation()
	{
		GetRunTimeData.CurRotation = RoleRationDef.LieRotation;
		
		if(GetAniCtrl != null)
		{
			GetAniCtrl.Play(AnimationNameDef.Hit1, WrapMode.ClampForever,false,1);
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


