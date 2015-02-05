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

		if(m_bbData.DataInfo.team == eSceneTeamType.Me)
		{
			this.gameObject.tag = TagDef.Me;
		}


		//========================== Skill 1 =================================//

		SRoleSkillItem skill1 = new SRoleSkillItem();
		skill1.skillIndex = 1;
		skill1.skillId = 1;
		skill1.hitMethod = eHitMethod.HitCommonByStay;
		
		SkillAniChEvent skill1Evt1 = new SkillAniChEvent();
		skill1Evt1.m_aniName = AnimationNameDef.Attack5;
		skill1Evt1.m_startTime = 0f;
		skill1Evt1.m_duration = 1.5f;
		skill1Evt1.m_speed = 1f;
		skill1.aniList.Add(skill1Evt1);
		
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
		
		SkillAttributeChEvent skill1Evt3 = new SkillAttributeChEvent();
		skill1Evt3.m_startTime = 1.2f;
		skill1Evt3.m_activeChStateEnable = true;
		skill1.attributeList.Add(skill1Evt3);
		
		SkillHitBoundAddEvent skill1Evt4 = new SkillHitBoundAddEvent();
		skill1Evt4.m_startTime = 0.1f;
		skill1Evt4.m_duration = 0.8f;
		skill1Evt4.m_localPos = new Vector3(1f,0.8f,0f);
		skill1Evt4.m_boundSize = new Vector3(2f,1.5f,1f);
		skill1Evt4.m_hitData.hitTimes = 4;
		skill1Evt4.m_hitData.hitInterval = 0.2f;
		skill1Evt4.m_hitData.hitSound = SoundDef.SWORD_HIT;
		//skill1Evt4.m_hitData.hitForce = eSkillHitForce.Force_Hit;
		skill1.hitBoundList.Add(skill1Evt4);
		
		SkillEffectAddEvent skill1Evt5 = new SkillEffectAddEvent();
		skill1Evt5.m_startTime = 0f;
		skill1Evt5.m_asset = "Assets/SourceData/Prefabs/Effect/Effect1.prefab";
		skill1Evt5.m_localPos = new Vector3(0.47f,0f,-0.5f);
		skill1.effectList.Add(skill1Evt5);
		
		m_cRoleSkill.AddSkillItem(eSkillKey.SkillOne,skill1);

		//=========================== Skill 2 ==============================//
		
		SRoleSkillItem skill2 = new SRoleSkillItem();
		skill2.skillIndex = 2;
		skill2.skillId = 2;
		skill2.hitMethod = eHitMethod.HitCommonByNum;
		
		SkillAniChEvent skill2Evt1 = new SkillAniChEvent();
		skill2Evt1.m_aniName = AnimationNameDef.Attack4;
		skill2Evt1.m_startTime = 0f;
		skill2Evt1.m_duration = 0.77f;
		skill2.aniList.Add(skill2Evt1);
		
		SkillPosChEvent skill2Evt2 = new SkillPosChEvent();
		skill2Evt2.m_startTime = 0.2f;
		skill2Evt2.m_duration = 0.4f;
		skill2Evt2.m_skillMoveMethod = eSkillMoveMethod.Translation;
		skill2Evt2.m_motion = new Vector3(4f,0f,0f);
		skill2Evt2.m_aSpeed = -100f;
		skill2.posList.Add(skill2Evt2);
		
		SkillAlphaChEvent skill2Evt3 = new SkillAlphaChEvent();
		skill2Evt3.m_startTime = 0.2f;
		skill2Evt3.m_duration = 0.4f;
		skill2Evt3.m_startAlpha = 0f;
		skill2Evt3.m_endAlpha = 1f;
		skill2.alphaList.Add(skill2Evt3);
		
		SkillHitBoundAddEvent skill2Evt4 = new SkillHitBoundAddEvent();
		skill2Evt4.m_startTime = 0f;
		skill2Evt4.m_duration = 0.4f;
		skill2Evt4.m_localPos = new Vector3(1.5f,0.8f,0f);
		skill2Evt4.m_boundSize = new Vector3(4f,2f,1f);
		//skill2Evt4.m_IsLocal = false;
		skill2Evt4.m_placeMode = SkillPlaceMode.SelfOutside;
		skill2Evt4.m_hitData.hitMoment = eHitMoment.MoveXPos;
		skill2Evt4.m_hitData.hitTimes = 1;
		skill2Evt4.m_hitData.hitLook = eSkillHitLookDirection.OppositeAttackerMove;
		skill2Evt4.m_hitData.hitSpeed = new Vector3(3.5f,14f,0);
		skill2Evt4.m_hitData.hitSound = SoundDef.SWORD_HIT;
//		skill2Evt4.m_motion = new Vector3(1f,0f,0f);
//		skill2Evt4.m_moveTime = 0.5f;
		skill2.hitBoundList.Add(skill2Evt4);

		SkillHitBoundAddEvent skill2Evt6 = new SkillHitBoundAddEvent();
		skill2Evt6.m_startTime = 0.4f;
		skill2Evt6.m_duration = 0.4f;
		skill2Evt6.m_localPos = new Vector3(0.8f,0.8f,0f);
		skill2Evt6.m_boundSize = new Vector3(3f,1.5f,1f);
		skill2Evt6.m_hitData.hitTimes = 1;
		skill2Evt6.m_hitData.hitSound = SoundDef.SWORD_HIT;
		//skill2Evt6.m_hitData.hitSpeed = new Vector3(3.5f,13f,0);
		//		skill2Evt4.m_motion = new Vector3(1f,0f,0f);
		//		skill2Evt4.m_moveTime = 0.5f;
		skill2.hitBoundList.Add(skill2Evt6);
		
		SkillEffectAddEvent skill2Evt5 = new SkillEffectAddEvent();
		skill2Evt5.m_startTime = 0.3f;
		//skill2Evt5.m_duration = 0.
		skill2Evt5.m_asset = "Assets/SourceData/Prefabs/Effect/Effect2.prefab";
		skill2Evt5.m_localPos = new Vector3(0.5f,1f,-0.5f);
		skill2.effectList.Add(skill2Evt5);
		
		m_cRoleSkill.AddSkillItem(eSkillKey.SkillTwo,skill2);

		//===========================  Skill 3    ==============================//

		SRoleSkillItem skill3 = new SRoleSkillItem();
		skill3.skillIndex = 1;
		skill3.skillId = 1;
		skill3.sound = SoundDef.Air;
		skill3.hitMethod = eHitMethod.HitCommonByNum;
	
		SkillAniChEvent skill3Evt1 = new SkillAniChEvent();
		skill3Evt1.m_aniName = AnimationNameDef.Skill3;
		skill3Evt1.m_startTime = 0f;
		skill3Evt1.m_duration = 0.8f;
		skill3Evt1.m_speed = 1f;
		skill3.aniList.Add(skill3Evt1);

		SkillHitBoundAddEvent skill3Evt2 = new SkillHitBoundAddEvent();
		skill3Evt2.m_startTime = 0.2f;
		skill3Evt2.m_duration = 0.4f;
		skill3Evt2.m_localPos = new Vector3(0f,0.8f,0f);
		skill3Evt2.m_boundSize = new Vector3(1f,1.5f,1f);
		skill3Evt2.m_placeMode = SkillPlaceMode.TargetInside;
		skill3Evt2.m_hitData.hitTimes = 1;
		skill3Evt2.m_hitData.hitForce = eSkillHitForce.Force_ComeUp;
		skill3Evt2.m_hitData.hitDuration = 0.3f;
		//skill3Evt2.m_hitData.hitSound = SoundDef.Air;
		//skill1Evt4.m_hitData.hitForce = eSkillHitForce.Force_Hit;
		skill3.hitBoundList.Add(skill3Evt2);

		SkillEffectAddEvent skill3Evt3 = new SkillEffectAddEvent();
		skill3Evt3.m_startTime = 0.2f;
		skill3Evt3.m_asset = "Assets/SourceData/Prefabs/Effect/Effect6.prefab";
		skill3Evt3.m_effectMethod = eSkillEffectMethod.Link;
		skill3Evt3.m_placeMode = SkillPlaceMode.TargetInside;
		skill3Evt3.m_localPos = new Vector3(0f,1f,0f);
		skill3.effectList.Add(skill3Evt3);
		
		m_cRoleSkill.AddSkillItem(eSkillKey.SkillThree,skill3);

		//============================= Skill 4 ===================================//

		SRoleSkillItem skill4 = new SRoleSkillItem();
		skill4.skillIndex = 1;
		skill4.skillId = 1;
		skill4.durationTime = 3f;
		skill4.sound = SoundDef.Not_Use;
		skill4.hitMethod = eHitMethod.HitAloneByNum;
		
		SkillAniChEvent skill4Evt1 = new SkillAniChEvent();
		skill4Evt1.m_aniName = AnimationNameDef.Skill4;
		skill4Evt1.m_startTime = 0f;
		skill4Evt1.m_duration = 0.8f;
		skill4Evt1.m_speed = 1f;
		skill4.aniList.Add(skill4Evt1);

		SkillAniChEvent skill4Evt2 = new SkillAniChEvent();
		skill4Evt2.m_aniName = AnimationNameDef.Skill4;
		skill4Evt2.m_startTime = 0.8f;
		skill4Evt2.m_duration = 0.9f;
		skill4Evt2.m_speed = 1f;
		skill4.aniList.Add(skill4Evt2);

		SkillAniChEvent skill4Evt3 = new SkillAniChEvent();
		skill4Evt3.m_aniName = AnimationNameDef.Skill4;
		skill4Evt3.m_startTime = 1.6f;
		skill4Evt3.m_duration = 0.9f;
		skill4Evt3.m_speed = 1f;
		skill4.aniList.Add(skill4Evt3);

		SkillEffectAddEvent skill4Evt4 = new SkillEffectAddEvent();
		skill4Evt4.m_startTime = 0.2f;
		skill4Evt4.m_asset = "Assets/SourceData/Prefabs/Effect/Effect7.prefab";
		skill4Evt4.m_localPos = new Vector3(2f,0f,0f);
		skill4Evt4.m_duration = 0.2f;
		skill4Evt4.m_sound = SoundDef.Explode;
		skill4.effectList.Add(skill4Evt4);

		SkillEffectAddEvent skill4Evt5 = new SkillEffectAddEvent();
		skill4Evt5.m_startTime = 1f;
		skill4Evt5.m_asset = "Assets/SourceData/Prefabs/Effect/Effect7.prefab";
		skill4Evt5.m_localPos = new Vector3(5f,0f,0f);
		skill4Evt5.m_duration = 0.4f;
		skill4Evt5.m_sound = SoundDef.Explode;
		skill4.effectList.Add(skill4Evt5);

		SkillEffectAddEvent skill4Evt6 = new SkillEffectAddEvent();
		skill4Evt6.m_startTime = 1.8f;
		skill4Evt6.m_asset = "Assets/SourceData/Prefabs/Effect/Effect7.prefab";
		skill4Evt6.m_localPos = new Vector3(8f,0f,0f);
		skill4Evt6.m_duration = 1.5f;
		skill4Evt6.m_sound = SoundDef.Explode;
		skill4.effectList.Add(skill4Evt6);

		SkillHitBoundAddEvent skill4Evt7 = new SkillHitBoundAddEvent();
		skill4Evt7.m_startTime = 0.2f;
		skill4Evt7.m_duration = 0.6f;
		skill4Evt7.m_boundIndex = 1;
		skill4Evt7.m_localPos = new Vector3(2f,2f,0f);
		skill4Evt7.m_boundSize = new Vector3(2f,4f,1f);
		skill4Evt7.m_hitData.hitTimes = 1;
		skill4Evt7.m_hitData.hitSpeed = new Vector3(3f,14f,0);
		//skill4Evt7.m_hitData.hitSound = SoundDef.FireHit;
		skill4.hitBoundList.Add(skill4Evt7);

		SkillHitBoundAddEvent skill4Evt8 = new SkillHitBoundAddEvent();
		skill4Evt8.m_startTime = 1.1f;
		skill4Evt8.m_duration = 0.6f;
		skill4Evt8.m_boundIndex = 2;
		skill4Evt8.m_localPos = new Vector3(5f,2f,0f);
		skill4Evt8.m_boundSize = new Vector3(2f,4f,1f);
		skill4Evt8.m_hitData.hitTimes = 1;
		skill4Evt8.m_hitData.hitSpeed = new Vector3(4f,12f,0);
		//skill4Evt8.m_hitData.hitSound = SoundDef.FireHit;
		skill4.hitBoundList.Add(skill4Evt8);

		SkillHitBoundAddEvent skill4Evt9 = new SkillHitBoundAddEvent();
		skill4Evt9.m_startTime = 2.2f;
		skill4Evt9.m_duration = 0.6f;
		skill4Evt9.m_boundIndex = 3;
		skill4Evt9.m_localPos = new Vector3(8f,2f,0f);
		skill4Evt9.m_boundSize = new Vector3(2f,4f,1f);
		skill4Evt9.m_hitData.hitTimes = 1;
		skill4Evt9.m_hitData.hitSpeed = new Vector3(3f,15f,0);
		//skill4Evt9.m_hitData.hitSound = SoundDef.FireHit;
		skill4.hitBoundList.Add(skill4Evt9);
		
		m_cRoleSkill.AddSkillItem(eSkillKey.SkillFour,skill4);
		
		//=========================== Attack 1 =================================//
		
		SRoleSkillItem attack1 = new SRoleSkillItem();
		attack1.skillIndex = 0;
		attack1.skillId = 4;
		attack1.durationTime = 0.44f;
		attack1.hitMethod = eHitMethod.HitCommonByNum;
		
		SkillAniChEvent attack1Evt1 = new SkillAniChEvent();
		attack1Evt1.m_aniName = AnimationNameDef.Attack1;
		attack1Evt1.m_startTime = 0f;
		attack1Evt1.m_duration = 0.44f;
		attack1.aniList.Add(attack1Evt1);
		
		SkillHitBoundAddEvent attack1Evt2 = new SkillHitBoundAddEvent();
		attack1Evt2.m_startTime = 0.1f;
		attack1Evt2.m_duration = 0.3f;
		attack1Evt2.m_localPos = new Vector3(0.5f,0.8f,0f);
		attack1Evt2.m_boundSize = new Vector3(1.8f,1.4f,1f);
		attack1Evt2.m_hitData.hitTimes = 1;
		attack1Evt2.m_hitData.hitSound = SoundDef.SWORD_HIT;
		attack1.hitBoundList.Add(attack1Evt2);
		
		SkillEffectAddEvent attack1Evt3 = new SkillEffectAddEvent();
		attack1Evt3.m_startTime = 0.1f;
		attack1Evt3.m_asset = "Assets/SourceData/Prefabs/Effect/Effect4.prefab";
		attack1Evt3.m_localPos = new Vector3(1f,1f,0f);
		attack1.effectList.Add(attack1Evt3);
		
		m_cRoleSkill.AddSkillItem(eSkillKey.Attack,attack1);
		
		//=========================== Attack 2 =================================//
		
		SRoleSkillItem attack2 = new SRoleSkillItem();
		attack2.skillIndex = 1;
		attack2.skillId = 5;
		attack2.durationTime = 0.44f;
		attack2.hitMethod = eHitMethod.HitCommonByNum;

		SkillAniChEvent attack2Evt1 = new SkillAniChEvent();
		attack2Evt1.m_aniName = AnimationNameDef.Attack2;
		attack2Evt1.m_startTime = 0f;
		attack2Evt1.m_duration = 0.33f;
		attack2.aniList.Add(attack2Evt1);
		
		SkillEffectAddEvent attack2Evt2 = new SkillEffectAddEvent();
		attack2Evt2.m_startTime = 0f;
		attack2Evt2.m_asset = "Assets/SourceData/Prefabs/Effect/Effect5.prefab";
		attack2Evt2.m_localPos = new Vector3(1f,1f,0f);
		attack2.effectList.Add(attack2Evt2);
		
		SkillHitBoundAddEvent attack2Evt3 = new SkillHitBoundAddEvent();
		attack2Evt3.m_startTime = 0f;
		attack2Evt3.m_duration = 0.3f;
		attack2Evt3.m_localPos = new Vector3(0.5f,0.8f,0f);
		attack2Evt3.m_boundSize = new Vector3(1.8f,1.4f,1f);
		attack2Evt3.m_hitData.hitTimes = 1;
		attack2Evt3.m_hitData.hitSound = SoundDef.SWORD_HIT;
		attack2.hitBoundList.Add(attack2Evt3);
		
		m_cRoleSkill.AddSkillItem(eSkillKey.Attack,attack2);

		//============================ Attack 3 ================================//

		SRoleSkillItem attack3 = new SRoleSkillItem();
		attack3.skillIndex = 2;
		attack3.skillId = 4;
		attack3.durationTime = 0.6f;
		attack3.hitMethod = eHitMethod.HitAloneByNum;
		
//		SkillAniChEvent attack3evt1 = new SkillAniChEvent();
//		attack3evt1.m_aniName = AnimationNameDef.Attack1;
//		attack3evt1.m_startTime = 0f;
//		attack3evt1.m_duration = 0.44f;
//		attack3.aniList.Add(attack3evt1);
//		
//		SkillAniChEvent attack3evt2 = new SkillAniChEvent();
//		attack3evt2.m_aniName = AnimationNameDef.Attack2;
//		attack3evt2.m_startTime = 0.45f;
//		attack3evt2.m_duration = 0.33f;
//		attack3.aniList.Add(attack3evt2);

		SkillAniChEvent attack3evt1 = new SkillAniChEvent();
		attack3evt1.m_aniName = AnimationNameDef.Skill05;
		attack3evt1.m_startTime = 0f;
		attack3evt1.m_duration = 0.6f;
		attack3.aniList.Add(attack3evt1);

		SkillHitBoundAddEvent attack3evt3 = new SkillHitBoundAddEvent();
		attack3evt3.m_startTime = 0.1f;
		attack3evt3.m_duration = 0.4f;
		attack3evt3.m_localPos = new Vector3(0.5f,0.8f,0f);
		attack3evt3.m_boundSize = new Vector3(1.8f,1.4f,1f);
		attack3evt3.m_boundIndex = 0;
		attack3evt3.m_hitData.hitTimes = 1;
		attack3evt3.m_hitData.hitSpeed = new Vector3(0,14f,0);
		attack3evt3.m_hitData.hitSound = SoundDef.HIT;
		//attack3evt3.m_hitData.hitForce = eSkillHitForce.Force_Hit;
		attack3.hitBoundList.Add(attack3evt3);

		SkillAttributeChEvent attack3evt4 = new SkillAttributeChEvent();
		attack3evt4.m_startTime = 0.5f;
		attack3evt4.m_activeChStateEnable = true;
		attack3.attributeList.Add(attack3evt4);
		
		//		SkillHitBoundAddEvent attack3evt3 = new SkillHitBoundAddEvent();
//		attack3evt3.m_startTime = 0.1f;
//		attack3evt3.m_duration = 0.3f;
//		attack3evt3.m_localPos = new Vector3(0.5f,0.8f,0f);
//		attack3evt3.m_boundSize = new Vector3(1.8f,1.4f,1f);
//		attack3evt3.m_boundIndex = 0;
//		attack3evt3.m_hitData.hitTimes = 1;
//		//attack3evt3.m_hitData.hitForce = eSkillHitForce.Force_Hit;
//		attack3.hitBoundList.Add(attack3evt3);
//		
//		SkillHitBoundAddEvent attack3evt4 = new SkillHitBoundAddEvent();
//		attack3evt4.m_startTime = 0.6f;
//		attack3evt4.m_duration = 0.3f;
//		attack3evt4.m_localPos = new Vector3(0.5f,0.8f,0f);
//		attack3evt4.m_boundSize = new Vector3(1.8f,1.4f,1f);
//		attack3evt4.m_boundIndex = 1;
//		attack3evt4.m_hitData.hitTimes = 1;
//		attack3evt4.m_hitData.hitSpeed = new Vector3(0,14f,0);
//		//attack3evt4.m_hitData.hitForce = eSkillHitForce.Force_FlyUp;
//		attack3.hitBoundList.Add(attack3evt4);
		
//		SkillEffectAddEvent attack3evt5 = new SkillEffectAddEvent();
//		attack3evt5.m_startTime = 0.1f;
//		attack3evt5.m_asset = "Assets/SourceData/Prefabs/Effect/Effect4.prefab";
//		attack3evt5.m_localPos = new Vector3(1f,1f,0f);
//		attack3.effectList.Add(attack3evt5);
		
		SkillEffectAddEvent attack3evt6 = new SkillEffectAddEvent();
		attack3evt6.m_startTime = 0.1f; //0.5f;
		attack3evt6.m_asset = "Assets/SourceData/Prefabs/Effect/Effect5.prefab";
		attack3evt6.m_localPos = new Vector3(1f,1f,0f);
		attack3.effectList.Add(attack3evt6);
		
		m_cRoleSkill.AddSkillItem(eSkillKey.Attack,attack3);

		//============================ Jump Attack1 ==========================//

		SRoleSkillItem jumpAttack1 = new SRoleSkillItem();
		jumpAttack1.skillIndex = 0;
		jumpAttack1.skillId = 4;
		jumpAttack1.durationTime = 0.44f;
		jumpAttack1.hitMethod = eHitMethod.HitCommonByNum;
		
		SkillAniChEvent jumpAttack1Evt1 = new SkillAniChEvent();
		jumpAttack1Evt1.m_aniName = AnimationNameDef.Attack1;
		jumpAttack1Evt1.m_startTime = 0f;
		jumpAttack1Evt1.m_duration = 0.44f;
		jumpAttack1.aniList.Add(jumpAttack1Evt1);

		SkillPosChEvent jumpAttack1Evt4 = new SkillPosChEvent();
		jumpAttack1Evt4.m_startTime = 0f;
		jumpAttack1Evt4.m_duration = 0.3f;
		jumpAttack1Evt4.m_skillMoveMethod = eSkillMoveMethod.Translation;
		jumpAttack1Evt4.m_motion = new Vector3(1f,0f,0f);
		jumpAttack1.posList.Add(jumpAttack1Evt4);
		
		SkillHitBoundAddEvent jumpAttack1Evt2 = new SkillHitBoundAddEvent();
		jumpAttack1Evt2.m_startTime = 0.1f;
		jumpAttack1Evt2.m_duration = 0.3f;
		jumpAttack1Evt2.m_localPos = new Vector3(0.5f,0.8f,0f);
		jumpAttack1Evt2.m_boundSize = new Vector3(2f,1.5f,1f);
		jumpAttack1Evt2.m_hitData.hitTimes = 1;
		jumpAttack1Evt2.m_hitData.hitSpeed = new Vector3(2f,6f,0);
		jumpAttack1Evt2.m_hitData.hitSound = SoundDef.SWORD_HIT;
		jumpAttack1.hitBoundList.Add(jumpAttack1Evt2);
		
		SkillEffectAddEvent jumpAttack1Evt3 = new SkillEffectAddEvent();
		jumpAttack1Evt3.m_startTime = 0.1f;
		jumpAttack1Evt3.m_asset = "Assets/SourceData/Prefabs/Effect/Effect4.prefab";
		jumpAttack1Evt3.m_localPos = new Vector3(1f,1f,0f);
		jumpAttack1.effectList.Add(jumpAttack1Evt3);
		
		m_cRoleSkill.AddSkillItem(eSkillKey.JumpAttack,jumpAttack1);

		//============================= Jump Attack2 ============================//

		SRoleSkillItem jumpAttack2 = new SRoleSkillItem();
		jumpAttack2.skillIndex = 1;
		jumpAttack2.skillId = 5;
		jumpAttack2.durationTime = 0.44f;
		jumpAttack2.hitMethod = eHitMethod.HitCommonByNum;
		
		SkillAniChEvent jumpAttack2Evt1 = new SkillAniChEvent();
		jumpAttack2Evt1.m_aniName = AnimationNameDef.Attack2;
		jumpAttack2Evt1.m_startTime = 0f;
		jumpAttack2Evt1.m_duration = 0.33f;
		jumpAttack2.aniList.Add(jumpAttack2Evt1);

		SkillPosChEvent jumpAttack2Evt2 = new SkillPosChEvent();
		jumpAttack2Evt2.m_startTime = 0f;
		jumpAttack2Evt2.m_duration = 0.3f;
		jumpAttack2Evt2.m_skillMoveMethod = eSkillMoveMethod.Translation;
		jumpAttack2Evt2.m_motion = new Vector3(1f,1f,0f);
		jumpAttack2.posList.Add(jumpAttack2Evt2);

		SkillHitBoundAddEvent jumpAttack2Evt4 = new SkillHitBoundAddEvent();
		jumpAttack2Evt4.m_startTime = 0f;
		jumpAttack2Evt4.m_duration = 0.3f;
		jumpAttack2Evt4.m_localPos = new Vector3(0.5f,0.8f,0f);
		jumpAttack2Evt4.m_boundSize = new Vector3(2f,1.5f,1f);
		jumpAttack2Evt4.m_hitData.hitTimes = 1;
		jumpAttack2Evt4.m_hitData.hitSpeed = new Vector3(2f,7f,0);
		jumpAttack2Evt4.m_hitData.hitSound = SoundDef.SWORD_HIT;
		jumpAttack2.hitBoundList.Add(jumpAttack2Evt4);

		SkillEffectAddEvent jumpAttack2Evt3 = new SkillEffectAddEvent();
		jumpAttack2Evt3.m_startTime = 0f;
		jumpAttack2Evt3.m_asset = "Assets/SourceData/Prefabs/Effect/Effect5.prefab";
		jumpAttack2Evt3.m_localPos = new Vector3(1f,1f,0f);
		jumpAttack2.effectList.Add(jumpAttack2Evt3);
		
		m_cRoleSkill.AddSkillItem(eSkillKey.JumpAttack,jumpAttack2);

		//============================== Jump Attack3 ==========================//

		SRoleSkillItem jumpAttack3 = new SRoleSkillItem();
		jumpAttack3.skillIndex = 2;
		jumpAttack3.skillId = 4;
		jumpAttack3.durationTime = 1f;
		jumpAttack3.hitMethod = eHitMethod.HitAloneByNum;
		
		SkillAniChEvent jumpAttack3evt1 = new SkillAniChEvent();
		jumpAttack3evt1.m_aniName = AnimationNameDef.Attack1;
		jumpAttack3evt1.m_startTime = 0f;
		jumpAttack3evt1.m_duration = 0.44f;
		jumpAttack3.aniList.Add(jumpAttack3evt1);
		
		SkillAniChEvent jumpAttack3evt2 = new SkillAniChEvent();
		jumpAttack3evt2.m_aniName = AnimationNameDef.Attack2;
		jumpAttack3evt2.m_startTime = 0.6f;
		jumpAttack3evt2.m_duration = 0.33f;
		jumpAttack3.aniList.Add(jumpAttack3evt2);

		SkillPosChEvent jumpAttack3Evt8 = new SkillPosChEvent();
		jumpAttack3Evt8.m_startTime = 0f;
		jumpAttack3Evt8.m_duration = 0.3f;
		jumpAttack3Evt8.m_skillMoveMethod = eSkillMoveMethod.Translation;
		jumpAttack3Evt8.m_motion = new Vector3(0.5f,0f,0f);
		jumpAttack3.posList.Add(jumpAttack3Evt8);

		SkillPosChEvent jumpAttack3Evt7 = new SkillPosChEvent();
		jumpAttack3Evt7.m_startTime = 0.4f;
		jumpAttack3Evt7.m_duration = 0.2f;
		jumpAttack3Evt7.m_skillMoveMethod = eSkillMoveMethod.Translation;
		jumpAttack3Evt7.m_motion = new Vector3(0f,2.5f,0f);
		jumpAttack3.posList.Add(jumpAttack3Evt7);

		SkillCameraEvent jumpAttack3Evt9 = new SkillCameraEvent();
		jumpAttack3Evt9.m_startTime = 0.6f;
		jumpAttack3Evt9.m_duration = 0.4f;
		jumpAttack3.cameraList.Add(jumpAttack3Evt9);
		
		SkillHitBoundAddEvent jumpAttack3evt3 = new SkillHitBoundAddEvent();
		jumpAttack3evt3.m_startTime = 0.1f;
		jumpAttack3evt3.m_duration = 0.4f;
		jumpAttack3evt3.m_localPos = new Vector3(0.5f,0.8f,0f);
		jumpAttack3evt3.m_boundSize = new Vector3(2f,1.5f,1f);
		jumpAttack3evt3.m_boundIndex = 0;
		jumpAttack3evt3.m_hitData.hitTimes = 1;
		jumpAttack3evt3.m_hitData.hitSpeed = new Vector3(0,12,0);
		jumpAttack3evt3.m_hitData.hitSound = SoundDef.SWORD_HIT;
		jumpAttack3.hitBoundList.Add(jumpAttack3evt3);
		
		SkillHitBoundAddEvent jumpAttack3evt4 = new SkillHitBoundAddEvent();
		jumpAttack3evt4.m_startTime = 0.6f;
		jumpAttack3evt4.m_duration = 0.3f;
		jumpAttack3evt4.m_localPos = new Vector3(0.5f,0.8f,0f);
		jumpAttack3evt4.m_boundSize = new Vector3(2f,1.5f,1f);
		jumpAttack3evt4.m_boundIndex = 1;
		jumpAttack3evt4.m_hitData.hitTimes = 1;
		jumpAttack3evt4.m_hitData.hitSpeed = new Vector3(13,-5f,0);
		jumpAttack3evt4.m_hitData.hitTimeScale = SkillTimeScaleMoment.HitMoment;
		jumpAttack3evt4.m_hitData.hitSound = SoundDef.SWORD_HIT;
		jumpAttack3.hitBoundList.Add(jumpAttack3evt4);
		
		SkillEffectAddEvent jumpAttack3evt5 = new SkillEffectAddEvent();
		jumpAttack3evt5.m_startTime = 0.1f;
		jumpAttack3evt5.m_asset = "Assets/SourceData/Prefabs/Effect/Effect4.prefab";
		jumpAttack3evt5.m_localPos = new Vector3(1f,1f,0f);
		jumpAttack3.effectList.Add(jumpAttack3evt5);
		
		SkillEffectAddEvent jumpAttack3evt6 = new SkillEffectAddEvent();
		jumpAttack3evt6.m_startTime = 0.6f;
		jumpAttack3evt6.m_asset = "Assets/SourceData/Prefabs/Effect/Effect5.prefab";
		jumpAttack3evt6.m_localPos = new Vector3(1f,1f,0f);
		jumpAttack3.effectList.Add(jumpAttack3evt6);

		m_cRoleSkill.AddSkillItem(eSkillKey.JumpAttack,jumpAttack3);
		
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

//	public void OnTriggerEnter(Collider other) 
//	{
//		Debug.Log(this.gameObject+" Role OnTriggerEnter! " + other.gameObject);
//		
//		if(this.gameObject.tag == "Enemy")
//		{
//			Debug.Log(" ================================= hit other "+ other.gameObject);
//			
//			RoleDataLocal target = other.gameObject.GetComponent<RoleDataLocal>();
//			
//			RoleDataLocal myself = this.gameObject.transform.parent.GetComponent<RoleDataLocal>();
//			if(myself != null && myself.GetBBData() != null)
//			{
//				myself.GetBBData().CtrlSkill.HitTargetEnter(target.GetBBData(),m_boundIndex,m_hitData);
//			}
//		}
//	}
	
}


