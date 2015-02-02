using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleForceFallDownAN : RoleDoAction
{
	protected THitMessage m_curMsg = null;
	
	public override void Initalize ()
	{
		this.m_name = "ForceFallDown";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg.GetActionType == eActionType.ForceFallDown)
		{
			return true;
		}
		else return false;
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		base.Enter(input);

		m_curMsg = GetFrontWaitMsg as THitMessage;
		GetMsgCtrl.AddRunTLMsg(GetFrontWaitMsg);
		GetMsgCtrl.RemoveWaitMsg(GetFrontWaitMsg);
	}

	private float m_jumpFloatTime = 0f; //浮空时间
	private float m_jumpFloatLastTime = 0f; //浮空剩余时间

	private Vector3 m_hitSpeed = Vector3.zero;
	
	protected override void StartAction()
	{
		GetRunTimeData.ActionType = eActionType.ForceFallDown;
		GetRunTimeData.MoveMethod = eMoveMethod.ForceSpeed;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
		GetRunTimeData.MoveDirection = CharacterUtil.GetMoveDirectionBySpeed(m_curMsg.hitSpeed.x);

		GetRunTimeData.IsTrigger = true;
		GetRunTimeData.CollisionFlag = CollisionFlags.None;

		m_jumpFloatLastTime = m_jumpFloatTime;
		m_hitSpeed = m_curMsg.hitSpeed;

		if(m_jumpFloatLastTime > 0)
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
		GetRunTimeData.ActiveChStateEnalbe = false;

		m_jumpFloatLastTime -= Time.deltaTime;
		if(m_jumpFloatLastTime <= 0)
		{
			GetRunTimeData.PostureType = ePostureType.Pose_HitFlyDown;
		}
		
		GetRunTimeData.ForceSpeed = new Vector3(0,0,0);
	}
	
	private void FlyDown()
	{
		GetRunTimeData.ActiveChStateEnalbe = false;

		float xSpeed = 0f;
		float ySpeed = 0f;
		float zSpeed = 0f;
		
		if(m_hitSpeed.magnitude != 0)
		{
			xSpeed = CharacterUtil.GetXSpeed(m_hitSpeed.x, RoleASpeedDef.AirXResistance,GetRunTimeData.MoveDirection);
			ySpeed = -Mathf.Abs(m_hitSpeed.y) - RoleASpeedDef.GravityAir * TimerManager.Instance.GetDeltaTime;
		
			m_hitSpeed = Vector3.zero;
		}
		else
		{
			xSpeed = CharacterUtil.GetXSpeed(GetRunTimeData.ForceSpeed.x, RoleASpeedDef.AirXResistance,GetRunTimeData.MoveDirection);
			ySpeed = GetRunTimeData.ForceSpeed.y - RoleASpeedDef.GravityAir * TimerManager.Instance.GetDeltaTime;
		}

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


