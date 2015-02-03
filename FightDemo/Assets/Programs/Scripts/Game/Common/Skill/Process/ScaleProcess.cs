using UnityEngine;
using System;

public class ScaleProcess
{
	protected eProcessStatus m_status = eProcessStatus.None;

	protected RoleBlackBoard m_selfBB = null;
	protected SkillScaleChEvent m_skillScaleEvent = null;

	protected float m_curScale = 1f;
	protected float m_endScale = 1f;
	protected float m_curSpeed = 0f;
	protected float m_remainTime = 0f;

	public void Reset(RoleBlackBoard selfBB , SkillScaleChEvent skillScaleEvent)
	{
		Clean();


		m_selfBB = selfBB;
		m_skillScaleEvent = skillScaleEvent;
	}
	
	public void Start()
	{
		m_curScale = m_skillScaleEvent.m_startScale;
		if(m_curScale < 0)
		{
			m_curScale = m_selfBB.DataRunTime.CurScale;
		}

		m_remainTime = m_skillScaleEvent.m_duration;
		m_endScale = m_skillScaleEvent.m_endScale;
		m_curSpeed = (m_endScale - m_curScale) / m_remainTime;

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
		}
		else if(m_status == eProcessStatus.Run)
		{
			m_remainTime -= TimerManager.Instance.GetDeltaTime;
			m_curScale += m_curSpeed * TimerManager.Instance.GetDeltaTime;
			
			if((m_curSpeed > 0 && m_curScale >= m_endScale) || (m_curSpeed < 0 && m_curScale <= m_endScale))
			{
				End();
			}
		}
		else if(m_status == eProcessStatus.End)
		{
			m_status = eProcessStatus.None;
		}

		//return m_curScale;
	}

	public void End()
	{
		m_curScale = m_endScale;
		m_status = eProcessStatus.End;
	}
	
	protected void Clean()
	{
		m_status = eProcessStatus.None;
		
		m_selfBB = null;
		m_skillScaleEvent = null;
		
		m_curScale = 1f;
		m_endScale = 1f;
		m_curSpeed = 0f;
		m_remainTime = 0f;
	}

	public float GetCurScale
	{
		get{return m_curScale;}
	}
	
	public float GetEndScale
	{
		get{return m_endScale;}
	}

	public eProcessStatus GetStatus()
	{
		return m_status;
	}
	
	public bool IsRunning()
	{
		return (m_status != eProcessStatus.None);
	}
}