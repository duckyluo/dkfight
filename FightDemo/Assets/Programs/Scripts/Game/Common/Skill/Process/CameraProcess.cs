using UnityEngine;
using System;


public class CameraProcess : IProcess
{
	protected float m_duration = 0f;

	protected float m_remainTime = 0f;

	protected RoleBlackBoard m_selfBB = null;

	protected eProcessStatus m_status = eProcessStatus.None;
	
	public void Reset(RoleBlackBoard selfBB, SkillCameraEvent skillCameraEvent)
	{
		m_selfBB = selfBB;
		m_duration = skillCameraEvent.m_duration;
	}

	public void Start()
	{
		if(m_selfBB.DataInfo.team == eSceneTeamType.Me)
		{
			Time.timeScale = 0.2f;
			m_remainTime = m_duration;
			m_status = eProcessStatus.Start;
		}
	}

	public void Stop()
	{
		Time.timeScale = 1f;
		CameraManager.Instance.ChangeCurMode(CameraMode.SceneMode);
		Clean();
	}

	public void Update()
	{
		if(m_status == eProcessStatus.Start)
		{
			m_status = eProcessStatus.Run;
			CameraManager.Instance.ChangeCurMode(CameraMode.PlayerMode);
		}
		else if(m_status == eProcessStatus.Run)
		{
			m_remainTime -= TimerManager.Instance.GetDeltaTime;
			if(m_remainTime <= 0)
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
		Stop();
		m_status = eProcessStatus.End;
	}

	public void Destroy()
	{
		if(m_status == eProcessStatus.Run)
		{
			Stop();
		}
		Clean();
	}

	protected void Clean()
	{
		m_duration = 0;
		m_remainTime = 0;
		m_selfBB = null;

		m_status = eProcessStatus.None;
	}
	
	public bool IsRunning()
	{
		return (m_status != eProcessStatus.None);
	}

	public eProcessStatus GetStatus()
	{
		return m_status;
	}
}