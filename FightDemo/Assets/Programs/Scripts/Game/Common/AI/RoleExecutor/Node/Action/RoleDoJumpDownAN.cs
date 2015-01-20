using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleDoJumpDownAN : RoleBaseActionNode
{
	protected TMoveMessage m_curMsg = null;
	
	protected TimeLineMessage m_nextMsg = null;
	
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
		InputManager.KeyJumpEnalbe = false;
		
		GetRunTimeData.ActionType = eActionType.JumpDown;
		GetRunTimeData.MoveMethod = eMoveMethod.None;
		GetRunTimeData.MoveDirection = eMoveDirection.None;
		//GetRunTimeData.LookDirection = eLookDirection.None;
		GetRunTimeData.PostureType = ePostureType.Pose_None;
		
		GetRunTimeData.MoveEnable = false;
		GetRunTimeData.ActiveChStateEnalbe = false;
		GetRunTimeData.PassiveChStateEnalbe = false;
		GetRunTimeData.UseGravity = eUseGravity.No;

		GetRunTimeData.ForceSpeed = Vector3.zero;
		GetRunTimeData.CurAlpha = 1f;
		GetRunTimeData.CurScale = 1f;
		
		m_curMsg = GetFrontWaitMsg as TMoveMessage;
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
		if(GetRunTimeData.MoveMethod == eMoveMethod.None)
		{
			StartJump();
		}
		else
		{
			if(m_nextMsg != null && m_nextMsg.GetActionType == eActionType.JumpAttack)
			{
				Exit(null);
			}
			else
			{
				DoJumping();
			}
		}
	}
	
	private float m_jumpFloatTime = 0.5f; //浮空时间
	private float m_jumpFloatLastTime = 0f; //浮空剩余时间
	private float m_jumpDownAV = 30f; //下降加速度
	
	private void StartJump()
	{
		GetRunTimeData.MoveMethod = eMoveMethod.Jump;
		GetRunTimeData.PostureType = ePostureType.Pose_JumpFloat;
		GetRunTimeData.CollisionFlag = CollisionFlags.None;
		
		m_jumpFloatLastTime = m_jumpFloatTime;
		
		UpdateAnimation();
	}
	
	private void DoJumping()
	{
		switch(GetRunTimeData.PostureType)
		{
//		case ePostureType.Pose_JumpUp:
//			JumpUp();
//			break;
		case ePostureType.Pose_JumpFloat:
			JumpFloat();
			break;
		case ePostureType.Pose_JumpDown:
			JumpDown();
			break;
		}
	}
	
//	private void JumpUp()
//	{
//		GetRunTimeData.ActiveChStateEnalbe = true;
//		
//		float yspeed = GetRunTimeData.ForceSpeed.y;
//		float oldSpeed = yspeed;
//		yspeed -= m_jumpUpAV*Time.deltaTime;
//		
//		if(yspeed <= 0)
//		{
//			yspeed = oldSpeed;
//			Debug.Log("[warn][jump speed] "+m_jumpUpAV +"  ==== "+yspeed+" ==== "+Time.deltaTime);
//		}
//		
//		float height = GetRunTimeData.CurPos.y + yspeed*Time.deltaTime;
//		if(height >= m_jumpHeight )
//		{
//			
//			GetTransformCtrl.MoveTo(new Vector3(GetRunTimeData.CurPos.x, m_jumpHeight , GetRunTimeData.CurPos.z));
//			GetRunTimeData.PostureType = ePostureType.Pose_JumpFloat;
//			yspeed = 0;
//		}
//		
//		GetRunTimeData.ForceSpeed = new Vector3(GetRunTimeData.ForceSpeed.x,yspeed,GetRunTimeData.ForceSpeed.z);
//	}
	
	private void JumpFloat()
	{
		GetRunTimeData.ActiveChStateEnalbe = true;
		
		m_jumpFloatLastTime -= Time.deltaTime;
		if(m_jumpFloatLastTime <= 0)
		{
			GetRunTimeData.PostureType = ePostureType.Pose_JumpDown;
		}
		
		GetRunTimeData.ForceSpeed = new Vector3(0,0,0);
	}
	
	private void JumpDown()
	{
		GetRunTimeData.ActiveChStateEnalbe = false;
		
		float yspeed =  GetRunTimeData.ForceSpeed.y;
		yspeed -= m_jumpDownAV * TimerManager.Instance.GetDeltaTime;//Time.deltaTime;
		float nextYPos = ( GetRunTimeData.CurPos.y + 0.8f  ) + yspeed * TimerManager.Instance.GetDeltaTime;//Time.deltaTime;//to fix resource ! shit	
		if(nextYPos <= GetTransformCtrl.StandHeight)
		{
			GetTransformCtrl.MoveTo(new Vector3(GetRunTimeData.CurPos.x,GetTransformCtrl.StandHeight,GetRunTimeData.CurPos.z));
			GetRunTimeData.ForceSpeed = Vector3.zero;
			Exit(null);
		}
		else
		{
			GetRunTimeData.ForceSpeed = new Vector3(0,yspeed,0);
			//Debug.Log("Down !!! "+yspeed);
		}
	}
	
	protected void UpdateAnimation()
	{
		if(GetAniCtrl != null)
		{
			GetAniCtrl.Play(AnimationNameDef.Jump,WrapMode.ClampForever,false,1f);
			//GetAniCtrl.SampleCurAtTime(GetAniCtrl.GetCurState.length);
		}
	}
	
	protected override void Exit (DkBtInputParam input)
	{
		m_curMsg = null;
		m_nextMsg = null;
		
		base.Exit (input);
	}
}



