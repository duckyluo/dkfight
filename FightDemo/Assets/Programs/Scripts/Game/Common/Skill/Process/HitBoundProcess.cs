using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HitBoundProcess
{
	protected GameObject m_hitBoundObj = null;
	protected HitBoundRecord m_hitRecord ;

	protected int m_boundIndex = 0;
	
	protected Vector3 m_direction = Vector3.zero;
	protected float   m_Speed = 0f;
	protected float   m_remainTime = 0f;

	protected RoleBlackBoard m_selfBB = null;
	protected SkillHitData m_hitData = null;
		
	protected bool m_isDestroyed = false;
	
	protected eProcessStatus m_status = eProcessStatus.None;
	public eProcessStatus Status
	{
		get{return m_status;}
	}
	
	public void Initalize(RoleBlackBoard bbData, SkillHitBoundAddEvent hitBound)
	{
		m_selfBB = bbData;
		m_boundIndex = hitBound.m_boundIndex;
		m_hitData = hitBound.m_hitData;

		Vector3 m_localPos = hitBound.m_localPos;
		Vector3 m_motion = hitBound.m_motion;

		m_direction = m_motion.normalized;

		if(hitBound.m_moveTime > 0)
		{
			m_Speed = m_motion.magnitude/hitBound.m_moveTime;
		}
		else
		{
			m_localPos += m_motion;
			m_Speed = 0f;
		}

		m_hitBoundObj = new GameObject();
		m_hitBoundObj.name = "HitBound";

		BoxCollider box = m_hitBoundObj.AddComponent<BoxCollider>();
		box.isTrigger = true;
		box.size = hitBound.m_boundSize;
	
		Rigidbody rigidBody = m_hitBoundObj.AddComponent<Rigidbody>();
		rigidBody.isKinematic = true;
		rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
		rigidBody.detectCollisions = true;

		m_hitRecord = m_hitBoundObj.AddComponent<HitBoundRecord>();
		m_hitRecord.Initalize(m_boundIndex,m_hitData,m_selfBB);

		if(hitBound.m_IsLocal)
		{
			m_hitBoundObj.transform.parent = m_selfBB.PrefabMain.transform;
			m_hitBoundObj.transform.localPosition = m_localPos;
		}
		else
		{
			Vector3 pos = m_selfBB.DataRunTime.CurPos + m_localPos;
			if(m_selfBB.DataRunTime.LookDirection == eLookDirection.Right)
			{
				m_hitBoundObj.transform.localPosition = pos;
			}
			else
			{
				pos.x = pos.x - m_localPos.x * 2;
				m_hitBoundObj.transform.localPosition = pos;
			}
		}
		m_remainTime = hitBound.m_duration;
		m_status = eProcessStatus.Start;
	}
	
	public void Update()
	{
		if(m_status == eProcessStatus.Start)
		{
			m_status = eProcessStatus.Run;
		}
		else if(m_status == eProcessStatus.Run)
		{
			m_remainTime -= TimerManager.Instance.GetDeltaTime;
			
			if(m_Speed != 0)
			{
				Vector3 motion = m_direction * m_Speed * TimerManager.Instance.GetDeltaTime;
				m_hitBoundObj.transform.localPosition += motion;
			}
			
			if(m_remainTime <= 0)
			{
				m_status = eProcessStatus.End;
			}
		}
	}
	
	public void Destroy()
	{
		if(m_isDestroyed == false)
		{
			m_isDestroyed = true;
			m_status = eProcessStatus.None;
			m_hitRecord.Destroy();
			GameObject.Destroy(m_hitBoundObj);
			m_selfBB = null;
			m_hitRecord = null;
		}
	}
}

public class HitBoundRecord : MonoBehaviour
{
	protected int m_boundIndex = 0;

	protected RoleBlackBoard m_selfBB = null;

	protected SkillHitData m_hitData = null;

	protected List<RoleBlackBoard> m_list = new List<RoleBlackBoard>();

	public void Initalize(int index , SkillHitData hitData , RoleBlackBoard selfBB)
	{
		m_boundIndex = index;
		m_hitData = hitData;
		m_selfBB = selfBB;
	}

	public void Destroy()
	{
		foreach(RoleBlackBoard target in m_list)
		{
			m_selfBB.CtrlSkill.HitTargetOut(target,m_boundIndex);
		}
		m_list.Clear();
		m_list = null;
		m_hitData = null;
		m_selfBB = null;
	}

	public void OnParticleCollision(GameObject other)
	{
		Debug.Log(this.gameObject+" OnParticleCollision! ");
	}
	
	public void OnCollisionEnter(Collision collisionInfo)
	{
		Debug.Log(this.gameObject+" OnCollisionEnter!");
	}
	
	public void OnTriggerEnter(Collider other) 
	{
		//Debug.Log(this.gameObject+" Role OnTriggerEnter! " + other.gameObject);

		if(other.gameObject.tag == "Enemy")
		{
			//Debug.Log(" ================================= OnTriggerEnter ");

			RoleDataLocal target = other.gameObject.GetComponent<RoleDataLocal>();

			if(m_list.Contains(target.GetBBData()) == false)
			{
				m_list.Add(target.GetBBData());
			}
			
			m_selfBB.CtrlSkill.HitTargetEnter(target.GetBBData(),m_boundIndex,m_hitData);

		}
	}

//	public void OnTriggerStay(Collider other)
//	{
//		if(other.gameObject.tag == "Enemy")
//		{
			//Debug.Log(" ================================= OnTriggerStay ");
//		}
//	}

	public void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			RoleDataLocal target = other.gameObject.GetComponent<RoleDataLocal>();
			m_selfBB.CtrlSkill.HitTargetOut(target.GetBBData(),m_boundIndex);
		}
	}
}

