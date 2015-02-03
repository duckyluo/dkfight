using UnityEngine;
using System;

public class AlphaProcess : IProcess
{
	protected float m_startAlpha = 0;
	protected float m_endAlpha = 0;
	protected float m_curAlpha = 0;
	protected float m_curSpeed = 0;
	protected float m_remainTime = 0;

	protected eProcessStatus m_status = eProcessStatus.None;

	public void Reset(RoleBlackBoard m_selfBB , SkillAlphaChEvent alphaEvent)
	{
		Clean();

		if(alphaEvent.m_startAlpha < 0)
		{
			alphaEvent.m_startAlpha = m_selfBB.DataRunTime.CurAlpha;
		}

		m_startAlpha = alphaEvent.m_startAlpha;
		m_endAlpha = alphaEvent.m_endAlpha;
		m_remainTime = alphaEvent.m_duration;
	}

	public void Start()
	{
		m_curAlpha = m_startAlpha;
		m_curSpeed = (m_endAlpha - m_startAlpha)/m_remainTime;
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
			m_curAlpha += m_curSpeed * TimerManager.Instance.GetDeltaTime;

			if((m_curSpeed > 0 && m_curAlpha >= m_endAlpha) || (m_curSpeed < 0 && m_curAlpha <= m_endAlpha))
			{
				End();
			}
		}
		else if(m_status == eProcessStatus.End)
		{
			m_status = eProcessStatus.None;
		}
	}

	public void End()
	{
		m_curAlpha = m_endAlpha;
		m_status = eProcessStatus.End;
	}
	
	protected void Clean()
	{
		m_status = eProcessStatus.None;

		m_curAlpha = 1f;
		m_startAlpha = 1f;
		m_endAlpha = 1f;
		m_curSpeed = 0f;
	}

	public eProcessStatus GetStatus()
	{
		return m_status;
	}
	
	public bool IsRunning()
	{
		return (m_status != eProcessStatus.None);
	}

	public float GetCurAlpha
	{
		get
		{
			if(m_curAlpha < 0)
			{
				m_curAlpha = 0;
			}
			else if(m_curAlpha > 1)
			{
				m_curAlpha = 1;
			}
			return m_curAlpha;
		}
	}

	public float GetEndAlpha
	{
		get{return m_endAlpha;}
	}
}