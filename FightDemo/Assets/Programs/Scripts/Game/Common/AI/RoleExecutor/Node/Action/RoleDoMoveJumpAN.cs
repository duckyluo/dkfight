using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleDoMoveJumpAN : RoleDoAction
{
	protected TMoveMessage m_curMsg = null;

	public override void Initalize ()
	{
		this.m_name = "DoJump";
		base.Initalize ();
	}

	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg.GetActionType == eActionType.Jump)
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
	
	private float m_jumpHeight = 1.6f; //上升高度
	private float m_jumpUpTime = 0.3f; //上升时间
	private float m_jumpUpAV = 0f; //上升加速度
	private float m_jumpFloatTime = 0.3f; //浮空时间
	private float m_jumpFloatLastTime = 0f; //浮空剩余时间
	private float m_jumpDownAV = 0f; //下降加速度
	private Vector3 m_startJumpSpeed = Vector3.zero;
	
	protected override void StartAction()
	{
		GetRunTimeData.ActionType = eActionType.Jump;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
		GetRunTimeData.MoveMethod = eMoveMethod.Jump;
		GetRunTimeData.MoveDirection = CharacterUtil.GetMoveDirectionByJump(m_curMsg.jumpDirection);

		GetRunTimeData.ActiveChStateEnalbe = true;
		GetRunTimeData.PassiveChStateEnalbe = true;
		GetRunTimeData.IsTrigger = true;
		GetRunTimeData.CollisionFlag = CollisionFlags.None;
		
		m_jumpFloatLastTime = m_jumpFloatTime;
		m_jumpUpAV = -m_jumpHeight/(m_jumpUpTime * m_jumpUpTime);

		m_jumpDownAV = RoleASpeedDef.GravityAir;

		if(GetRunTimeData.MoveDirection != eMoveDirection.None)
		{
			m_startJumpSpeed.x = RoleSpeedDef.JumpXSpeed;
		}
		m_startJumpSpeed.y = m_jumpHeight * 2 / m_jumpUpTime;
	}
	
	protected override void DoAction()
	{
		switch(GetRunTimeData.PostureType)
		{
		case ePostureType.Pose_None:
		case ePostureType.Pose_JumpUp:
			JumpUp();
			break;
		case ePostureType.Pose_JumpFloat:
			JumpFloat();
			break;
		case ePostureType.Pose_JumpDown:
			JumpDown();
			break;
		}
	}

	protected override void NextAction ()
	{
		base.NextAction ();
	}

	private void JumpUp()
	{

		float xSpeed = 0;
		float ySpeed = 0;
		float zSpeed = 0;

		if(GetRunTimeData.PostureType == ePostureType.Pose_None)
		{
			GetRunTimeData.PostureType = ePostureType.Pose_JumpUp;
			UpdateAnimation();

			xSpeed = CharacterUtil.GetXSpeed(m_startJumpSpeed.x,RoleASpeedDef.AirXResistance,GetRunTimeData.MoveDirection);
			ySpeed = m_startJumpSpeed.y - m_jumpUpAV * TimerManager.Instance.GetDeltaTime;

			m_startJumpSpeed = Vector3.zero;
		}
		else
		{
			xSpeed = CharacterUtil.GetXSpeed(GetRunTimeData.ForceSpeed.x,RoleASpeedDef.AirXResistance,GetRunTimeData.MoveDirection);
			ySpeed = GetRunTimeData.ForceSpeed.y - m_jumpUpAV * TimerManager.Instance.GetDeltaTime;
		}

		if(ySpeed <= 0)
		{
			ySpeed = GetRunTimeData.ForceSpeed.y;
			Debug.Log("[warn][jump speed] "+m_jumpUpAV +"  ==== "+ySpeed+" ==== "+Time.deltaTime);
		}
		
		float height = GetRunTimeData.CurPos.y + ySpeed * TimerManager.Instance.GetDeltaTime;
		if(height >= m_jumpHeight )
		{

			Vector3 motion = new Vector3(0,m_jumpHeight - GetRunTimeData.CurPos.y,0);
			GetTransformCtrl.MoveLimit(motion);

			GetRunTimeData.PostureType = ePostureType.Pose_JumpFloat;
			ySpeed = 0;
		}

		GetRunTimeData.ForceSpeed = new Vector3(xSpeed,ySpeed,zSpeed);
	}

	private void JumpFloat()
	{

		float xSpeed = CharacterUtil.GetXSpeed(GetRunTimeData.ForceSpeed.x,RoleASpeedDef.AirXResistance,GetRunTimeData.MoveDirection);
		float ySpeed = 0;
		float zSpeed = 0;

		m_jumpFloatLastTime -= TimerManager.Instance.GetDeltaTime;
		if(m_jumpFloatLastTime <= 0)
		{
			GetRunTimeData.PostureType = ePostureType.Pose_JumpDown;
		}

		GetRunTimeData.ForceSpeed = new Vector3(xSpeed,ySpeed,zSpeed);
	}

	private void JumpDown()
	{
		float xSpeed = CharacterUtil.GetXSpeed(GetRunTimeData.ForceSpeed.x,RoleASpeedDef.AirXResistance,GetRunTimeData.MoveDirection);
		float ySpeed = GetRunTimeData.ForceSpeed.y - m_jumpDownAV * TimerManager.Instance.GetDeltaTime;
		float zSpeed = 0;

		float nextYPos = ( GetRunTimeData.CurPos.y + 0.8f ) + ySpeed * TimerManager.Instance.GetDeltaTime;//to fix resource ! shit	
		if(nextYPos <= GetTransformCtrl.GetFloorHeight)
		{
			Vector3 motion = new Vector3(0,GetTransformCtrl.GetFloorHeight - GetRunTimeData.CurPos.y,0);
			GetTransformCtrl.MoveLimit(motion);

			GetRunTimeData.ForceSpeed = Vector3.zero;
			Exit(null);
		}
		else
		{
			GetRunTimeData.ForceSpeed = new Vector3(xSpeed,ySpeed,zSpeed);
		}
	}
	
	protected void UpdateAnimation()
	{
		if(GetAniCtrl != null)
		{
			GetAniCtrl.Play(AnimationNameDef.Jump,WrapMode.ClampForever,false,1f);
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
