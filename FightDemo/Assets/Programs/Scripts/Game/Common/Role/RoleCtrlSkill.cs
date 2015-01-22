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

	protected CRoleSkillItem m_skillItem;
	
	protected SkillProcess m_skillProcess = new SkillProcess();

	protected PosProcess m_posProcess = new PosProcess();

	protected AlphaProcess m_alphaProcess = new AlphaProcess();

	protected ScaleProcess m_scaleProcess = new ScaleProcess();

	protected List<EffectProcess> m_effectList = new List<EffectProcess>();

	protected List<HitBoundProcess> m_hitBoundList = new List<HitBoundProcess>();

	protected Dictionary<int,HitTargetProcess> m_hitTargetDict = new Dictionary<int, HitTargetProcess>();

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
			m_skillProcess.AddEventListen(SkillProcess.SKILL_EVENT_ENTER,action);
		}
	}

	public void StartSkillProcess(eSkillKey key, int index)
	{
		if(m_curSkillStatus != eProcessStatus.None)
		{
			FinishSkillProcess();
		}

		m_skillItem = GetLocalData.GetSkillGroup(key).MoveToIndex(index);
		m_skillProcess.Reset(m_skillItem);
		m_skillProcess.Start();

		m_curSkillStatus = eProcessStatus.Run;
	}

	public void FinishSkillProcess()
	{
		m_curSkillStatus = eProcessStatus.None;
		//to do;
		m_skillProcess.Stop();
		m_posProcess.Stop();
		m_alphaProcess.Stop();
		m_scaleProcess.Stop();

		foreach(EffectProcess effect in m_effectList)
		{
			effect.Destroy();
		}
		m_effectList.Clear();

		foreach(HitBoundProcess hit in m_hitBoundList)
		{
			hit.Destroy();
		}
		m_hitBoundList.Clear();

	    foreach(KeyValuePair<int,HitTargetProcess> pair in m_hitTargetDict)
		{
			pair.Value.Destroy();
		}
		m_hitTargetDict.Clear();
	}

	public void UpdateSkillProcess()
	{
		if(m_curSkillStatus == eProcessStatus.Run)
		{
			ProcessSkill();
			ProcessPos();
			ProcessAlpha();
			ProcessScale();
			ProcessEffect();
			ProcessHitBound();
			ProcessHitTarget();
			CheckAllProcessStatus();
		}
		else if(m_curSkillStatus == eProcessStatus.End)
		{
			Debug.LogError("[CtrlSkill][Update] Something impossible !");
		}
	}

	public void AddHitTarget(RoleBlackBoard target)
	{
		if(this.m_hitTargetDict.ContainsKey(target.DataInfo.index) == false)
		{
			HitTargetProcess hitTarget = new HitTargetProcess();
			hitTarget.Initalize(target,m_skillItem.hitData);
			m_hitTargetDict.Add(target.DataInfo.index,hitTarget);
		}
	}

	private void CheckAllProcessStatus()
	{
		if(m_skillProcess.Status == eProcessStatus.End)
		{
			m_curSkillStatus = eProcessStatus.End;

			if(m_posProcess.IsRunning)
			{
				Vector3 motion = m_posProcess.GetEndPos - GetRunTimeData.CurPos;
				Debug.LogError("[warn] skill Finish but move not finsih , must motion :"+ motion);
				GetTransformCtrl.MoveLimit(motion);
			}
			
			if(m_alphaProcess.IsRunning)
			{
				GetRunTimeData.CurAlpha = 1f;
			}
			
			if(m_scaleProcess.IsRunning)
			{
				GetRunTimeData.CurScale = 1;
			}

			FinishSkillProcess();
		}
	}

	private void ProcessSkill()
	{
		m_skillProcess.Update();
	}
	
	private void ProcessPos()
	{
		if(m_posProcess.IsRunning)
		{
			Vector3 motion = m_posProcess.UpdateMotion();
			if(!m_posProcess.IsRunning)
			{
				GetRunTimeData.MoveMethod = eMoveMethod.None;
				GetRunTimeData.ForceSpeed = Vector3.zero;
			}
			else
			{
				GetRunTimeData.MoveMethod = eMoveMethod.Motion;
				GetRunTimeData.ForceSpeed = motion;
			}
		}
	}
	
	private void ProcessAlpha()
	{
		if(m_alphaProcess.IsRunning)
		{
			m_alphaProcess.UpdateAlpha();
			GetRunTimeData.CurAlpha = m_alphaProcess.GetCurAlpha;
		}
	}
	
	private void ProcessScale()
	{
		if(m_scaleProcess.IsRunning)
		{
			m_scaleProcess.UpdateScale();
			GetRunTimeData.CurScale = m_scaleProcess.GetCurScale;
		}
	}

	private void ProcessEffect()
	{
		List<EffectProcess> list = new List<EffectProcess>();
		foreach(EffectProcess effect in m_effectList)
		{
			effect.Update();
			if(effect.Status == eProcessStatus.End)
			{
				list.Add(effect);
			}
		}
		
		foreach(EffectProcess item in list)
		{
			m_effectList.Remove(item);
			item.Destroy();
		}
		
		list.Clear();
	}

	private void ProcessHitBound()
	{
		List<HitBoundProcess> list = new List<HitBoundProcess>();
		foreach(HitBoundProcess hitBound in m_hitBoundList)
		{
			hitBound.Update();
			if(hitBound.Status == eProcessStatus.End)
			{
				list.Add(hitBound);
			}
		}

		foreach(HitBoundProcess item in list)
		{
			m_hitBoundList.Remove(item);
			item.Destroy();
		}

		list.Clear();
	}

	protected void ProcessHitTarget()
	{
		foreach(KeyValuePair<int,HitTargetProcess> pair in m_hitTargetDict)
		{
			pair.Value.Update();
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
		default:
			Debug.Log("[warn] something forgot");
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
		
		if(skillPosEvent.m_skillMoveMethod == eSkillMoveMethod.Translation)
		{
			Vector3 motionDis = skillPosEvent.m_motion;
			if(GetRunTimeData.LookDirection == eLookDirection.Left)
			{
				motionDis.x = -motionDis.x;
			}
			m_posProcess.Restart(GetRunTimeData.CurPos,motionDis,skillPosEvent.m_duration,skillPosEvent.m_aSpeed);
		}
	}
	
	private void AlphaEventHandler(SkillAlphaChEvent skillAlphaEvent)
	{
		if(skillAlphaEvent.m_endAlpha < 0)
		{
			Debug.Log("[error][AlphaEventHandler] event error");
			return;
		}
		
		if(skillAlphaEvent.m_startAlpha < 0)
		{
			skillAlphaEvent.m_startAlpha = GetRunTimeData.CurAlpha;
		}
		m_alphaProcess.Restart(skillAlphaEvent.m_startAlpha,skillAlphaEvent.m_endAlpha,skillAlphaEvent.m_duration);
	}
	
	private void ScaleEventHandler(SkillScaleChEvent skillScaleEvent)
	{
		if(skillScaleEvent.m_endScale < 0)
		{
			Debug.Log("[error][ScaleEventHandler] event error");
			return;
		}
		
		if(skillScaleEvent.m_startScale < 0)
		{
			skillScaleEvent.m_startScale = GetRunTimeData.CurScale;
		}
		m_scaleProcess.Restart(skillScaleEvent.m_startScale,skillScaleEvent.m_endScale,skillScaleEvent.m_duration);
	}

	private void AttributeEventHandler(SkillAttributeChEvent skillAttrubuteEvent)
	{
		GetRunTimeData.ActiveChStateEnalbe = skillAttrubuteEvent.m_activeChStateEnable;
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
		m_effectList.Add(process);
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
		m_hitBoundList.Add(process);
	}

	private void MagicEventHandler(SkillMagicAddEvent skillMagicEvent)
	{

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