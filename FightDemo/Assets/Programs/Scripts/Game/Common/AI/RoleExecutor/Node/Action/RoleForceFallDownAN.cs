using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleForceFallDownAN : RoleBaseActionNode
{
	protected THitMessage m_curMsg = null;
	
	protected TimeLineMessage m_nextMsg = null;
	
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
		CheckSelfRule();
		
		GetRunTimeData.ActionType = eActionType.None;
		GetRunTimeData.MoveMethod = eMoveMethod.None;
		GetRunTimeData.MoveDirection = eMoveDirection.None;
		//GetRunTimeData.LookDirection = eLookDirection.None;
		GetRunTimeData.PostureType = ePostureType.Pose_None;
		
		GetRunTimeData.MoveEnable = false;
		GetRunTimeData.ActiveChStateEnalbe = false;
		GetRunTimeData.PassiveChStateEnalbe = false;
		GetRunTimeData.UseGravity = eUseGravity.No;
		GetRunTimeData.IsTrigger = true;

		GetRunTimeData.ForceSpeed = Vector3.zero;
		GetRunTimeData.CurAlpha = 1f;
		GetRunTimeData.CurScale = 1f;
		GetRunTimeData.CurRotation = new Vector3(-20f,0,0);
		
		m_curMsg = GetFrontWaitMsg as THitMessage;
		GetMsgCtrl.AddRunTLMsg(GetFrontWaitMsg);
		GetMsgCtrl.RemoveWaitMsg(GetFrontWaitMsg);
		
		m_nextMsg = null;
		
		base.Enter(input);
	}
	
	protected override void Exectue (DkBtInputParam input)
	{
		UpdateCurStatus();
	}
	
	protected void UpdateCurStatus()
	{
		CheckNextMsg();
		UpdateHitMsg();
		UpdateMoveMsg();
		UpdateCurMsg();
	}
	
	protected void CheckNextMsg()
	{
		while(GetFrontWaitMsg != null)
		{
			TimeLineMessage waitMsg = GetFrontWaitMsg;
			
			if(waitMsg.GetCmdType == eCommandType.Cmd_Hit)
			{
				if(waitMsg.GetActionType == eActionType.Not_Use)
				{
					GetMsgCtrl.AddRunTLMsg(waitMsg);
					GetMsgCtrl.RemoveWaitMsg(waitMsg);
					
					continue;
				}
				else
				{
					m_nextMsg = waitMsg;
					break;
				}
			}
			else 
			{
				m_nextMsg = waitMsg;
				break;
			}
		}
	}
	
	protected void UpdateHitMsg()
	{
		if (GetMsgCtrl.HitList.Count > 0) 
		{
			GetMsgCtrl.HitList.Clear();// to do
		}
	}
	
	protected void UpdateMoveMsg()
	{
		if(GetMsgCtrl.MoveList.Count > 0)
		{
			GetMsgCtrl.MoveList.Clear();// to do
		}
	}
	
	protected void UpdateCurMsg()
	{
		if(m_nextMsg != null)
		{
			Exit(null);
		}
		else if(GetRunTimeData.ActionType == eActionType.None)
		{
			StartFloatDown();
		}
		else
		{
			DoFloatDowning();
		}
	}

	private float m_jumpFloatTime = 0.5f; //浮空时间
	private float m_jumpFloatLastTime = 0f; //浮空剩余时间
	private float m_jumpDownAV = 30f; //下降加速度
	
	private void StartFloatDown()
	{
		GetRunTimeData.ActionType = eActionType.ForceFallDown;
		GetRunTimeData.MoveMethod = eMoveMethod.ForceSpeed;
		GetRunTimeData.PostureType = ePostureType.Pose_HitFlyFloat;
		GetRunTimeData.CollisionFlag = CollisionFlags.None;
		
		m_jumpFloatLastTime = m_jumpFloatTime;
	}
	
	private void DoFloatDowning()
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
		
		float yspeed =  GetRunTimeData.ForceSpeed.y;
		yspeed -= m_jumpDownAV * TimerManager.Instance.GetDeltaTime;//Time.deltaTime;
		float nextYPos = GetRunTimeData.CurPos.y + yspeed * TimerManager.Instance.GetDeltaTime;//Time.deltaTime;//to fix resource ! shit	
		if(nextYPos <= GetTransformCtrl.GetFloorHeight)
		{
			Vector3 motion = new Vector3(0,GetTransformCtrl.GetFloorHeight - GetRunTimeData.CurPos.y,0);
			GetTransformCtrl.MoveLimit(motion);
			//GetTransformCtrl.MoveTo(new Vector3(GetRunTimeData.CurPos.x,GetTransformCtrl.GetFloorHeight,GetRunTimeData.CurPos.z));
			GetRunTimeData.ForceSpeed = Vector3.zero;
			Exit(null);
		}
		else
		{
			GetRunTimeData.ForceSpeed = new Vector3(0,yspeed,0);
		}
	}


//	protected void UpdateAnimation()
//	{
//		if(GetAniCtrl != null)
//		{
//			GetAniCtrl.Play(AnimationNameDef.Hit1, WrapMode.ClampForever,true,1);
//		}
//	}
	
	protected override void Exit (DkBtInputParam input)
	{
		m_curMsg = null;
		m_nextMsg = null;

		base.Exit (input);
	}
	
	protected void CheckSelfRule()
	{
		if(GetRoleBBData.DataInfo.team == eSceneTeamType.Me)
		{
			InputManager.KeyJumpEnalbe = false;
		}
	}
}


