using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleJoinFallenAN : RoleDoAction
{
	protected TNextMessage m_curMsg = null;
	
	public override void Initalize ()
	{
		this.m_name = "JoinFallen";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg.GetActionType == eActionType.JoinFallen)
		{
			return true;
		}
		else return false;
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		base.Enter(input);
		
		m_curMsg = GetFrontWaitMsg as TNextMessage;
		GetMsgCtrl.AddRunTLMsg(GetFrontWaitMsg);
		GetMsgCtrl.RemoveWaitMsg(GetFrontWaitMsg);
	}
	
	protected const float FloatTime = 0f; //浮空时间
	protected float m_floatLastTime = 0f; //浮空剩余时间
	
	private Vector3 m_hitSpeed = Vector3.zero;
	
	protected override void StartAction()
	{
		GetRunTimeData.ActionType = eActionType.JoinFallen;
		GetRunTimeData.MoveMethod = eMoveMethod.ForceSpeed;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
		
		GetRunTimeData.IsTrigger = true;
		GetRunTimeData.CollisionFlag = CollisionFlags.None;
		
		m_floatLastTime = FloatTime;

		if(m_floatLastTime > 0)
		{
			GetRunTimeData.PostureType = ePostureType.Pose_HitFlyFloat;
		}
		else
		{
			GetRunTimeData.PostureType = ePostureType.Pose_HitFlyDown;
		}
		
		UpdateAnimation();
	}
	
	protected override void DoAction()
	{
		switch(GetRunTimeData.PostureType)
		{
		case ePostureType.Pose_HitFlyFloat:
			FlyFloat();
			break;
		case ePostureType.Pose_HitFlyDown:
			FlyDown();
			break;
		}
	}
	
	protected override void NextAction ()
	{
		base.NextAction ();
	}
	
	private void FlyFloat()
	{
		m_floatLastTime -= TimerManager.Instance.GetDeltaTime;
		if(m_floatLastTime <= 0)
		{
			GetRunTimeData.PostureType = ePostureType.Pose_HitFlyDown;
		}
		
		GetRunTimeData.ForceSpeed = new Vector3(0,0,0);
	}
	
	private void FlyDown()
	{
		float xSpeed = 0f;
		float ySpeed = 0f;
		float zSpeed = 0f;

		ySpeed = GetRunTimeData.ForceSpeed.y - RoleASpeedDef.GravityAir * TimerManager.Instance.GetDeltaTime;

		GetRunTimeData.ForceSpeed = new Vector3(xSpeed,ySpeed,zSpeed);
		
		float nextYPos = GetRunTimeData.CurPos.y + ySpeed * TimerManager.Instance.GetDeltaTime;//Time.deltaTime;//to fix resource ! shit	
		if(nextYPos <= GetTransformCtrl.GetFloorHeight)
		{
			Vector3 motion = new Vector3(0,GetTransformCtrl.GetFloorHeight - GetRunTimeData.CurPos.y,0);
			GetTransformCtrl.MoveLimit(motion);
			
			ToJoinImpact();
		}
	}
	
	protected void UpdateAnimation()
	{
		GetRunTimeData.CurRotation =  RoleRationDef.FallRotation;
		
		if(GetAniCtrl != null)
		{
			GetAniCtrl.Play(AnimationNameDef.Hit1, WrapMode.ClampForever,true,1);
		}
	}
	
	protected void ToJoinImpact()
	{
		TNextMessage msg = new TNextMessage();
		msg.GetCmdType = eCommandType.Cmd_Hit;
		msg.GetActionType = eActionType.JoinImpact;
		msg.GetLookDirection = GetRunTimeData.LookDirection;
		msg.hitSpeed = GetRunTimeData.ForceSpeed;
		msg.impactMothod = eImpactMothod.Ground;
		this.GetMsgCtrl.NextTLMsg(msg);
		
		Exit(null);
	}
	
	protected override void Exit (DkBtInputParam input)
	{
		m_curMsg = null;
		base.Exit (input);
	}
	
	protected  override void CheckSelfRule()
	{
		base.CheckSelfRule();
	}
}


