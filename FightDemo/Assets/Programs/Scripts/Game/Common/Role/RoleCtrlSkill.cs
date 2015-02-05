using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Util;
using Dk.Event;

public class RoleCtrlSkill
{
	protected bool m_isInited = false;

	protected RoleBlackBoard m_bbData;

	protected CRoleSkillItem m_curSkillItem;
	
	protected SkillProcess m_curSkillProcess = new SkillProcess();

	protected PosProcess m_curPosProcess = new PosProcess();

	protected AlphaProcess m_curAlphaProcess = new AlphaProcess();

	protected ScaleProcess m_curScaleProcess = new ScaleProcess();

	protected CameraProcess m_curCameraProcess = new CameraProcess();

	protected List<EffectProcess> m_curEffectList = new List<EffectProcess>();

	protected List<HitBoundProcess> m_curHitBoundList = new List<HitBoundProcess>();

	protected Dictionary<int,HitTarget> m_curHitTargetDict = new Dictionary<int, HitTarget>();

	protected Dictionary<eSkillKey,RoleSkillCoolTime> m_skillCoolTimeDict = new Dictionary<eSkillKey, RoleSkillCoolTime>();

	protected eProcessStatus m_curSkillStatus = eProcessStatus.None;
	public eProcessStatus CurSkillStatus
	{
		get{return m_curSkillStatus;}
	}

	public void Initalize(RoleBlackBoard bbData)
	{
		if(m_isInited == false)
		{
			m_isInited = true;
			m_bbData = bbData;

			Action<DkEvent> action = OnSkillEnterHandler;
			m_curSkillProcess.AddEventListen(SkillProcess.SKILL_EVENT_ENTER,action);
		}
	}

	public void Update()
	{
		UpdateSkillCoolTime();
	}

	protected void UpdateSkillCoolTime()
	{
		foreach(KeyValuePair<eSkillKey,RoleSkillCoolTime> pair in m_skillCoolTimeDict)
		{
			if(pair.Value.IsCoolTime)
			{
				pair.Value.UpdateCoolTime();
			}
		}
	}

	protected void AddSkillCoolTime(eSkillKey key , float coolTime)
	{
		RoleSkillCoolTime skillCoolTime = null;

		if(!m_skillCoolTimeDict.TryGetValue(key,out skillCoolTime))
		{
			skillCoolTime = new RoleSkillCoolTime();
			m_skillCoolTimeDict.Add(key,skillCoolTime);
		}

		skillCoolTime.StartCoolTime(coolTime);
	}

	public bool IsKeyCool(eSkillKey key)
	{
		RoleSkillCoolTime skillCoolTime = null;
		if(!m_skillCoolTimeDict.TryGetValue(key,out skillCoolTime))
		{
			skillCoolTime = new RoleSkillCoolTime();
			m_skillCoolTimeDict.Add(key,skillCoolTime);
		}

		return skillCoolTime.IsCoolTime;
	}

	public void StartSkillProcess(eSkillKey key, int index)
	{
		if(m_curSkillStatus != eProcessStatus.None)
		{
			FinishSkillProcess();
		}

		CRoleSkillGroup group = GetLocalData.GetSkillGroup(key);
		m_curSkillItem = group.MoveToIndex(index);

		if(group.IsLastIndex)
		{
			AddSkillCoolTime(key,-1);
		}

		m_curSkillProcess.Reset(m_curSkillItem);
		m_curSkillProcess.Start();

		m_curSkillStatus = eProcessStatus.Run;
	}

	public void FinishSkillProcess()
	{
		m_curSkillStatus = eProcessStatus.None;
		//to do;
		m_curSkillProcess.Stop();
		m_curPosProcess.Stop();
		m_curAlphaProcess.Stop();
		m_curScaleProcess.Stop();
		m_curCameraProcess.Stop();

		foreach(EffectProcess effect in m_curEffectList)
		{
			effect.Destroy();
		}
		m_curEffectList.Clear();

		foreach(HitBoundProcess hit in m_curHitBoundList)
		{
			hit.Destroy();
		}
		m_curHitBoundList.Clear();

	    foreach(KeyValuePair<int,HitTarget> pair in m_curHitTargetDict)
		{
			pair.Value.Destroy();
		}
		m_curHitTargetDict.Clear();
	}

	public void UpdateSkillProcess()
	{
		if(m_curSkillStatus == eProcessStatus.Run)
		{
			ProcessSkill();
			ProcessPos();
			ProcessAlpha();
			ProcessScale();
			ProcessHitBound();
			ProcessHitTarget();
			ProcessEffect();
			ProcessCamera();
			CheckAllProcessStatus();
		}
//		else if(m_curSkillStatus == eProcessStatus.End)
//		{
//			Debug.LogError("[CtrlSkill][Update] Something impossible !");
//		}
	}
	
	private void CheckAllProcessStatus()
	{
		if(m_curSkillProcess.GetStatus() == eProcessStatus.End)
		{
			m_curSkillStatus = eProcessStatus.End;

			if(m_curPosProcess.IsRunning())
			{
				Vector3 motion = m_curPosProcess.GetEndPos - GetRunTimeData.CurPos;
				Debug.LogError("[warn] skill Finish but move not finsih , must motion :"+ motion);
				GetTransformCtrl.MoveLimit(motion);
			}
			
			if(m_curAlphaProcess.IsRunning())
			{
				GetRunTimeData.CurAlpha = 1f;
			}
			
			if(m_curScaleProcess.IsRunning())
			{
				GetRunTimeData.CurScale = 1;
			}

			FinishSkillProcess();
		}
	}

	private void ProcessSkill()
	{
		m_curSkillProcess.Update();
	}
	
	private void ProcessPos()
	{
		if(m_curPosProcess.IsRunning())
		{
			m_curPosProcess.Update();
			if(!m_curPosProcess.IsRunning())
			{
				GetRunTimeData.MoveMethod = eMoveMethod.None;
				GetRunTimeData.ForceSpeed = Vector3.zero;
			}
			else
			{
				GetRunTimeData.MoveMethod = eMoveMethod.Motion;
				GetRunTimeData.ForceSpeed = m_curPosProcess.GetNextMotion;
			}
		}
	}
	
	private void ProcessAlpha()
	{
		if(m_curAlphaProcess.IsRunning())
		{
			m_curAlphaProcess.Update();
			GetRunTimeData.CurAlpha = m_curAlphaProcess.GetCurAlpha;
		}
	}
	
	private void ProcessScale()
	{
		if(m_curScaleProcess.IsRunning())
		{
			m_curScaleProcess.Update();
			GetRunTimeData.CurScale = m_curScaleProcess.GetCurScale;
		}
	}
	
	private void ProcessHitBound()
	{
		List<HitBoundProcess> list = new List<HitBoundProcess>();
		foreach(HitBoundProcess hitBound in m_curHitBoundList)
		{
			hitBound.Update();
			if(hitBound.GetStatus() == eProcessStatus.End)
			{
				list.Add(hitBound);
			}
		}

		foreach(HitBoundProcess item in list)
		{
			m_curHitBoundList.Remove(item);
			item.Destroy();
		}

		list.Clear();
	}

	protected void ProcessHitTarget()
	{
		foreach(KeyValuePair<int,HitTarget> pair in m_curHitTargetDict)
		{
			HitTarget hitTarget = pair.Value;
			hitTarget.Update(m_curSkillProcess.GetElapseTime);
		}
	}
		
	private void ProcessEffect()
	{
		List<EffectProcess> list = new List<EffectProcess>();
		foreach(EffectProcess effect in m_curEffectList)
		{
			effect.Update();
			if(!effect.IsRunning())
			{
				list.Add(effect);
			}
		}
		
		foreach(EffectProcess item in list)
		{
			m_curEffectList.Remove(item);
			item.Destroy();
		}
		
		list.Clear();
	}

	private void ProcessCamera()
	{
		if(m_curCameraProcess.IsRunning())
		{
			m_curCameraProcess.Update();
		}
	}

	private void OnSkillEnterHandler(DkEvent evt)
	{
		SkillProcessEvent skillEvent = evt.data as SkillProcessEvent;
		
		switch(skillEvent.SkillEventType)
		{
		case eSkillProcessEventType.ChAnimation:
			AniEventHandler(skillEvent as SkillAniChEvent);
			break;
		case eSkillProcessEventType.ChPos:
			PosEventHandler(skillEvent as SkillPosChEvent);
			break;
		case eSkillProcessEventType.ChAlpha:
			AlphaEventHandler(skillEvent as SkillAlphaChEvent);
			break;
		case eSkillProcessEventType.ChScale:
			ScaleEventHandler(skillEvent as SkillScaleChEvent);
			break;
		case eSkillProcessEventType.ChAttribute:
			AttributeEventHandler(skillEvent as SkillAttributeChEvent);
			break;
		case eSkillProcessEventType.AddEffect:
			EffectEventHandler(skillEvent as SkillEffectAddEvent);
			break;
		case eSkillProcessEventType.AddHitBound:
			HitBoundEventHandler(skillEvent as SkillHitBoundAddEvent);
			break;
		case eSkillProcessEventType.AddMagic:
			MagicEventHandler(skillEvent as SkillMagicAddEvent);
			break;
		case eSkillProcessEventType.ChCamera:
			CameraEventHander(skillEvent as SkillCameraEvent);
			break;
		default:
			Debug.LogError(" Something forgot");
			break;
		}
	}
	
	private void AniEventHandler(SkillAniChEvent skillAniEvent)
	{
		string aniName = skillAniEvent.m_aniName;
		if(GetAniCtrl != null)
		{
			GetAniCtrl.Play(aniName,WrapMode.ClampForever,true,skillAniEvent.m_speed);
		}
	}
	
	private void PosEventHandler(SkillPosChEvent skillPosEvent)
	{
		if(skillPosEvent.m_motion.magnitude == 0)
		{
			return;
		}

		m_curPosProcess.Reset(m_bbData, skillPosEvent);
		m_curPosProcess.Start();
	}
	
	private void AlphaEventHandler(SkillAlphaChEvent skillAlphaEvent)
	{
		if(skillAlphaEvent.m_endAlpha < 0)
		{
			Debug.Log("[error][AlphaEventHandler] event error");
			return;
		}

		m_curAlphaProcess.Reset(m_bbData , skillAlphaEvent);
		m_curAlphaProcess.Start();
	}
	
	private void ScaleEventHandler(SkillScaleChEvent skillScaleEvent)
	{
		if(skillScaleEvent.m_endScale < 0)
		{
			Debug.Log("[error][ScaleEventHandler] event error");
			return;
		}
		

		//m_curScaleProcess.Restart(skillScaleEvent.m_startScale,skillScaleEvent.m_endScale,skillScaleEvent.m_duration);
	
			m_curScaleProcess.Reset(m_bbData,skillScaleEvent);
			m_curScaleProcess.Start();
	}

	private void CameraEventHander(SkillCameraEvent skillCameraEvent)
	{
		m_curCameraProcess.Reset(m_bbData, skillCameraEvent);
		m_curCameraProcess.Start();
	}

	private void AttributeEventHandler(SkillAttributeChEvent skillAttrubuteEvent)
	{
		GetRunTimeData.ActiveChStateEnalbe = skillAttrubuteEvent.m_activeChStateEnable;
		if(skillAttrubuteEvent.m_activeChStateEnable)
		{
			GetRunTimeData.MoveEnable = skillAttrubuteEvent.m_activeChStateEnable;
		}
	}

	private void EffectEventHandler(SkillEffectAddEvent skillEffectEvent)
	{
		if(String.IsNullOrEmpty(skillEffectEvent.m_asset))
		{
			Debug.Log("[error][EffectEventHandler] event error");
			return ;
		}

		EffectProcess process = new EffectProcess();
		process.Initalize(m_bbData,skillEffectEvent);
		process.Start();
		m_curEffectList.Add(process);
	}

	private void HitBoundEventHandler(SkillHitBoundAddEvent skillHitBoundEvent)
	{
		if(skillHitBoundEvent.m_boundSize.sqrMagnitude  == 0)
		{
			Debug.Log("[error][HitBoundEventHandler] event error");
			return ;
		}

		HitBoundProcess process = new HitBoundProcess();
		process.Initalize(m_bbData,skillHitBoundEvent);
		m_curHitBoundList.Add(process);
	}

	private void MagicEventHandler(SkillMagicAddEvent skillMagicEvent)
	{

	}

	//==================================================================//

	public void HitTargetEnter(RoleBlackBoard target , int boundIndex , SkillHitData hitData)
	{
		HitTarget hitTarget = null;
		if(!m_curHitTargetDict.TryGetValue(target.DataInfo.index, out hitTarget))
		{
			hitTarget = new HitTarget();
			hitTarget.Initalize(m_curSkillItem.hitMethod,target,m_bbData);
			m_curHitTargetDict.Add(target.DataInfo.index,hitTarget);
		}
		hitTarget.TargetEnter(boundIndex,hitData);
	}
	
	public void HitTargetOut(RoleBlackBoard target , int boundIndex)
	{
		HitTarget hitTarget = null;
		if(m_curHitTargetDict.TryGetValue(target.DataInfo.index,out hitTarget))
		{
			hitTarget.TargetOut(boundIndex);
		}
	}

	public RoleBlackBoard GetSingleTarget()
	{
		RoleBlackBoard target = SceneManager.GetTargetFrom(1);
		return target;
	}

	//===============================================================//

	protected RoleDataRunTime GetRunTimeData
	{
		get{return m_bbData.DataRunTime;}
	}
	
	protected RoleDataLocal GetLocalData
	{
		get{return m_bbData.DataLocal;}
	}
	
	protected RoleCtrlMessage GetMsgCtrl
	{
		get{return m_bbData.CtrlMsg;}
	}
	
	protected RoleCtrlTransform GetTransformCtrl
	{
		get{return m_bbData.CtrlTransform;}
	}

	protected RoleCtrlAnimation GetAniCtrl
	{
		get{return m_bbData.CtrlAnimation;}
	}
}


public class RoleSkillCoolTime
{
	protected const float DefalueCoolTime = 1.5f;

	protected float m_remainTime = 0f;
	public float GetRemainTime
	{
		get{return m_remainTime;}
	}

	public eProcessStatus m_status = eProcessStatus.None;

	public void StartCoolTime(float coolTime)
	{
		if(coolTime >= 0)
		{
			m_remainTime = coolTime;
		}
		else
		{
			m_remainTime = DefalueCoolTime;
		}
		m_status = eProcessStatus.Start;
	}

	public void UpdateCoolTime()
	{
		if(m_status == eProcessStatus.Start)
		{
			m_status = eProcessStatus.Run;
		}
		else if(m_status == eProcessStatus.Run)
		{
			if(m_remainTime > 0)
			{
				m_remainTime -= TimerManager.Instance.GetDeltaTime;
			}

			if(m_remainTime <= 0)
			{
				m_remainTime = 0;
				m_status = eProcessStatus.End;
			}
		}
		else if(m_status == eProcessStatus.End)
		{
			m_status = eProcessStatus.None;
		}
	}	

	public bool IsCoolTime
	{
		get{return (m_remainTime > 0);}
	}
}