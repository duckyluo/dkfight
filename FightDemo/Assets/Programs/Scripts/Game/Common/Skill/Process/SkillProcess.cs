using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Event;

public class SkillProcess : DkEventDispatch
{
	public const string SKILL_EVENT_ENTER = "SkillEventEnter";
	
	protected int m_skillId = 0;

	protected int m_skillIndex = 0;

	protected List<SkillProcessEvent> eventList = new List<SkillProcessEvent>();
	
	protected float m_startTime = 0;

	protected float m_passTime = 0;

	protected float m_finishTime = 0;

	protected eProcessStatus m_status = eProcessStatus.None;
	public eProcessStatus Status
	{
		get{return m_status;}
	}

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
			m_finishTime = skillItem.durationTime;
		}
		else 
		{
			foreach(SkillProcessEvent item in eventList)
			{
				if(item.GetEndTime > m_finishTime)
				{
					m_finishTime = item.GetEndTime;
				}
			}
		}

		if(m_finishTime <= 0)
		{
			Debug.Log("[Warn] What happen ?! skill  "+ m_skillIndex +" finishTime is 0 ");
		}
	}

	public void Start()
	{
		if(m_status == eProcessStatus.Start)
		{
			m_status = eProcessStatus.Run;
			m_startTime = TimerManager.Instance.GetElapsedTime;
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

	public bool IsRunning
	{
		get{return (m_status == eProcessStatus.Run);}
	}

	public void Update()
	{
		if(m_status == eProcessStatus.Run)
		{
			m_passTime = TimerManager.Instance.GetElapsedTime - m_startTime;
			CheckEventList();
			if(m_passTime > m_finishTime)
			{
				m_status = eProcessStatus.End;
				//Debug.Log("[SkillProcess] FinishTime :  "+m_passTime);
			}
		}
	}

	protected void Clean()
	{
		m_status = eProcessStatus.None;

		eventList.Clear();
		m_skillId = 0;
		m_skillIndex = 0;
		m_finishTime = 0;
		m_passTime = 0;
		m_startTime = 0;
	}

	public override void Destroy ()
	{
		base.Destroy ();
		eventList.Clear();
		eventList = null;
	}

	protected void CheckEventList()
	{
		while(eventList.Count > 0)
		{
			SkillProcessEvent item = eventList[0];
			if(item.m_startTime <= m_passTime)
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
