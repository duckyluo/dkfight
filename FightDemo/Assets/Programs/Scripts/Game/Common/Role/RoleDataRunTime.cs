﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Util;

public class RoleDataRunTime 
{
	protected eUseGravity m_useGravity = eUseGravity.No;
	public eUseGravity UseGravity
	{
		set{m_useGravity = value;}
		get{return m_useGravity;}
	}

	protected bool m_isTrigger = false;
	public bool IsTrigger
	{
		set{m_isTrigger = value;}
		get{return m_isTrigger;}
	}

	protected bool m_isShake = false;
	public bool IsShake
	{
		set{m_isShake = value;}
		get{return m_isShake;}
	}
	
	protected Vector3 m_forceSpeed = new Vector3();
	public Vector3 ForceSpeed
	{
		set{m_forceSpeed = value;}
		get{return m_forceSpeed;}
	}
	
	protected Vector3 m_curPos = Vector3.zero;
	public Vector3 CurPos
	{
		set{ m_curPos = value;}
		get{ return m_curPos; }
	}

	protected float m_curAlpha = 1f;
	public float CurAlpha
	{
		set{ m_curAlpha = value;}
		get{ return m_curAlpha; }
	}

	protected float m_curScale = 1f;
	public float CurScale
	{
		set{ m_curScale = value;}
		get{ return m_curScale; }
	}

	protected Vector3 m_curRotation = Vector3.zero;
	public Vector3 CurRotation
	{
		set{ m_curRotation = value;}
		get{ return m_curRotation; }
	}

//	protected Vector3 m_logicPos = Vector3.zero;
//	public Vector3 LogicPos
//	{
//		set{ m_logicPos = value;}
//		get{ return m_logicPos; }
//	}
	
	protected eLookDirection m_lookDirection = eLookDirection.Not_Use;
	public eLookDirection LookDirection
	{
		set{ m_lookDirection = value;}
		get{ return m_lookDirection; }
	}

	protected eMoveDirection m_moveDirection = eMoveDirection.Not_Use;
	public eMoveDirection MoveDirection
	{
		set{ m_moveDirection = value;}
		get{ return m_moveDirection; }
	}

	protected eMoveMethod m_moveMethod = eMoveMethod.Not_Use;
	public eMoveMethod MoveMethod
	{
		set{ m_moveMethod = value;}
		get{ return m_moveMethod; }
	}

	protected ePostureType m_posture = ePostureType.Not_Use;
	public ePostureType PostureType
	{
		set{ m_posture = value;}
		get{ return m_posture; }
	}

	protected eStateType m_state = eStateType.Not_Use;
	public eStateType StateType
	{
		set{ m_state = value;}
		get{ return m_state; }
	}

	protected eActionType m_action = eActionType.Not_Use;
	public eActionType ActionType
	{
		set{ m_action = value;}
		get{ return m_action; }
	}

	protected CollisionFlags m_collisionFlag = CollisionFlags.None;
	public CollisionFlags CollisionFlag
	{
		set{m_collisionFlag = value;}
		get{return m_collisionFlag;}
	}
	
	protected bool m_moveEnable = false;
	public bool MoveEnable
	{
		set{m_moveEnable = value;}
		get{return m_moveEnable;}
	}

	protected bool m_activeChStateEnable = false; //enable change state actively
	public bool ActiveChStateEnalbe
	{
		set{m_activeChStateEnable = value;}
		get{return m_activeChStateEnable;}
	}

	protected bool m_passiveChStateEnalbe = false; //enable other change target state
	public bool PassiveChStateEnalbe
	{
		set{m_passiveChStateEnalbe = value;}
		get{return m_passiveChStateEnalbe;}
	}

	protected bool m_isPaused = false;
	public bool IsPaused
	{
		get{return m_isPaused;}
	}

	public void Pause()
	{
		m_isPaused = true;
	}

	public void Resume()
	{
		m_isPaused = false;
	}
	
	public bool IsGround
	{
		get{return (CollisionFlags.CollidedBelow & CollisionFlag) != 0;}
	}

	public bool JumpAtkEnable
	{
		get{ return (CurPos.y >= RoleHeightDef.JumpAtkHeight);}
	}

	public bool IsOnAir
	{
		get
		{
			return !IsGround;
//			if(this.m_posture == ePostureType.Pose_JumpUp ||
//			   this.m_posture == ePostureType.Pose_JumpFloat ||
//			   this.m_posture == ePostureType.Pose_JumpDown ||
//			   this.m_posture == ePostureType.Pose_HitFlyUp ||
//			   this.m_posture == ePostureType.Pose_HitFlyFloat ||
//			   this.m_posture == ePostureType.Pose_HitFlyDown ||
//			   this.m_posture == ePostureType.Pose_JumpAttack)
//			{
//				return true;
//			}
//			else return false;
		}
	}
	
	public eLookDirection GetLookDirectionToPos(Vector3 pos)
	{
		eLookDirection look = LookDirection;
		if(pos.x > CurPos.x)
		{
			look = eLookDirection.Right;
		}
		else if(pos.x < CurPos.x)
		{
			look = eLookDirection.Left;
		}
		return look;
	}
	
	public override string ToString ()
	{
		string str = "";
		str += "[StateType] "+StateType + " , ";
		str += "[MoveMethod] "+MoveMethod + " , ";
		str += "[PostureType] "+PostureType + " , ";
		str += "[LookDirection] "+LookDirection + " , ";
		str += "[MoveDirection] "+MoveDirection + " , ";

		str += "[UseGravity] "+UseGravity + " , ";
		str += "[CurPos] "+CurPos + " , ";
		str += "[ForceSpeed] "+ForceSpeed + " , ";
		str += "[MoveEnable] "+MoveEnable + " , ";
		str += "[CollisionFlags] "+CollisionFlag + " , ";
		str += "[IsPaused] "+IsPaused + " , ";

		return str;
	}

}
