using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleForceDownAN : RoleDoAction
{
	protected THitMessage m_curMsg = null;

	public override void Initalize ()
	{
		this.m_name = "ForceDown";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		return true;
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		base.Enter(input);

		m_curMsg = GetFrontWaitMsg as THitMessage;
		GetMsgCtrl.AddRunTLMsg(GetFrontWaitMsg);
		GetMsgCtrl.RemoveWaitMsg(GetFrontWaitMsg);
	}
	
	protected float m_remainTime = 0f;

	protected override void StartAction()
	{
		GetRunTimeData.ActionType = eActionType.ForceHit;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
		
		m_remainTime = GetAniCtrl.GetState(AnimationNameDef.Hit1).length;
		
		UpdateAnimation();
	}
	
	protected override void DoAction ()
	{
		m_remainTime -= TimerManager.Instance.GetDeltaTime;

		if(m_remainTime <= 0)
		{
			Exit(null);
		}
	}

	protected override void NextAction ()
	{
		base.NextAction();
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
		base.Exit (input);
	}
	
	protected override void CheckSelfRule()
	{
		base.CheckSelfRule();
	}
}

