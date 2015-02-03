using UnityEngine;
using System;

public class PosProcess
{
	protected SkillPosChEvent m_skillPosChEvent = null;
	protected RoleBlackBoard m_selfBB = null;

	protected Vector3 m_startPos = Vector3.zero;
	protected Vector3 m_desPos = Vector3.zero;	
	protected Vector3 m_moveDirection = Vector3.zero;
	protected float m_aSpeed = 0f;
	protected float m_curSpeed = 0f;
	protected float m_moveDis = 0f;

	protected Vector3 m_nextMotion = Vector3.zero;
	protected Vector3 m_totalMotion = Vector3.zero;
	protected float m_remainTime = 0f;

	protected eProcessStatus m_status = eProcessStatus.None;

	public void Reset( RoleBlackBoard selfBB, SkillPosChEvent skillPosEvent)
	{
		Clean();

		m_skillPosChEvent = skillPosEvent;
		m_selfBB = selfBB;
	}
	
	public void Start()
	{
		Vector3 motionDis = Vector3.zero;
		if(m_skillPosChEvent.m_skillMoveMethod == eSkillMoveMethod.Translation)
		{
			motionDis = m_skillPosChEvent.m_motion;
			if(m_selfBB.DataRunTime.LookDirection == eLookDirection.Left)
			{
				m_selfBB.DataRunTime.MoveDirection = eMoveDirection.Left;
				motionDis.x = -motionDis.x;
			}
			else
			{
				m_selfBB.DataRunTime.MoveDirection = eMoveDirection.Right;
			}
		}

		m_remainTime = m_skillPosChEvent.m_duration;
		m_aSpeed = m_skillPosChEvent.m_aSpeed;
		m_startPos = m_selfBB.DataRunTime.CurPos;
		m_desPos = m_startPos + motionDis;
		m_moveDirection = motionDis.normalized;
	
		m_moveDis = Vector3.Distance(m_desPos,m_startPos);
		m_curSpeed = (m_moveDis / m_remainTime) - (m_aSpeed * m_remainTime / 2);
		if(m_curSpeed < 0)
		{
			m_curSpeed = 0;
			m_aSpeed = 2 * m_moveDis / (m_remainTime * m_remainTime);
		}

		m_status = eProcessStatus.Start;
	}

	public void Stop()
	{
		Clean();
	}
	
	public void Destroy()
	{
		Clean();
	}

	public void Update()
	{
		if(m_status == eProcessStatus.Start)
		{
			m_status = eProcessStatus.Run;
			m_nextMotion = Vector3.zero;
		}
		else if(m_status == eProcessStatus.Run)
		{
			m_remainTime -= TimerManager.Instance.GetDeltaTime;
			m_curSpeed += m_aSpeed *TimerManager.Instance.GetDeltaTime;
			float sMove = m_curSpeed * TimerManager.Instance.GetDeltaTime;
			Vector3 nextMotion = m_moveDirection * sMove;
			Vector3 nextTotal = m_totalMotion + nextMotion;
			if(nextTotal.magnitude >= m_moveDis)
			{
				nextMotion = m_desPos - m_startPos - m_totalMotion;
				m_status = eProcessStatus.End;
			}

			m_nextMotion = nextMotion;
			m_totalMotion += m_nextMotion;
		}
		else
		{
			m_status = eProcessStatus.None;
			m_nextMotion = Vector3.zero;
		}
	}

	public void End()
	{

	}

	public eProcessStatus GetStatus()
	{
		return m_status;
	}
	
	public bool IsRunning()
	{
		return (m_status != eProcessStatus.None);
	}

	public Vector3 GetNextMotion
	{
		get{return m_nextMotion; }
	}
	
	public Vector3 GetEndPos
	{
		get{return m_desPos;}
	}

	protected void Clean()
	{
		m_skillPosChEvent = null;
		m_selfBB = null;
		
		m_startPos = Vector3.zero;
		m_desPos = Vector3.zero;	
		m_moveDirection = Vector3.zero;
		m_aSpeed = 0f;
		m_curSpeed = 0f;
		m_moveDis = 0f;
		
		m_nextMotion = Vector3.zero;
		m_totalMotion = Vector3.zero;
		m_remainTime = 0f;

		m_status = eProcessStatus.None;
	}
}

