using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Util;

public class RoleCtrlTransform
{
	protected bool m_isInited = false;
		
	protected GameObject m_mainObj;
	
	protected GameObject m_modelObj;
	
	protected RoleBlackBoard m_bbData;

	protected eMoveMethod m_oldMethod = eMoveMethod.Not_Use;

	protected eLookDirection m_curLook = eLookDirection.None;

	protected float m_curXSpeed = 2.5f;
	
	protected float m_bodyHeight = 1.6f;
	
	protected float m_bodyRadius = 0.5f;

	protected float m_curAlpha = 1f;

	protected float m_curScale = 1f;

	protected float m_curGravity = GravityDef.Nomal;
	
	protected CharacterController m_characterController;
	public CharacterController CharController
	{
		get{return m_characterController;}
	}

	protected RoleRootPoint m_rootPoint;
	public RoleRootPoint RootPoint
	{
		get{ return m_rootPoint; }
	}
		
	public void Initalize(RoleBlackBoard bbData)
	{
		m_bbData = bbData;
		setMainObj(m_bbData.PrefabMain);
		setModelObj( m_bbData.PrefabModel);

		if(m_mainObj == null || m_modelObj == null || GetRunTimeData == null || RootPoint == null)
		{
			Debug.Log("[error ]Some thing is null : "+this.m_mainObj+ " | "+this.m_modelObj+" | "+GetRunTimeData + " | "+RootPoint);
			m_isInited = false;
		}
		else 
		{
			m_isInited = true;
			ChangeLook(eLookDirection.Right);
		}
	}

	public void Update()
	{
		UpdateTrigger ();
		UpdateAlpha ();
		UpdateScale ();
		UpdatePos ();
	}

	protected void UpdateTrigger()
	{
		if(GetRunTimeData.IsTrigger && this.CharController.enabled)
		{
			this.CharController.enabled = false;
		}
		else if(GetRunTimeData.IsTrigger == false && this.CharController.enabled == false)
		{
			this.CharController.enabled = true;
		}
	}

	protected void UpdateScale()
	{
		if(m_curLook != this.GetRunTimeData.LookDirection || m_curScale != GetRunTimeData.CurScale)
		{
			m_curLook = this.GetRunTimeData.LookDirection;
			m_curScale = this.GetRunTimeData.CurScale;
			ChangeScale(m_curScale);
		}
	}
	
	protected void UpdateAlpha()
	{
		if(m_curAlpha != GetRunTimeData.CurAlpha)
		{
			m_curAlpha = ChangeAlpha(GetRunTimeData.CurAlpha);
			GetRunTimeData.CurAlpha = m_curAlpha;
		}
	}

	protected void UpdatePos()
	{
		eMoveMethod moveMothod = GetRunTimeData.MoveMethod;
		
		switch(moveMothod)
		{
		case eMoveMethod.Gravity:
			MovoToGround();
			break;
		case eMoveMethod.Direction:
			MoveToDirection();
			break;
		case eMoveMethod.Jump:
			MoveToJump();
			break;
		case eMoveMethod.ForceSpeed:
			MoveToForceSpeed();
			break;
		case eMoveMethod.Motion:
			MoveToMotion();
			break;
		case eMoveMethod.RootPoint:
			MoveToRootPoint();
			break;
		case eMoveMethod.Position:
			//MoveToPosition();
			break;
		case eMoveMethod.Path:
			//MoveToPath();
			break;
		}
		
		GetRunTimeData.CurPos = GetTramsForm.position;
	}

	protected void MovoToGround()
	{
		if(!IsGround)
		{
			ApplyGravity();
			
			Vector3 motion = Vector3.zero;
			motion.y = GetRunTimeData.ForceSpeed.y * Time.deltaTime;
			MoveCollision(motion);
		}
	}
	
	protected void MoveToDirection()
	{
		Vector3 motion = Vector3.zero;

		if(GetRunTimeData.MoveDirection == eMoveDirection.Left)
		{
			motion.x = -m_curXSpeed * Time.deltaTime;
		}
		else if(GetRunTimeData.MoveDirection == eMoveDirection.Right)
		{
			motion.x = m_curXSpeed * Time.deltaTime;
		}

		if(GetRunTimeData.UseGravity == eUseGravity.Yes)
		{
			ApplyGravity();
			motion.y = GetRunTimeData.ForceSpeed.y * Time.deltaTime;
		}

		MoveCollision(motion);
	}

	protected void MoveToJump()
	{
		Vector3 motion = Vector3.zero;
		motion.y = GetRunTimeData.ForceSpeed.y * Time.deltaTime;

		if(GetRunTimeData.MoveDirection == eMoveDirection.Left)
		{
			motion.x = -m_curXSpeed * Time.deltaTime;
		}
		else if(GetRunTimeData.MoveDirection == eMoveDirection.Right)
		{
			motion.x = m_curXSpeed * Time.deltaTime;
		}

		MoveTo(new Vector3(GetRunTimeData.CurPos.x+motion.x,GetRunTimeData.CurPos.y+motion.y,GetRunTimeData.CurPos.z));
		//Move(motion);
	}

	protected void MoveToForceSpeed()
	{
		Vector3 motion = Vector3.zero;
		motion.x = GetRunTimeData.ForceSpeed.x * Time.deltaTime;
		motion.y = GetRunTimeData.ForceSpeed.y * Time.deltaTime;
		motion.z = GetRunTimeData.ForceSpeed.z * Time.deltaTime;

		MoveTo(new Vector3(GetRunTimeData.CurPos.x+motion.x,GetRunTimeData.CurPos.y+motion.y,GetRunTimeData.CurPos.z));
	}
	
	protected void MoveToMotion()
	{
		Vector3 motion = GetRunTimeData.ForceSpeed;
		MoveLimit(motion);
	}
	
	protected void MoveToRootPoint()
	{
		if(GetAniCtrl.GetCurState.time > 0)
		{
			Vector3 motion = Vector3.zero;
			if(GetRunTimeData.LookDirection == eLookDirection.Left)
			{
				motion = RootPoint.UpdateRecord(); 
			}
			else if(GetRunTimeData.LookDirection == eLookDirection.Right)
			{
				motion -= RootPoint.UpdateRecord(); 
			}
			 
			MoveCollision(motion);
		}
	}

	//	
	//	protected void MoveToPosition()
	//	{
	//
	//	}
	//
	//	protected void MoveToPath()
	//	{
	//
	//	}

	protected void MoveCollision( Vector3 motion )
	{
		if(motion.magnitude == 0)
		{
			return;
		}

		if(this.m_characterController != null && this.m_characterController.enabled == true)
		{
			GetRunTimeData.CollisionFlag = m_characterController.Move(motion);
		}
		else
		{
			Debug.Log("========================== [error] id "+ m_bbData.DataInfo.index + " move" + GetRunTimeData.MoveMethod + " ");
		}
	}

	public void MoveFree( Vector3 motion)
	{
		if(motion.magnitude == 0)
		{
			return;
		}

		Vector3 nextPos = new Vector3(GetTramsForm.position.x + motion.x , GetTramsForm.position.y + motion.y ,GetTramsForm.position.z + motion.z );
		MoveTo(nextPos);
	}

	public void MoveLimit( Vector3 motion)
	{
		if(motion.magnitude == 0)
		{
			return;
		}

		Vector3 nextPos = new Vector3(GetTramsForm.position.x + motion.x , GetTramsForm.position.y + motion.y ,GetTramsForm.position.z + motion.z );

		if( nextPos.x < (RoomColliderManager.GetXMin + m_bodyRadius))
		{
			nextPos.x = RoomColliderManager.GetXMin + m_bodyRadius;
		}
		else if( nextPos.x > (RoomColliderManager.GetXMax - m_bodyRadius))
		{
			nextPos.x = RoomColliderManager.GetXMax - m_bodyRadius;
		}

		if( nextPos.y < RoomColliderManager.GetYMin )
		{
			nextPos.y = RoomColliderManager.GetYMin;
		}

		MoveTo(nextPos);
	}
	
	protected void MoveTo(Vector3 pos)
	{
		GetTramsForm.position = pos;
		GetRunTimeData.CurPos = GetTramsForm.position;
	}

	protected void ApplyGravity()
	{
		if (GetRunTimeData !=  null && GetRunTimeData.UseGravity == eUseGravity.Yes)
		{
			if(!IsGround)
			{
				float yspeed = GetRunTimeData.ForceSpeed.y - m_curGravity * Time.deltaTime;
				GetRunTimeData.ForceSpeed = new Vector3(GetRunTimeData.ForceSpeed.x,yspeed,GetRunTimeData.ForceSpeed.z);
			}
			else
			{
				GetRunTimeData.ForceSpeed = new Vector3(GetRunTimeData.ForceSpeed.x,0,GetRunTimeData.ForceSpeed.z);
			}
		}
	}
	
	public void ChangeLook(eLookDirection look)
	{
		if(this.m_isInited)
		{
			this.GetRunTimeData.LookDirection = look;
			UpdateScale();
		}
	}

	private void ChangeScale(float scale)
	{
		if(scale < 0)
		{
			Debug.Log("[ChangeScale] What the hell?!");
			return ;
		}
		
		if(m_curLook == eLookDirection.Left)
		{
			GetTramsForm.localScale = new Vector3(-scale,scale,scale);
		}
		else if(m_curLook == eLookDirection.Right)
		{
			GetTramsForm.localScale = new Vector3(scale,scale,scale);
		}
	}

	public float ChangeAlpha(float alpha)
	{
		if(alpha < 0)
		{
			alpha = 0;
		}
		else if(alpha > 1)
		{
			alpha = 1;
		}

		Renderer[] renders = m_modelObj.GetComponentsInChildren<Renderer>();
		foreach(Renderer render in renders)
		{
			Material[] mats = render.materials;
			foreach(Material item in mats)
			{
				//item.color = new Color(item.color.r,item.color.g,item.color.b,alpha);
				item.SetFloat("_Transparent", alpha);
			}
		}
		return alpha;
	}

	protected void ChangeMoveMethod(eMoveMethod method)
	{
		m_oldMethod = GetRunTimeData.MoveMethod;
		GetRunTimeData.MoveMethod = method;
	}

	public bool IsGround
	{
		get{return GetRunTimeData.IsGround;}
	}

	public float GetFloorHeight
	{
		get{return RoomColliderManager.GetYMin;}
	}

	//====================================================================//

	private void setMainObj(GameObject obj)
	{
		m_mainObj = obj;
		if(m_mainObj != null)
		{
			m_mainObj.transform.position = m_bbData.DataInfo.initPos; //new Vector3(2,StandHeight,0);
			
			if(m_modelObj != null)
			{
				m_modelObj.transform.parent = m_mainObj.transform;
				m_modelObj.transform.localPosition = Vector3.zero;
			}
			
			m_characterController = m_mainObj.AddComponent<CharacterController>();
			m_characterController.slopeLimit = 0;
			m_characterController.stepOffset = 0;
			m_characterController.height = m_bodyHeight;
			m_characterController.radius = m_bodyRadius;;
			m_characterController.center = new Vector3(0f,m_bodyHeight/2,0f);

			BoxCollider box =  m_mainObj.AddComponent<BoxCollider>();
			box.center = new Vector3(0f,m_bodyHeight/2,0f);
			box.size = new Vector3(m_bodyRadius,m_bodyHeight,m_bodyRadius);
			box.isTrigger = true;
		}
	}
	
	private void setModelObj(GameObject obj)
	{
		m_modelObj = obj;
		if(m_modelObj != null)
		{
			if(m_mainObj != null)
			{
				m_modelObj.transform.parent = m_mainObj.transform;
				m_modelObj.transform.localPosition = Vector3.zero;
				//m_modelObj.transform.Rotate(new Vector3(0,180,0));
			}
			
			m_rootPoint = m_modelObj.GetComponentInChildren<RoleRootPoint>();
		}
	}


	protected Transform GetTramsForm
	{
		get
		{
			if(this.m_mainObj != null)
			{
				return this.m_mainObj.transform;
			}
			else return null;
		}
	}
	
	protected RoleDataRunTime GetRunTimeData
	{
		get
		{
			if(m_bbData != null)
			{
				return m_bbData.DataRunTime;
			}
			else return null;
		}
	}
	
	protected RoleCtrlAnimation GetAniCtrl
	{
		get{return m_bbData.CtrlAnimation;}
	}
}
