using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Event;

public class SkillProcess : DkEventDispatch , IProcess
{
	public const string SKILL_EVENT_ENTER = "SkillEventEnter";
	
	protected int m_skillId = 0;

	protected int m_skillIndex = 0;

	protected List<SkillProcessEvent> eventList = new List<SkillProcessEvent>();

	protected float m_duration = 0;

	protected float m_remainTime = 0f;
	
	protected eProcessStatus m_status = eProcessStatus.None;
	
	public void Reset(CRoleSkillItem skillItem)
	{
		Clean();

		m_status = eProcessStatus.Start;

		m_skillId = skillItem.skillId;
		m_skillIndex = skillItem.skillIndex;
		eventList.AddRange(skillItem.skillEvents);
		eventList.Sort();

		if(skillItem.durationTime > 0)
		{
			m_duration = skillItem.durationTime;
		}
		else 
		{
			foreach(SkillProcessEvent item in eventList)
			{
				if(item.GetEndTime > m_duration)
				{
					m_duration = item.GetEndTime;
				}
			}
		}

		if(m_duration <= 0)
		{
			Debug.LogError("[Warn] What happen ?! skill  "+ m_skillIndex +" finishTime is 0 ");
		}
	}

	public void Start()
	{
		if(m_status == eProcessStatus.Start)
		{
			m_remainTime = m_duration;
			m_status = eProcessStatus.Run;
		}
		else
		{
			Debug.LogError("something error");
		}
	}

	public void Stop()
	{
		Clean();
	}
	
	public void Update()
	{
		if(m_status == eProcessStatus.Run)
		{
			m_remainTime -= TimerManager.Instance.GetDeltaTime;
			CheckEventList();
			if(m_remainTime <= 0)
			{
				End();
			}
		}
	}

	public void End()
	{
		m_remainTime = 0;
		m_status = eProcessStatus.End;
	}
	
	protected void Clean()
	{
		m_status = eProcessStatus.None;

		eventList.Clear();
		m_skillId = 0;
		m_skillIndex = 0;
		m_remainTime = 0;
		m_duration = 0;
	}

	public override void Destroy ()
	{
		base.Destroy ();
		eventList.Clear();
		eventList = null;
	}
	
	public float GetElapseTime
	{
		get{return m_duration - m_remainTime;}
	}

	public eProcessStatus GetStatus()
	{
		return m_status;
	}

	public bool IsRunning()
	{
		return (m_status == eProcessStatus.Run);
	}
	
	protected void CheckEventList()
	{
		while(eventList.Count > 0)
		{
			SkillProcessEvent item = eventList[0];
			if(item.m_startTime <= GetElapseTime)
			{
				eventList.RemoveAt(0);
				this.DispatchEvent(new DkEvent(SkillProcess.SKILL_EVENT_ENTER,item));
			}
			else
			{
				break;
			}
		}
	}
}
