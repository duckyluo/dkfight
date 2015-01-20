using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EffectProcess
{
	public RoleBlackBoard m_bbData = null;
	public GameObject m_target = null;

	public Vector3 m_localPos = Vector3.zero;
	public Vector3 m_motion = Vector3.zero;
	public float   m_moveTime = 0f;
	public float   m_duration = 0f;

	private Vector3 m_direction = Vector3.zero;
	private float   m_Speed = 0f;
	private float   m_timeCount = 0f;

	private List<ParticleSystem> m_list = new List<ParticleSystem>();
	
	public bool m_isDestroyed = false;

	protected eProcessStatus m_status = eProcessStatus.None;
	public eProcessStatus Status
	{
		get{return m_status;}
	}

	public void Initalize(RoleBlackBoard bbData, SkillEffectAddEvent effect)
	{
		m_bbData = bbData;
		m_localPos = effect.m_localPos;
		m_motion = effect.m_motion;
		m_direction = m_motion.normalized;
		m_moveTime = effect.m_moveTime;
		m_duration = effect.m_duration;

		if(m_moveTime > 0)
		{
			m_Speed = m_motion.magnitude/m_moveTime;
		}
		else
		{
			m_localPos += m_motion;
			m_Speed = 0f;
		}

//		GameObject gameobject = new GameObject();
//		gameobject.name = "gua";
//		gameobject.transform.parent = m_bbData.PrefabMain.transform;
//		gameobject.transform.localPosition = Vector3.zero;
		//gameobject.transform.localScale = new Vector3(1f,1f,1f);

		UnityEngine.Object obj = Resources.LoadAssetAtPath(effect.m_asset,typeof(GameObject));
		m_target = GameObject.Instantiate(obj) as GameObject;

		m_target.transform.parent = m_bbData.PrefabMain.transform; //gameobject.transform;
		m_target.transform.localScale = new Vector3(1f,1f,1f);
		m_target.transform.localPosition = m_localPos;

//
//
//		if(m_bbData.PrefabMain.transform.localScale.x < 0)
//		{
//			Debug.Log(" ================ ");

//			ParticleSystem[] particles = m_target.GetComponentsInChildren<ParticleSystem>();
//			foreach(ParticleSystem item in particles)
//			{
//				item.startRotation = 180f; 
//					//m_list.Add(item);
//			}
//			m_target.transform.localScale = new Vector3(-1f,1f,1f);
//		}
//		else
//		{
//			Debug.Log("=============  "+m_bbData.PrefabMain.transform.localScale);
//		}

		//m_target.transform.localScale = new Vector3(1,1,1);

		m_timeCount = 0;
		m_status = eProcessStatus.Start;
	}

	public void Update()
	{
		if(m_status == eProcessStatus.Start)
		{
			m_status = eProcessStatus.Run;

//			foreach(ParticleSystem item in m_list)
//			{
//				item.Play();
//			}
		}
		else if(m_status == eProcessStatus.Run)
		{
			m_timeCount += TimerManager.Instance.GetDeltaTime;

			if(m_Speed != 0)
			{
				Vector3 motion = m_direction*m_Speed*TimerManager.Instance.GetDeltaTime;
				m_target.transform.localPosition += motion;
			}

			if(m_duration > 0 && m_timeCount > m_duration)
			{
				m_status = eProcessStatus.End;
			}
		}
	}

	public void Destroy()
	{
		if(m_isDestroyed == false)
		{
			m_bbData = null;
			GameObject.Destroy(m_target);
			m_status = eProcessStatus.None;
			m_list.Clear();
		}
	}
}
