using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EffectProcess : IProcess
{
	protected RoleBlackBoard m_selfBB = null;
	protected GameObject m_effectPrefab = null;

	protected Vector3 m_localPos = Vector3.zero;
	protected Vector3 m_motion = Vector3.zero;
	protected float   m_moveTime = 0f;
	protected float   m_duration = 0f;

	protected Vector3 m_direction = Vector3.zero;
	protected float   m_speed = 0f;
	protected float   m_remainTime = 0f;

	protected List<ParticleSystem> m_list = new List<ParticleSystem>();
	
	protected bool m_isDestroyed = false;

	protected eProcessStatus m_status = eProcessStatus.None;

	public void Initalize(RoleBlackBoard bbData, SkillEffectAddEvent effectEvent)
	{
		m_selfBB = bbData;
		m_localPos = effectEvent.m_localPos;
		m_motion = effectEvent.m_motion;
		m_direction = m_motion.normalized;
		m_moveTime = effectEvent.m_moveTime;
		m_duration = effectEvent.m_duration;

		if(m_moveTime > 0)
		{
			m_speed = m_motion.magnitude/m_moveTime;
		}
		else
		{
			m_localPos += m_motion;
			m_speed = 0f;
		}

		UnityEngine.Object obj = Resources.LoadAssetAtPath(effectEvent.m_asset,typeof(GameObject));
		m_effectPrefab = GameObject.Instantiate(obj) as GameObject;

//		m_effectPrefab.transform.parent = m_selfBB.PrefabMain.transform; 
//		m_effectPrefab.transform.localScale = new Vector3(1f,1f,1f);
//		m_effectPrefab.transform.localPosition = m_localPos;

		if(effectEvent.m_placeMode == PlaceMode.SelfInside)
		{
			m_effectPrefab.transform.parent = m_selfBB.PrefabMain.transform; 
			m_effectPrefab.transform.localPosition = m_localPos;
			m_effectPrefab.transform.localScale = new Vector3(1f,1f,1f);
		}
		else if(effectEvent.m_placeMode == PlaceMode.SelfOutside)
		{
			Vector3 pos = m_selfBB.DataRunTime.CurPos + m_localPos;
			if(m_selfBB.DataRunTime.LookDirection == eLookDirection.Right)
			{
				m_effectPrefab.transform.localPosition = pos;
			}
			else
			{
				pos.x = pos.x - m_localPos.x * 2;
				m_effectPrefab.transform.localPosition = pos;
			}
		}
		else
		{
			Debug.Log("Not Yet!!!!!!!!!!");
		}
	}

	public void Start()
	{
		m_remainTime = m_duration;
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
			m_remainTime -= TimerManager.Instance.GetDeltaTime;

			if(m_speed != 0)
			{
				Vector3 motion = m_direction * m_speed * TimerManager.Instance.GetDeltaTime;
				m_effectPrefab.transform.localPosition += motion;
			}

			if(m_duration > 0 && m_remainTime <= 0)
			{
				End();
			}
		}
	}

	public void Destroy()
	{
		if(m_isDestroyed == false)
		{
			m_isDestroyed = true;
			GameObject.Destroy(m_effectPrefab);
			m_selfBB = null;
			m_effectPrefab = null;
			m_list.Clear();

			m_status = eProcessStatus.None;
		}
	}

	public void Stop()
	{
		Destroy();
	}

	public void End()
	{
		m_status = eProcessStatus.End;
	}

	protected void Clean()
	{
		m_localPos = Vector3.zero;
		m_motion = Vector3.zero;
		m_moveTime = 0f;
		m_duration = 0f;
		
		m_direction = Vector3.zero;
		m_speed = 0f;
		m_remainTime = 0f;
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
