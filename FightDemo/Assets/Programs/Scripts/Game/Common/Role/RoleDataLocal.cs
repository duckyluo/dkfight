using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dk.Util;

public class RoleDataLocal : DkSerializeData
{
	[System.NonSerialized]
	protected CRoleSkill m_cRoleSkill = new CRoleSkill();

	[System.NonSerialized]
	protected RoleBlackBoard m_bbData = null;

	public void Awake()
	{
		if(this.bytes != null && this.bytes.Length > 0)
		{
			SRoleData sData = this.GetDeSerialiezeObj() as SRoleData;
			if(sData != null)
			{
				LoadSData(sData);
			}
		}
	}

	public void Initalize(RoleBlackBoard bbData)
	{
		m_bbData = bbData;

		SRoleSkillItem skill1 = new SRoleSkillItem();
		skill1.skillIndex = 2;
		skill1.skillId = 1;
		skill1.hitTimes = 4;
		skill1.hitInterval = 0.18f;
		
		SkillAniChEvent skillThreeEvt1 = new SkillAniChEvent();
		skillThreeEvt1.m_aniName = AnimationNameDef.Attack5;
		skillThreeEvt1.m_startTime = 0f;
		skillThreeEvt1.m_duration = 1.5f;
		skillThreeEvt1.m_speed = 1f;
		skill1.aniList.Add(skillThreeEvt1);
		
//		SkillScaleChEvent skillThreeEvt2 = new SkillScaleChEvent();
//		skillThreeEvt2.m_startTime = 0f;
//		skillThreeEvt2.m_duration = 0.6f;
//		skillThreeEvt2.m_endScale = 1.5f;
//		skill3.scaleList.Add(skillThreeEvt2);
//		
//		SkillScaleChEvent skillThreeEvt3 = new SkillScaleChEvent();
//		skillThreeEvt3.m_startTime = 1.3f;
//		skillThreeEvt3.m_duration = 0.2f;
//		skillThreeEvt3.m_endScale = 1f;
//		skill3.scaleList.Add(skillThreeEvt3);

		SkillAttributeChEvent skill1Event3 = new SkillAttributeChEvent();
		skill1Event3.m_startTime = 1.2f;
		skill1Event3.m_activeChStateEnable = true;
		skill1.attributeList.Add(skill1Event3);

		SkillHitBoundAddEvent hitEvent1 = new SkillHitBoundAddEvent();
		hitEvent1.m_startTime = 0.2f;
		hitEvent1.m_duration = 1f;
		hitEvent1.m_localPos = new Vector3(0.5f,1f,0f);
		hitEvent1.m_boundSize = new Vector3(1.5f,1.5f,1.5f);
//		hitEvent1.m_motion = new Vector3(1f,0f,0f);
//		hitEvent1.m_moveTime = 0.5f;
		skill1.hitBoundList.Add(hitEvent1);
		//SkillPosChEvent moveEvent = new SkillPosChEvent();

		SkillEffectAddEvent effectEvent1 = new SkillEffectAddEvent();
		effectEvent1.m_startTime = 0f;
		effectEvent1.m_asset = "Assets/SourceData/Prefabs/Effect/Effect1.prefab";
		effectEvent1.m_localPos = new Vector3(0.47f,0f,-0.5f);
		skill1.effectList.Add(effectEvent1);
		
		m_cRoleSkill.AddSkillItem(eSkillKey.Attack,skill1);

		//===================================================================//


		//=====================================================================//
	
		SRoleSkillItem skill2 = new SRoleSkillItem();
		skill2.skillIndex = 1;
		skill2.skillId = 2;
		skill2.hitTimes = 1;
		
		SkillAniChEvent skillTwoEvt4 = new SkillAniChEvent();
		skillTwoEvt4.m_aniName = AnimationNameDef.Attack4;
		skillTwoEvt4.m_startTime = 0f;
		skillTwoEvt4.m_duration = 0.77f;
		skill2.aniList.Add(skillTwoEvt4);

		SkillPosChEvent skillTwoEvt5 = new SkillPosChEvent();
		skillTwoEvt5.m_startTime = 0.2f;
		skillTwoEvt5.m_duration = 0.4f;
		skillTwoEvt5.m_skillMoveMethod = eSkillMoveMethod.Translation;
		skillTwoEvt5.m_motion = new Vector3(4f,0f,0f);
		skillTwoEvt5.m_aSpeed = -100f;
		skill2.posList.Add(skillTwoEvt5);

		SkillAlphaChEvent skillTowEvt6 = new SkillAlphaChEvent();
		skillTowEvt6.m_startTime = 0.2f;
		skillTowEvt6.m_duration = 0.4f;
		skillTowEvt6.m_startAlpha = 0f;
		skillTowEvt6.m_endAlpha = 1f;
		skill2.alphaList.Add(skillTowEvt6);

		SkillHitBoundAddEvent hitEvent2 = new SkillHitBoundAddEvent();
		hitEvent2.m_startTime = 0.4f;
		hitEvent2.m_duration = 0.3f;
		hitEvent2.m_localPos = new Vector3(1f,1f,0f);
		hitEvent2.m_boundSize = new Vector3(1.5f,1.5f,1.5f);
		//		hitEvent1.m_motion = new Vector3(1f,0f,0f);
		//		hitEvent1.m_moveTime = 0.5f;
		skill2.hitBoundList.Add(hitEvent2);

		SkillEffectAddEvent effectEvent2 = new SkillEffectAddEvent();
		effectEvent2.m_startTime = 0.3f;
		//effectEvent2.m_duration = 0.
		effectEvent2.m_asset = "Assets/SourceData/Prefabs/Effect/Effect2.prefab";
		effectEvent2.m_localPos = new Vector3(0.5f,1f,-0.5f);
		skill2.effectList.Add(effectEvent2);
		
		m_cRoleSkill.AddSkillItem(eSkillKey.Attack,skill2);

		//===============================================================//


		SRoleSkillItem skill3 = new SRoleSkillItem();
		skill3.skillIndex = 0;
		skill3.skillId = 4;
		skill3.durationTime = 0.9f;
		
		SkillAniChEvent skill3evt1 = new SkillAniChEvent();
		skill3evt1.m_aniName = AnimationNameDef.Attack1;
		skill3evt1.m_startTime = 0f;
		skill3evt1.m_duration = 0.44f;
		skill3.aniList.Add(skill3evt1);
		
		SkillAniChEvent skill3evt2 = new SkillAniChEvent();
		skill3evt2.m_aniName = AnimationNameDef.Attack2;
		skill3evt2.m_startTime = 0.45f;
		skill3evt2.m_duration = 0.33f;
		skill3.aniList.Add(skill3evt2);

//		SkillHitBoundAddEvent hitEvent1 = new SkillHitBoundAddEvent();
//		hitEvent1.m_startTime = 0.1f;
//		hitEvent1.m_duration = 0.6f;
//		hitEvent1.m_localPos = new Vector3(1f,1f,0f);
//		hitEvent1.m_boundSize = new Vector3(1f,1f,1f);
//		hitEvent1.m_motion = new Vector3(1f,0f,0f);
//		hitEvent1.m_moveTime = 0.5f;
//		skill1.hitBoundList.Add(hitEvent1);
//		//SkillPosChEvent moveEvent = new SkillPosChEvent();
//
		SkillEffectAddEvent skill3evt4 = new SkillEffectAddEvent();
		skill3evt4.m_startTime = 0.1f;
		skill3evt4.m_asset = "Assets/SourceData/Prefabs/Effect/Effect4.prefab";
		skill3evt4.m_localPos = new Vector3(1f,1f,0f);
		skill3.effectList.Add(skill3evt4);
		
		SkillEffectAddEvent skill3evt5 = new SkillEffectAddEvent();
		skill3evt5.m_startTime = 0.5f;
		skill3evt5.m_asset = "Assets/SourceData/Prefabs/Effect/Effect5.prefab";
		skill3evt5.m_localPos = new Vector3(1f,1f,0f);
		skill3.effectList.Add(skill3evt5);
		
		m_cRoleSkill.AddSkillItem(eSkillKey.Attack,skill3);


		//===============================================================//

		SRoleSkillItem skillJump = new SRoleSkillItem();
		skillJump.skillIndex = 0;
		skillJump.skillId = 4;
		skillJump.durationTime = 0.9f;
		
		SkillAniChEvent aniEvent1 = new SkillAniChEvent();
		aniEvent1.m_aniName = AnimationNameDef.Attack1;
		aniEvent1.m_startTime = 0f;
		aniEvent1.m_duration = 0.44f;
		skillJump.aniList.Add(aniEvent1);
		
		SkillAniChEvent aniEvent2 = new SkillAniChEvent();
		aniEvent2.m_aniName = AnimationNameDef.Attack2;
		aniEvent2.m_startTime = 0.45f;
		aniEvent2.m_duration = 0.33f;
		skillJump.aniList.Add(aniEvent2);
		
		//		SkillChAnimationEvent aniEvent3 = new SkillChAnimationEvent();
		//		aniEvent3.m_aniName = AnimationNameDef.Attack3;
		//		aniEvent3.m_startTime = 0.78f;
		//		aniEvent3.m_duration = 0.27f;
		//		skill1.aniList.Add(aniEvent3);
		
		//		SkillHitBoundAddEvent hitEvent1 = new SkillHitBoundAddEvent();
		//		hitEvent1.m_startTime = 0.1f;
		//		hitEvent1.m_duration = 0.6f;
		//		hitEvent1.m_localPos = new Vector3(1f,1f,0f);
		//		hitEvent1.m_boundSize = new Vector3(1f,1f,1f);
		//		hitEvent1.m_motion = new Vector3(1f,0f,0f);
		//		hitEvent1.m_moveTime = 0.5f;
		//		skill1.hitBoundList.Add(hitEvent1);
		//		//SkillPosChEvent moveEvent = new SkillPosChEvent();
		//
		SkillEffectAddEvent effectEvent4 = new SkillEffectAddEvent();
		effectEvent4.m_startTime = 0.1f;
		effectEvent4.m_asset = "Assets/SourceData/Prefabs/Effect/Effect4.prefab";
		effectEvent4.m_localPos = new Vector3(1f,1f,0f);
		skillJump.effectList.Add(effectEvent4);

		SkillEffectAddEvent effectEvent5 = new SkillEffectAddEvent();
		effectEvent5.m_startTime = 0.5f;
		effectEvent5.m_asset = "Assets/SourceData/Prefabs/Effect/Effect5.prefab";
		effectEvent5.m_localPos = new Vector3(1f,1f,0f);
		skillJump.effectList.Add(effectEvent5);
		
		m_cRoleSkill.AddSkillItem(eSkillKey.JumpAttack,skillJump);

		//==========================================================================//
	}

	public void SaveSData(SRoleData sData)
	{
		LoadSData(sData);
		this.DoSerialize(sData);
	}

	public void LoadSData(SRoleData sData)
	{
		m_cRoleSkill.SetSkillData(sData.roleSkill);
	}

	public CRoleSkillGroup GetSkillGroup(eSkillKey key)
	{
		return m_cRoleSkill.GetSkillGroup(key);
	}

	public RoleBlackBoard GetBBData()
	{
		return m_bbData;
	}
	
//	public void OnParticleCollision(GameObject other)
//	{
//		Debug.Log(this.gameObject+" OnParticleCollision! ");
//	}
//
//	public void OnCollisionEnter(Collision collisionInfo)
//	{
//		Debug.Log(this.gameObject+" OnCollisionEnter!");
//	}
//
//	public void OnTriggerEnter(Collider other) 
//	{
//		Debug.Log(this.gameObject+" Role OnTriggerEnter! " + other.gameObject);
//	}

}


