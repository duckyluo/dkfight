using UnityEngine;
using System;


public class AlphaProcess
{
	public float m_startAlpha;
	public float m_endAlpha;
	public float m_curAlpha;
	public float m_duration;
	public float m_curSpeed;
	public float m_timeCount;

	protected eProcessStatus m_status = eProcessStatus.None;
	public eProcessStatus Status
	{
		get{return m_status;}
	}

	public void Restart(float startAlpha , float desAlpha , float duration)
	{
		Clean();

		m_startAlpha = startAlpha;
		m_endAlpha = desAlpha;
		m_curAlpha = m_startAlpha;
		m_duration = duration;
		m_curSpeed = (desAlpha - startAlpha)/duration;
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

	public float UpdateAlpha()
	{
		if(m_status == eProcessStatus.Start)
		{
			m_status = eProcessStatus.Run;
			m_curAlpha = m_startAlpha;
		
		}
		else if(m_status == eProcessStatus.Run)
		{
			m_timeCount += TimerManager.Instance.GetDeltaTime;
			m_curAlpha += m_curSpeed * TimerManager.Instance.GetDeltaTime;

			if((m_curSpeed > 0 && m_curAlpha >= m_endAlpha) || (m_curSpeed < 0 && m_curAlpha <= m_endAlpha))
			{
				m_curAlpha = m_endAlpha;
				m_status = eProcessStatus.End;
			}
		}
		else if(m_status == eProcessStatus.End)
		{
			m_status = eProcessStatus.None;
		}

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

	protected void Clean()
	{
		m_status = eProcessStatus.None;

		m_curAlpha = 1f;
		m_startAlpha = 1f;
		m_endAlpha = 1f;
		m_duration = 0f;
		m_curSpeed = 0f;
		m_timeCount = 0f;
	}

	public bool IsRunning
	{
		get{ return (m_status != eProcessStatus.None);}
	}

	public float GetCurAlpha
	{
		get{return m_curAlpha;}
	}

	public float GetEndAlpha
	{
		get{return m_endAlpha;}
	}
}