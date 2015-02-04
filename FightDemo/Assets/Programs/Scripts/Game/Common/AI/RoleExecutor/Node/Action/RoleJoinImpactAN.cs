using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleJoinImpactAN : RoleDoAction
{
	protected TNextMessage m_curMsg = null;
	
	public override void Initalize ()
	{
		this.m_name = "JoinImpact";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg.GetActionType == eActionType.JoinImpact)
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
	
	protected const float m_jumpFloatTime = 0.0f; //浮空时间
	protected float m_jumpFloatLastTime = 0f; //浮空剩余时间
	protected Vector3 m_hitSpeed = Vector3.zero;
	
	protected override void StartAction()
	{
		m_jumpFloatLastTime = m_jumpFloatTime;
		m_hitSpeed = CharacterUtil.GetImpactSpeedBy(m_curMsg.hitSpeed , m_curMsg.impactMothod);
		
		GetRunTimeData.ActionType = eActionType.JoinImpact;
		GetRunTimeData.MoveMethod = eMoveMethod.ForceSpeed;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
		GetRunTimeData.MoveDirection = CharacterUtil.GetMoveDirectionBySpeed(m_curMsg.hitSpeed.x);
		GetRunTimeData.PostureType = ePostureType.Pose_None;
		GetRunTimeData.CollisionFlag = CollisionFlags.None;

		UpdateAnimation();
	}
	
	protected override void DoAction()
	{
		switch(GetRunTimeData.PostureType)
		{
		case ePostureType.Pose_None:
		case ePostureType.Pose_HitFlyUp:
			FlyUp();
			break;
		case ePostureType.Pose_HitFlyFloat:
			FlyFloat();
			break;
		case ePostureType.Pose_HitFlyDown:
			FlyDown();
			break;
		}
	}
	
	protected override void NextAction()
	{
		base.NextAction();
	}
	
	private void FlyUp()
	{
		float xSpeed = 0;
		float ySpeed = 0;
		float zSpeed = 0;
		
		if(GetRunTimeData.PostureType == ePostureType.Pose_None)
		{
			GetRunTimeData.PostureType = ePostureType.Pose_HitFlyUp;
			
			xSpeed = CharacterUtil.GetXSpeed(m_hitSpeed.x, RoleASpeedDef.AirXResistance,GetRunTimeData.MoveDirection);
			ySpeed = m_hitSpeed.y - RoleASpeedDef.GravityAir * TimerManager.Instance.GetDeltaTime;
			
			m_hitSpeed = Vector3.zero;

			SoundManager.PlaySound(SoundDef.FallOne);
			//Debug.Log(" ==================== "+GetRunTimeData.MoveDirection + " "+ xSpeed + " "+m_hitSpeed.x);
		}
		else
		{
			xSpeed = CharacterUtil.GetXSpeed(GetRunTimeData.ForceSpeed.x, RoleASpeedDef.AirXResistance,GetRunTimeData.MoveDirection);
			ySpeed = GetRunTimeData.ForceSpeed.y - RoleASpeedDef.GravityAir * TimerManager.Instance.GetDeltaTime;
		}
		
		if(ySpeed <= 0)
		{
			ySpeed = 0;
			GetRunTimeData.PostureType = ePostureType.Pose_HitFlyFloat;
		}
		
		GetRunTimeData.ForceSpeed = new Vector3(xSpeed,ySpeed,zSpeed);
	}
	
	private void FlyFloat()
	{
		float xSpeed = CharacterUtil.GetXSpeed(GetRunTimeData.ForceSpeed.x, RoleASpeedDef.AirXResistance,GetRunTimeData.MoveDirection);
		float ySpeed = 0;
		float zSpeed = 0;
		
		m_jumpFloatLastTime -= TimerManager.Instance.GetDeltaTime;
		if(m_jumpFloatLastTime <= 0)
		{
			GetRunTimeData.PostureType = ePostureType.Pose_HitFlyDown;
			GetRunTimeData.ActiveChStateEnalbe = false;
			GetRunTimeData.IsTrigger = true;
		}
		
		GetRunTimeData.ForceSpeed = new Vector3(xSpeed,ySpeed,zSpeed);
	}
	
	private void FlyDown()
	{
		float xSpeed = CharacterUtil.GetXSpeed(GetRunTimeData.ForceSpeed.x, RoleASpeedDef.AirXResistance,GetRunTimeData.MoveDirection);
		float ySpeed = GetRunTimeData.ForceSpeed.y - RoleASpeedDef.GravityAir * TimerManager.Instance.GetDeltaTime;
		float zSpeed = 0;
		
		float nextYPos = GetRunTimeData.CurPos.y + ySpeed * TimerManager.Instance.GetDeltaTime;//to fix resource ! shit	
		if(nextYPos <= GetTransformCtrl.GetFloorHeight)
		{
			Vector3 motion = new Vector3(0,GetTransformCtrl.GetFloorHeight - GetRunTimeData.CurPos.y,0);
			GetTransformCtrl.MoveLimit(motion);

			SoundManager.PlaySound(SoundDef.FallTwo);
			ToJoinLie();
		}
		else
		{
			GetRunTimeData.ForceSpeed = new Vector3(xSpeed,ySpeed,zSpeed);
		}
	}

	protected void ToJoinLie()
	{
		TNextMessage msg = new TNextMessage();
		msg.GetCmdType = eCommandType.Cmd_Action;
		msg.GetActionType = eActionType.JoinLie;
		msg.GetLookDirection = GetRunTimeData.LookDirection;
		this.GetMsgCtrl.NextTLMsg(msg);
		Exit(null);
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

