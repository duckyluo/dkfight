using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EffectProcess : IProcess
{
	protected EffectMethod m_method = null;

	protected SkillEffectAddEvent m_effectEvent = null;

	protected bool m_isDestroyed = false;

	protected eProcessStatus m_status = eProcessStatus.None;

	public void Initalize(RoleBlackBoard bbData, SkillEffectAddEvent effectEvent)
	{
		m_effectEvent = effectEvent;
		m_method = EffectMethod.MakeProcessBy(effectEvent.m_effectMethod);
		m_method.Initalize(bbData,effectEvent);
	}

	public void Start()
	{
		m_status = eProcessStatus.Run;
		m_method.Start();

		if(m_effectEvent.m_sound != SoundDef.Not_Use)
		{
			SoundManager.PlaySound(m_effectEvent.m_sound);
		}
	}
	
	public void Update()
	{
		if(m_status == eProcessStatus.Run && m_method.IsRunning())
		{
			m_method.Update();
		}
		else
		{
			m_status = eProcessStatus.None;
		}
	
	}

	public void Destroy()
	{
		if(m_isDestroyed == false)
		{
			m_isDestroyed = true;
			m_method.Destroy();
			m_method = null;
			m_effectEvent = null;
			m_status = eProcessStatus.None;
		}
	}

	public void Stop()
	{
		Destroy();
	}

	public void End()
	{
		m_status = eProcessStatus.None;
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


public class EffectMethod : IProcess
{
	protected eProcessStatus m_status = eProcessStatus.None;

	protected RoleBlackBoard m_selfBB;
	protected GameObject m_effectPrefab;
	protected SkillEffectAddEvent m_effectEvent;

	public virtual void Start(){}
	public virtual void Stop(){}
	public virtual void Update(){}
	public virtual void End(){}

	public virtual void Destroy()
	{
		m_effectEvent = null;
		m_selfBB = null;
		GameObject.Destroy(m_effectPrefab);
	}
	
	public void Initalize(RoleBlackBoard selfBB  , SkillEffectAddEvent effectEvent)
	{
		m_selfBB = selfBB;
		m_effectEvent = effectEvent;

		UnityEngine.Object obj = Resources.LoadAssetAtPath(effectEvent.m_asset,typeof(GameObject));
		m_effectPrefab = GameObject.Instantiate(obj) as GameObject;
	}

	public static EffectMethod MakeProcessBy(eSkillEffectMethod method)
	{
		EffectMethod effectMethod = null;
		if(method == eSkillEffectMethod.Link)
		{
			effectMethod = new EffectLinkProcess();
		}
		else 
		{
			effectMethod = new EffectMoveProcess();
		}
		return effectMethod;
	}

	protected void InitPos(Vector3 localPos)
	{
		if(m_effectEvent.m_placeMode == SkillPlaceMode.SelfInside)
		{
			m_effectPrefab.transform.parent = m_selfBB.PrefabMain.transform; 
			m_effectPrefab.transform.localPosition = localPos;
			m_effectPrefab.transform.localScale = new Vector3(1f,1f,1f);
		}
		else if(m_effectEvent.m_placeMode == SkillPlaceMode.SelfOutside)
		{
			Vector3 pos = m_selfBB.DataRunTime.CurPos + localPos;
			if(m_selfBB.DataRunTime.LookDirection == eLookDirection.Right)
			{
				m_effectPrefab.transform.localPosition = pos;
			}
			else
			{
				pos.x = pos.x - localPos.x * 2;
				m_effectPrefab.transform.localPosition = pos;
			}
		}
		else
		{
			Debug.LogError("Not Yet!!!!!!!!!!");
		}
	}

	public virtual eProcessStatus GetStatus()
	{
		return m_status;
	}
	
	public virtual bool IsRunning()
	{
		return (m_status != eProcessStatus.None);
	}
}

public class EffectMoveProcess : EffectMethod
{
	protected Vector3 m_localPos = Vector3.zero;
	protected Vector3 m_motion = Vector3.zero;
	protected float   m_moveTime = 0f;
	protected float   m_duration = 0f;
	
	protected Vector3 m_direction = Vector3.zero;
	protected float   m_speed = 0f;
	protected float   m_remainTime = 0f;

	public override void Start ()
	{
		m_localPos = m_effectEvent.m_localPos;
		m_motion = m_effectEvent.m_motion;
		m_direction = m_motion.normalized;
		m_moveTime = m_effectEvent.m_moveTime;
		m_remainTime = m_duration = m_effectEvent.m_duration;

		if(m_moveTime > 0)
		{
			m_speed = m_motion.magnitude/m_moveTime;
		}
		else
		{
			m_localPos += m_motion;
			m_speed = 0f;
		}

		InitPos(m_localPos);

		m_status = eProcessStatus.Run;
	}

	public override void Update()
	{
		if(m_status == eProcessStatus.Run)
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

	public override void End()
	{
		m_status = eProcessStatus.None;
	}

	public override void Destroy ()
	{
		base.Destroy ();
	}
}

public class EffectLinkProcess : EffectMethod
{
	protected RoleBlackBoard m_targetBB;
	protected UisEffectLinkObj m_effectLink;
	
	public override void Start ()
	{
		m_effectLink = m_effectPrefab.GetComponent<UisEffectLinkObj>();
		m_targetBB = m_selfBB.CtrlSkill.GetSingleTarget();
		if(m_effectLink != null)
		{
			m_effectLink.Initlize(m_selfBB.PrefabMain.transform,m_targetBB.PrefabMain.transform,new Vector3(0,0.8f,0));
			this.m_status = eProcessStatus.Run;
		}
	}

	public override void Update ()
	{
		if(m_status == eProcessStatus.Run)
		{
			m_effectLink.UpdateEffect();
		}
	}

	public override void Stop ()
	{
		this.m_status = eProcessStatus.None;
	}

	public override void End ()
	{
		this.m_status = eProcessStatus.None;
	}

	public override void Destroy ()
	{
		base.Destroy();

		m_effectLink = null;
		m_targetBB = null;
		this.m_status = eProcessStatus.None;
	}
	
}
