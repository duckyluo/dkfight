using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleForceMotionAN : RoleDoAction
{
	protected THitMessage m_curMsg = null;
	
	public override void Initalize ()
	{
		this.m_name = "ForceMotion";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg.GetActionType == eActionType.ForceMotion)
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

	protected const float FloatTime = 1f;
	protected float m_remainTime = 0f;
	protected Vector3 m_aSpeed = Vector3.zero;
	protected Vector3 m_startSpeed = Vector3.zero;
	protected Vector3 m_startPos = Vector3.zero;
	protected Vector3 m_endPos = Vector3.zero;
	protected float m_dis = 0f;
	
	protected override void StartAction()
	{
		GetRunTimeData.ActionType = eActionType.ForceMotion;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
		
		UpdateAnimation();

		m_remainTime = m_curMsg.hitDuration;
		m_startPos = GetRunTimeData.CurPos;
		m_endPos = GetRunTimeData.CurPos + m_curMsg.hitSpeed;
		m_dis = Vector3.Distance(m_endPos,m_startPos);

		if(m_dis == 0)
		{
			GetRunTimeData.PostureType = ePostureType.Pose_Hit;
		}
		else if(m_remainTime > 0)
		{
			GetRunTimeData.PostureType = ePostureType.Pose_HitFlyUp;
	
			float moveDis = m_curMsg.hitSpeed.magnitude;
			Vector3 direation = m_curMsg.hitSpeed.normalized;
			float aspeed = moveDis * 2 / (m_remainTime * m_remainTime);
			float startSpeed = aspeed * m_remainTime;
			m_aSpeed = aspeed * direation;
			m_startSpeed = startSpeed * direation;
			
			GetRunTimeData.ForceSpeed = m_startSpeed;
			GetRunTimeData.MoveMethod = eMoveMethod.ForceSpeed;

		}
		else 
		{
			GetRunTimeData.PostureType = ePostureType.Pose_HitFlyFloat;
		}
	}
	
	protected override void DoAction()
	{
		m_remainTime -= TimerManager.Instance.GetDeltaTime;

		if(GetRunTimeData.PostureType == ePostureType.Pose_Hit)
		{
			if(m_remainTime <= 0)
			{
				Debug.Log("============================== 1"); 
				GetRunTimeData.PostureType = ePostureType.Pose_HitFlyFloat;
				m_remainTime = FloatTime;
			}
		}
		else if(GetRunTimeData.PostureType == ePostureType.Pose_HitFlyUp)
		{
			if(m_remainTime <= 0)
			{
				//Debug.Log("============================== 2"); 
				GetRunTimeData.PostureType = ePostureType.Pose_HitFlyFloat;
				GetRunTimeData.ForceSpeed = Vector3.zero;
				m_remainTime = FloatTime;
			}
			else
			{

				Vector3 speed = GetRunTimeData.ForceSpeed - m_aSpeed * TimerManager.Instance.GetDeltaTime;
				if(GetRunTimeData.ForceSpeed.x >= 0 && speed.x < 0)
				{
					speed.x = 0;
				}
				else if(GetRunTimeData.ForceSpeed.x <= 0 && speed.x > 0)
				{
					speed.x = 0;
				}
				
				if(GetRunTimeData.ForceSpeed.y >= 0 && speed.y < 0)
				{
					speed.y = 0;
				}
				else if(GetRunTimeData.ForceSpeed.y <= 0 && speed.y > 0)
				{
					speed.y = 0;
				}
				
				GetRunTimeData.ForceSpeed = speed;
				
				Vector3 nextPos = GetRunTimeData.CurPos + GetRunTimeData.ForceSpeed * TimerManager.Instance.GetDeltaTime;
				float dis = Vector3.Distance(nextPos , m_startPos);
				if(m_dis <= dis)
				{
					//Debug.Log("============================== 3 " + m_remainTime); 
					GetRunTimeData.PostureType = ePostureType.Pose_HitFlyFloat;
					GetRunTimeData.ForceSpeed = Vector3.zero;
					m_remainTime = FloatTime;
				}
			}
		}
		else if(GetRunTimeData.PostureType == ePostureType.Pose_HitFlyFloat)
		{
			if(m_remainTime <= 0)
			{
				//Debug.Log("============================== 4"); 
				MotionEnd();
			}
		}
	}

	protected void MotionEnd()
	{
		Vector3 motion = m_endPos - GetRunTimeData.CurPos;
		GetTransformCtrl.MoveLimit(motion);

		if(GetRunTimeData.IsOnAir)
		{
			ToFallDown();
		}
		else 
		{
			ToIdle();
		}
	}
	
	protected override void NextAction()
	{
		base.NextAction();
	}

	protected void ToIdle()
	{
		Exit(null);
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
		GetRunTimeData.CurRotation = RoleRationDef.StandRotation;
		
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

