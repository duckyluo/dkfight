using UnityEngine;
using System;

public class PosProcess
{
	protected Vector3 m_desPos = Vector3.zero;	
	protected Vector3 m_startPos = Vector3.zero;
	protected float m_duration = 0f;
	protected float m_aSpeed = 0f;
	
	protected Vector3 m_moveDirection = Vector3.zero;
	protected float m_curSpeed = 0f;
	protected float m_moveDis = 0f;
	
	protected float m_timeCount = 0f;
	protected Vector3 m_totalMotion = Vector3.zero;
	
	protected Vector3 m_nextMotion = Vector3.zero;
	public Vector3 GetNextMotion
	{
		get{return m_nextMotion; }
	}
	
	protected eProcessStatus m_status = eProcessStatus.None;
	public eProcessStatus Status
	{
		get{return m_status;}
	}

	public Vector3 GetEndPos
	{
		get{return m_desPos;}
	}
	
	public void Restart( Vector3 startPos , Vector3 motionDis , float duration , float aSpeed)
	{
		Clean();

		m_startPos = startPos;
		m_desPos = m_startPos + motionDis;
		m_moveDirection = motionDis.normalized;
		m_duration = duration;
		m_aSpeed = aSpeed;
		m_moveDis = Vector3.Distance(m_desPos,m_startPos);
		m_curSpeed = (m_moveDis/m_duration) - (m_aSpeed*m_duration/2);
		if(m_curSpeed < 0)
		{
			m_curSpeed = 0;
			m_aSpeed = 2*m_moveDis/(m_duration*m_duration);
		}
		
		//Debug.Log("[move]Start : curSpeed "+m_curSpeed + " aSpeed "+m_aSpeed);
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

	public Vector3 UpdateMotion()
	{
		if(m_status == eProcessStatus.Start)
		{
			m_status = eProcessStatus.Run;
			return Vector3.zero;
		}
		else if(m_status == eProcessStatus.Run)
		{
			m_timeCount += TimerManager.Instance.GetDeltaTime;//Time.deltaTime;//
			m_curSpeed += m_aSpeed *TimerManager.Instance.GetDeltaTime;// Time.deltaTime;//
			float sMove = m_curSpeed * TimerManager.Instance.GetDeltaTime;//Time.deltaTime;//
			Vector3 nextMotion = m_moveDirection * sMove;
			Vector3 nextTotal = m_totalMotion + nextMotion;
			if(nextTotal.magnitude >= m_moveDis)
			{
				nextMotion = m_desPos - m_startPos - m_totalMotion;
				m_status = eProcessStatus.End;

				//Debug.Log("[move]Finish : TimeOut "+(m_timeCount - m_duration));
			}

			m_nextMotion = nextMotion;
			m_totalMotion += m_nextMotion;
			return m_nextMotion;
		}
		else
		{
			m_status = eProcessStatus.None;
			m_nextMotion = Vector3.zero;
			return m_nextMotion;
		}
	}

	public bool IsRunning
	{
		get{ return (m_status != eProcessStatus.None);}
	}
	
	protected void Clean()
	{
		m_status = eProcessStatus.None;

		m_desPos = Vector3.zero;
		m_startPos = Vector3.zero;
		m_moveDirection = Vector3.zero;
		m_aSpeed = 0f;
		m_curSpeed = 0f;
		m_moveDis = 0f;
		m_duration = 0f;
		m_timeCount = 0f;
		m_totalMotion = Vector3.zero;
		m_nextMotion = Vector3.zero;
	}
}

