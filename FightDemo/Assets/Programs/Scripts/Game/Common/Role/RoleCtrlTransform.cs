using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Util;

public class RoleCtrlTransform
{
	protected bool m_isInited = false;

	public float m_defaultSpeed = 2.5f;

	protected float m_standHeight = 0.1f;
	public float StandHeight
	{
		get{return m_standHeight;} 
	}
	
	protected GameObject m_mainObj;
	
	protected GameObject m_modelObj;
	
	protected RoleBlackBoard m_bbData;

	protected eMoveMethod m_oldMethod = eMoveMethod.Not_Use;

	protected eLookDirection m_curLook = eLookDirection.None;

	protected float m_curAlpha = 1f;

	protected float m_curScale = 1f;

	protected float m_curGravity = GravityDef.Nomal;
//	public float Gravity
//	{
//		set{m_gravity = value;}
//		get{return m_gravity;}
//	}
	
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
		UpdateAlpha ();
		UpdateScale ();
		UpdatePos ();
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
		case eMoveMethod.Direction:
			MoveToDirection();
			break;
		case eMoveMethod.Jump:
			MoveToJump();
			break;
		case eMoveMethod.Position:
			//MoveToPosition();
			break;
		case eMoveMethod.Path:
			//MoveToPath();
			break;
		case eMoveMethod.RootPoint:
			MoveToRootPoint();
			break;
		case eMoveMethod.Motion:
			MoveToMotion();
			break;
		case eMoveMethod.Gravity:
			MovoToGround();
			break;
		}
		
		GetRunTimeData.CurPos = GetTramsForm.position;
	}
	
	protected void MoveToDirection()
	{
		Vector3 motion = Vector3.zero;

		if(GetRunTimeData.MoveDirection == eMoveDirection.Left)
		{
			motion.x = -m_defaultSpeed * Time.deltaTime;
		}
		else if(GetRunTimeData.MoveDirection == eMoveDirection.Right)
		{
			motion.x = m_defaultSpeed * Time.deltaTime;
		}

		if(GetRunTimeData.UseGravity == eUseGravity.Yes)
		{
			ApplyGravity();
			motion.y = GetRunTimeData.ForceSpeed.y * Time.deltaTime;
		}

		Move(motion);
	}

	protected void MoveToJump()
	{
		Vector3 motion = Vector3.zero;
		motion.y = GetRunTimeData.ForceSpeed.y * Time.deltaTime;

		if(GetRunTimeData.MoveDirection == eMoveDirection.Left)
		{
			motion.x = -m_defaultSpeed * Time.deltaTime;
		}
		else if(GetRunTimeData.MoveDirection == eMoveDirection.Right)
		{
			motion.x = m_defaultSpeed * Time.deltaTime;
		}

		MoveTo(new Vector3(GetRunTimeData.CurPos.x+motion.x,GetRunTimeData.CurPos.y+motion.y,GetRunTimeData.CurPos.z));
		//Move(motion);
	}

//	protected void MoveToSpeed()
//	{
//		Vector3 motion = Vector3.zero;	
//		Move(motion);
//	}
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
			 
			Move(motion);
		}
	}

	protected void MovoToGround()
	{
		if(!IsGround)
		{
			ApplyGravity();

			Vector3 motion = Vector3.zero;
			motion.y = GetRunTimeData.ForceSpeed.y * Time.deltaTime;
			//Debug.Log("2 motion " + motion.ToString() + "  | " +GetRunTimeData.YSpeed);
			Move(motion);
		}
	}
	
	protected void MoveToMotion()
	{
		Vector3 motion = GetRunTimeData.ForceSpeed;
		Move(motion);
	}

	public void Move( Vector3 motion )
	{
		if(motion.magnitude == 0)
		{
			return;
		}

		if(this.m_characterController != null)
		{
			GetRunTimeData.CollisionFlag = m_characterController.Move(motion);
		}
	}

	protected void ApplyGravity()
	{
		if (GetRunTimeData !=  null && GetRunTimeData.UseGravity == eUseGravity.Yes)
		{
			if(!IsGround)
			{
				float yspeed = GetRunTimeData.ForceSpeed.y - m_curGravity * Time.deltaTime;
				//Debug.Log("111111111111   "+yspeed);
				GetRunTimeData.ForceSpeed = new Vector3(GetRunTimeData.ForceSpeed.x,yspeed,GetRunTimeData.ForceSpeed.z);
			}
			else
			{
				GetRunTimeData.ForceSpeed = new Vector3(GetRunTimeData.ForceSpeed.x,0,GetRunTimeData.ForceSpeed.z);
			}
		}
	}

	public bool IsGround
	{
		get{return GetRunTimeData.IsGround;}
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
			m_characterController.height = 1.6f;
			m_characterController.center = new Vector3(0f,0.8f,0f);
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

	public void MoveTo(Vector3 pos)
	{
		GetTramsForm.position = pos;
		GetRunTimeData.CurPos = GetTramsForm.position;
	}
	

}
