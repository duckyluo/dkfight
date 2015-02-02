using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleDoJumpDownAN : RoleDoAction
{
	protected TMoveMessage m_curMsg = null;
	
	public override void Initalize ()
	{
		this.m_name = "DoJumpDown";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg.GetActionType == eActionType.JumpDown)
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

	private float m_jumpFloatTime = 0.5f; //浮空时间
	private float m_jumpFloatLastTime = 0f; //浮空剩余时间
	
	protected override void StartAction()
	{
		GetRunTimeData.ActionType = eActionType.JumpDown;
		GetRunTimeData.MoveMethod = eMoveMethod.Jump;
		GetRunTimeData.PostureType = ePostureType.Pose_JumpFloat;
		GetRunTimeData.ActiveChStateEnalbe = true;
		GetRunTimeData.IsTrigger = true;
		GetRunTimeData.CollisionFlag = CollisionFlags.None;

		m_jumpFloatLastTime = m_jumpFloatTime;

		UpdateAnimation();
	}
	
	protected override void DoAction()
	{
		switch(GetRunTimeData.PostureType)
		{
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

	private void JumpFloat()
	{
		m_jumpFloatLastTime -= Time.deltaTime;
		if(m_jumpFloatLastTime <= 0)
		{
			GetRunTimeData.PostureType = ePostureType.Pose_JumpDown;
		}
		
		GetRunTimeData.ForceSpeed = new Vector3(0,0,0);
	}
	
	private void JumpDown()
	{
		float xSpeed = 0f;
		float ySpeed = GetRunTimeData.ForceSpeed.y - RoleASpeedDef.GravityAir * TimerManager.Instance.GetDeltaTime;
		float zSpeed = 0f;

		float nextYPos = ( GetRunTimeData.CurPos.y + 0.8f  ) + ySpeed * TimerManager.Instance.GetDeltaTime;//Time.deltaTime;//to fix resource ! shit	
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
