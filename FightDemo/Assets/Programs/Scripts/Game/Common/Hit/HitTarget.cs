using UnityEngine;

public class HitTarget
{
	protected HitMethod m_process = null;

	protected RoleBlackBoard m_self = null;

	protected RoleBlackBoard m_target = null;

	protected eHitMethod m_hitMethod = eHitMethod.Not_Use;

	protected bool m_isInited = false;
	
	public void Initalize(eHitMethod hitMethod, RoleBlackBoard target , RoleBlackBoard self)
	{
		m_self = self;
		m_target = target;
		m_hitMethod = hitMethod;

		if(m_hitMethod == eHitMethod.None || m_hitMethod == eHitMethod.Not_Use)
		{
			m_hitMethod = eHitMethod.HitCommonByNum;
			Debug.LogError("[warn] m_hitMethod is null !!!");
		}

		m_process = MakeProcessByHitMethod(m_hitMethod);
		if(m_process != null)
		{
			m_process.Initalize(target,self);
			m_isInited = true;
		}
	}

	public void Destroy()
	{
		if(m_process != null)
		{
			m_process.Destroy();
		}
		m_process = null;
	}

	public void TargetEnter(int boundIndex , SkillHitData hitData)
	{
		if(m_isInited)
		{
			m_process.TargetEnter(boundIndex,hitData);
		}
	}
	
	public void TargetOut(int boundIndex)
	{
		if(m_isInited)
		{
			m_process.TargetOut(boundIndex);
		}
	}
	
	public void Update(float timeCount)
	{
		if(m_isInited && m_process.GetStatus != eProcessStatus.End)
		{
			m_process.Update(timeCount);
		}
	}

	protected HitMethod MakeProcessByHitMethod(eHitMethod hitMethod)
	{
		HitMethod process = null;
		switch(hitMethod)
		{
		case eHitMethod.HitCommonByNum:
			process = new HitCommonByNumProcess();
			break;
		case eHitMethod.HitCommonByStay:
			process = new HitCommonByStayProcess();
			break;
		case eHitMethod.HitAloneByNum:
			process = new HitAloneByNumProcess();
			break;
		}
		
		return process;
	}
}

