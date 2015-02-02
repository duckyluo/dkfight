using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SRoleSkillData
{
	public List<SRoleSkillItem> skillList = new List<SRoleSkillItem>();
	public List<int> attackList = new List<int>();
	public List<int> jumpAttackList = new List<int>();
	public List<int> skillOneList = new List<int>();
	public List<int> skillTwoList = new List<int>();
	public List<int> skillThreeList = new List<int>();
	public List<int> skillFourList = new List<int>();
}

[System.Serializable]
public class SRoleSkillItem 
{
	public int skillIndex = 0;
	public int skillId = -1;
	public float durationTime = -1;
	public eHitMethod hitMethod = eHitMethod.Not_Use;
	
	public List<SkillAniChEvent> aniList = new List<SkillAniChEvent>();
	public List<SkillPosChEvent> posList = new List<SkillPosChEvent>();
	public List<SkillAlphaChEvent> alphaList = new List<SkillAlphaChEvent>();
	public List<SkillScaleChEvent> scaleList = new List<SkillScaleChEvent>();
	public List<SkillAttributeChEvent> attributeList = new List<SkillAttributeChEvent>();
	public List<SkillEffectAddEvent> effectList = new List<SkillEffectAddEvent>();
	public List<SkillHitBoundAddEvent> hitBoundList = new List<SkillHitBoundAddEvent>();
	public List<SkillMagicAddEvent> magicList = new List<SkillMagicAddEvent>();
}

[System.Serializable]
public class SkillProcessEvent : IComparable
{
	protected eSkillProcessEventType m_skillEventType = eSkillProcessEventType.Not_Use;
	public eSkillProcessEventType SkillEventType
	{
		get{ return m_skillEventType;}
	}
	
	public float m_startTime = 0;
	public float m_duration = -1;
	
	public int CompareTo(object obj)
	{
		int res = 0;
		try
		{
			SkillProcessEvent item = (SkillProcessEvent)obj;
			if(this.m_startTime > item.m_startTime)
			{
				res = 1;
			}
			else if(this.m_startTime < item.m_startTime)
			{
				res = -1;
			}
		}
		catch(Exception ex)
		{
			Debug.Log(ex.StackTrace);
		}
		return res;
	}
	
	public float GetEndTime
	{
		get
		{
			if(m_duration > 0)
			{
				return m_startTime + m_duration;
			}
			else return m_startTime;
		}
	}
}

[System.Serializable]
public class SkillAniChEvent : SkillProcessEvent
{
	public string m_aniName = string.Empty;
	public float m_speed = 1f;
	
	public SkillAniChEvent()
	{
		m_skillEventType = eSkillProcessEventType.ChAnimation;
	}
}

[System.Serializable]
public class SkillPosChEvent : SkillProcessEvent
{
	public eSkillMoveMethod m_skillMoveMethod = eSkillMoveMethod.Translation;
	public Vector3 m_motion = Vector3.zero;
	public float m_aSpeed = 0f;
	
	public SkillPosChEvent()
	{
		m_skillEventType = eSkillProcessEventType.ChPos;
	}
}

[System.Serializable]
public class SkillAlphaChEvent : SkillProcessEvent
{
	public float m_startAlpha = -1f;
	public float m_endAlpha = -1f;
	
	public SkillAlphaChEvent()
	{
		m_skillEventType = eSkillProcessEventType.ChAlpha;
	}
}

[System.Serializable]
public class SkillScaleChEvent : SkillProcessEvent
{
	public float m_startScale = -1f;
	public float m_endScale = -1f;
	
	public SkillScaleChEvent()
	{
		m_skillEventType = eSkillProcessEventType.ChScale;
	}
}

[System.Serializable]
public class SkillAttributeChEvent : SkillProcessEvent
{
	public bool m_activeChStateEnable = true;
	
	public SkillAttributeChEvent()
	{
		m_skillEventType = eSkillProcessEventType.ChAttribute;
	}
}

[System.Serializable]
public class SkillEffectAddEvent : SkillProcessEvent
{
	public String  m_asset = string.Empty;
	public Vector3 m_localPos = Vector3.zero;
	public Vector3 m_motion = Vector3.zero;
	public float   m_moveTime = 0f;

	public SkillEffectAddEvent()
	{
		m_skillEventType = eSkillProcessEventType.AddEffect;
	}
}

[System.Serializable]
public class SkillHitBoundAddEvent : SkillProcessEvent
{
	public int 	   m_boundIndex = 0;
	public Vector3 m_boundSize = Vector3.zero;
	public Vector3 m_localPos = Vector3.zero;
	public Vector3 m_motion = Vector3.zero;
	public float   m_moveTime = 0f;
	public bool    m_IsLocal = true;
	public SkillHitData m_hitData = new SkillHitData();
	
	public SkillHitBoundAddEvent()
	{
		m_skillEventType = eSkillProcessEventType.AddHitBound;
	}
}

[System.Serializable]
public class SkillMagicAddEvent : SkillProcessEvent
{
	public SkillMagicAddEvent()
	{
		m_skillEventType = eSkillProcessEventType.AddMagic;
	}
}

[System.Serializable]
public class SkillHitData
{
	public int hitTimes = 1;				//次数
	public float hitInterval = 0f;
	public eSkillHitForce hitForce = eSkillHitForce.Not_Use;
	public eHitMoment hitMoment = eHitMoment.Not_Use;
	public eSkillHitLookDirection hitLook = eSkillHitLookDirection.OppositeAttackerLook;
	public Vector3 hitSpeed = Vector3.zero;
}


