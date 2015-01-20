using UnityEngine;
using System;

public class HitBoundProcess
{
	public RoleBlackBoard m_bbData = null;
	public GameObject m_target = null;
	
	public Vector3 m_localPos = Vector3.zero;
	public Vector3 m_motion = Vector3.zero;
	public float   m_moveTime = 0f;
	public float   m_duration = 0f;
	
	private Vector3 m_direction = Vector3.zero;
	private float   m_Speed = 0f;
	private float   m_timeCount = 0f;

	protected HitBoundRecord m_hitRecord;
	
	public bool m_isDestroyed = false;
	
	protected eProcessStatus m_status = eProcessStatus.None;
	public eProcessStatus Status
	{
		get{return m_status;}
	}
	
	public void Initalize(RoleBlackBoard bbData, SkillHitBoundAddEvent hitBound)
	{
		m_bbData = bbData;
		m_localPos = hitBound.m_localPos;
		m_motion = hitBound.m_motion;
		m_direction = m_motion.normalized;
		m_moveTime = hitBound.m_moveTime;
		m_duration = hitBound.m_duration;
		
		if(m_moveTime > 0)
		{
			m_Speed = m_motion.magnitude/m_moveTime;
		}
		else
		{
			m_localPos += m_motion;
			m_Speed = 0f;
		}

		m_target = new GameObject();
		m_target.name = "HitBound";

		BoxCollider box = m_target.AddComponent<BoxCollider>();
		box.isTrigger = true;
		box.size = hitBound.m_boundSize;
	
		Rigidbody rigidBody = m_target.AddComponent<Rigidbody>();
		rigidBody.isKinematic = true;

		m_hitRecord = m_target.AddComponent<HitBoundRecord>();

		m_target.transform.parent = m_bbData.PrefabMain.transform;
		m_target.transform.localPosition = m_localPos;
		//m_target.transform.localScale = new Vector3(1,1,1);
		

		m_timeCount = 0;
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
			m_timeCount += TimerManager.Instance.GetDeltaTime;
			
			if(m_Speed != 0)
			{
				Vector3 motion = m_direction*m_Speed*TimerManager.Instance.GetDeltaTime;
				m_target.transform.localPosition += motion;
			}
			
			if(m_duration > 0 && m_timeCount > m_duration)
			{
				m_status = eProcessStatus.End;
			}
		}
	}
	
	public void Destroy()
	{
		if(m_isDestroyed == false)
		{
			m_bbData = null;
			GameObject.Destroy(m_target);
			m_status = eProcessStatus.None;
		}
	}
}

public class HitBoundRecord : MonoBehaviour
{
	protected bool m_isHit = false;

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
		m_isHit = true;

		if(other.gameObject.tag == "Enemy")
		{
			RoleDataLocal target = other.gameObject.GetComponent<RoleDataLocal>();

			RoleDataLocal myself = this.gameObject.transform.parent.GetComponent<RoleDataLocal>();
			if(myself != null && myself.GetBBData() != null)
			{
				myself.GetBBData().CtrlSkill.AddHitTarget(target.GetBBData());
			}

//			SceneObjInfo info = localData.GetBBData().DataInfo;
//			Debug.Log("info "+info.index + " "+info.nick);
//
//
//			RoleFsmMessage fsmMsg = new RoleFsmMessage();
//
//			fsmMsg.receiveIndex = info.index;
//			fsmMsg.cmdType = eCommandType.Cmd_Hit;
//			fsmMsg.actionType = eActionType.ForceHit;
//			fsmMsg.lookDirection = localData.GetBBData().DataRunTime.LookDirection;
//			fsmMsg.curPos = localData.GetBBData().DataRunTime.CurPos;
//
//			FsmMsgManager.SendFsmMsg(fsmMsg);
		}
	}
}

