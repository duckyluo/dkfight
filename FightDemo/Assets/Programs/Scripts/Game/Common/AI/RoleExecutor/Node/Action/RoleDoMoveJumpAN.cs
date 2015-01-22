using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleDoMoveJumpAN : RoleBaseActionNode
{
	protected TMoveMessage m_curMsg = null;
	
	protected TimeLineMessage m_nextMsg = null;

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
		CheckSelfRule();

		GetRunTimeData.ActionType = eActionType.None;
		GetRunTimeData.MoveMethod = eMoveMethod.None;
		GetRunTimeData.MoveDirection = eMoveDirection.None;
		//GetRunTimeData.LookDirection = eLookDirection.None;
		GetRunTimeData.PostureType = ePostureType.Pose_None;

		GetRunTimeData.MoveEnable = false;
		GetRunTimeData.ActiveChStateEnalbe = true;
		GetRunTimeData.PassiveChStateEnalbe = true;
		GetRunTimeData.UseGravity = eUseGravity.No;
		GetRunTimeData.IsTrigger = true;

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
		if(m_nextMsg != null)
		{
			Exit(null);
		}
		else if(GetRunTimeData.ActionType == eActionType.None)
		{
			StartJump();
		}
		else
		{
			DoJumping();
		}
	}

	private float m_jumpFloatTime = 0.3f; //浮空时间
	private float m_jumpUpTime = 0.3f; //上升时间
	private float m_jumpDownTime = 0.3f; //下降时间
	private float m_jumpHeight = 1.6f; //上升高度
	private float m_jumpUpAV = 0f; //上升加速度
	private float m_jumpDownAV = 0f; //下降加速度
	private float m_jumpFloatLastTime = 0f; //浮空剩余时间

	private void StartJump()
	{
		GetRunTimeData.ActionType = eActionType.Jump;
		GetRunTimeData.MoveMethod = eMoveMethod.Jump;
		GetRunTimeData.MoveDirection = m_curMsg.moveDirection;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
		GetRunTimeData.PostureType = ePostureType.Pose_JumpUp;
		GetRunTimeData.CollisionFlag = CollisionFlags.None;
		
		m_jumpFloatLastTime = m_jumpFloatTime;
		m_jumpUpAV = -m_jumpHeight/(m_jumpUpTime*m_jumpUpTime);
		m_jumpDownAV = m_jumpHeight/(m_jumpDownTime*m_jumpDownTime);
		
		float startSpeed = m_jumpHeight*2/m_jumpUpTime;
		
		GetRunTimeData.ForceSpeed = new Vector3(0f,startSpeed,0f);

		UpdateAnimation();
	}
	
	private void DoJumping()
	{
		switch(GetRunTimeData.PostureType)
		{
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

	private void JumpUp()
	{
		GetRunTimeData.ActiveChStateEnalbe = true;

		float yspeed = GetRunTimeData.ForceSpeed.y;
		float oldSpeed = yspeed;
		yspeed -= m_jumpUpAV*Time.deltaTime;
		
		if(yspeed <= 0)
		{
			yspeed = oldSpeed;
			Debug.Log("[warn][jump speed] "+m_jumpUpAV +"  ==== "+yspeed+" ==== "+Time.deltaTime);
		}
		
		float height = GetRunTimeData.CurPos.y + yspeed*Time.deltaTime;
		if(height >= m_jumpHeight )
		{

			Vector3 motion = new Vector3(0,m_jumpHeight - GetRunTimeData.CurPos.y,0);
			GetTransformCtrl.MoveLimit(motion);
			//GetTransformCtrl.MoveTo(new Vector3(GetRunTimeData.CurPos.x, m_jumpHeight , GetRunTimeData.CurPos.z));
			GetRunTimeData.PostureType = ePostureType.Pose_JumpFloat;
			yspeed = 0;
		}

		GetRunTimeData.ForceSpeed = new Vector3(GetRunTimeData.ForceSpeed.x,yspeed,GetRunTimeData.ForceSpeed.z);
	}

	private void JumpFloat()
	{
		GetRunTimeData.ActiveChStateEnalbe = true;

		m_jumpFloatLastTime -= Time.deltaTime;
		if(m_jumpFloatLastTime <= 0)
		{
			GetRunTimeData.PostureType = ePostureType.Pose_JumpDown;
		}

		GetRunTimeData.ForceSpeed = new Vector3(GetRunTimeData.ForceSpeed.x,0,GetRunTimeData.ForceSpeed.z);
	}

	private void JumpDown()
	{
		GetRunTimeData.ActiveChStateEnalbe = false;

		float yspeed =  GetRunTimeData.ForceSpeed.y;
		yspeed -= m_jumpDownAV*Time.deltaTime;
		float nextYPos = ( GetRunTimeData.CurPos.y + 0.8f ) + yspeed*Time.deltaTime;//to fix resource ! shit	
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
			GetRunTimeData.ForceSpeed = new Vector3(GetRunTimeData.ForceSpeed.x,yspeed,GetRunTimeData.ForceSpeed.z);
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
