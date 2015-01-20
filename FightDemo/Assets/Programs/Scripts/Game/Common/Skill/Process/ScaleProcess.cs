using UnityEngine;
using System;

public class ScaleProcess
{
	public float m_startScale;
	public float m_endScale;
	public float m_curScale;
	public float m_duration;
	public float m_curSpeed;
	public float m_timeCount;
	
	protected eProcessStatus m_status = eProcessStatus.None;
	public eProcessStatus Status
	{
		get{return m_status;}
	}
	
	public void Restart(float startScale , float desScale , float duration)
	{
		Clean();
		
		m_startScale = startScale;
		m_endScale = desScale;
		m_curScale = m_startScale;
		m_duration = duration;
		m_curSpeed = (m_endScale - m_startScale)/m_duration;
		m_timeCount = 0f;
		
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
	
	public float UpdateScale()
	{
		if(m_status == eProcessStatus.Start)
		{
			m_status = eProcessStatus.Run;
			m_curScale = m_startScale;
			
		}
		else if(m_status == eProcessStatus.Run)
		{
			m_timeCount += TimerManager.Instance.GetDeltaTime;
			m_curScale += m_curSpeed * TimerManager.Instance.GetDeltaTime;
			
			if((m_curSpeed > 0 && m_curScale >= m_endScale) || (m_curSpeed < 0 && m_curScale <= m_endScale))
			{
				m_curScale = m_endScale;
				m_status = eProcessStatus.End;
			}
		}
		else if(m_status == eProcessStatus.End)
		{
			m_status = eProcessStatus.None;
		}

		return m_curScale;
	}
	
	public void Clean()
	{
		m_status = eProcessStatus.None;
		
		m_curScale = 1f;
		m_startScale = 1f;
		m_endScale = 1f;
		m_duration = 0f;
		m_curSpeed = 0f;
		m_timeCount = 0f;
	}
	
	public bool IsRunning
	{
		get{ return (m_status != eProcessStatus.None);}
	}
	
	public float GetCurScale
	{
		get{return m_curScale;}
	}
	
	public float GetEndScale
	{
		get{return m_endScale;}
	}
}