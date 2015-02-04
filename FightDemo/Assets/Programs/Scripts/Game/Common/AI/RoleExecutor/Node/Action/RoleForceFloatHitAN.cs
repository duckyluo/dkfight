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


	protected const float FloatTime = 0.1f;
	protected const float ASpeed = 10f;
	protected float m_remainTime = 0f;
	protected Vector3 m_direation = Vector3.zero;
	protected Vector3 m_aSpeed = Vector3.zero;
	protected float m_startSpeed = 0f;
	
	protected override void StartAction()
	{
		GetRunTimeData.ActionType = eActionType.ForceHit;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;

		UpdateAnimation();

		if(m_curMsg.hitSpeed.sqrMagnitude != 0)
		{
			m_remainTime = GetAniCtrl.GetState(AnimationNameDef.Hit1).length;
			float moveDis = m_curMsg.hitSpeed.magnitude;
			m_direation = m_curMsg.hitSpeed.normalized;
			m_startSpeed = (moveDis / m_remainTime) + (ASpeed * m_remainTime / 2);
			m_aSpeed = m_direation * ASpeed ;
		}
		else
		{
			m_remainTime = GetAniCtrl.GetState(AnimationNameDef.Hit1).length +  FloatTime;
			GetRunTimeData.PostureType = ePostureType.Pose_HitFlyFloat;
		}
	}
	
	protected override void DoAction()
	{
		m_remainTime -= TimerManager.Instance.GetDeltaTime;

		if(GetRunTimeData.PostureType == ePostureType.Pose_None)
		{
			GetRunTimeData.PostureType = ePostureType.Pose_HitFlyUp;
			GetRunTimeData.MoveMethod = eMoveMethod.ForceSpeed;
			GetRunTimeData.ForceSpeed = m_direation * m_startSpeed;
		}
		else if(GetRunTimeData.PostureType == ePostureType.Pose_HitFlyUp)
		{
			if(m_remainTime <= 0)
			{
				m_remainTime = FloatTime;
				GetRunTimeData.PostureType = ePostureType.Pose_HitFlyFloat;
				GetRunTimeData.MoveMethod = eMoveMethod.None;
				GetRunTimeData.ForceSpeed = Vector3.zero;
			}
			else
			{
				Vector3 curSpeed = GetRunTimeData.ForceSpeed - m_aSpeed * TimerManager.Instance.GetDeltaTime;
				GetRunTimeData.ForceSpeed = curSpeed;
			}
		}
		else if(GetRunTimeData.PostureType == ePostureType.Pose_HitFlyFloat)
		{
			if(m_remainTime <= 0 )
			{
				ToFallDown();
			}
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

