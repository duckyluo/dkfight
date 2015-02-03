using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CRoleSkill
{
	protected Dictionary<eSkillKey,CRoleSkillGroup> m_skillDict = new Dictionary<eSkillKey, CRoleSkillGroup>();

	public void SetSkillData(SRoleSkillData sdata)
	{
		m_skillDict.Clear();
		AddSkillByList(eSkillKey.Attack, sdata.attackList , sdata.skillList);
		AddSkillByList(eSkillKey.JumpAttack, sdata.jumpAttackList , sdata.skillList);
		AddSkillByList(eSkillKey.SkillOne, sdata.skillOneList , sdata.skillList);
		AddSkillByList(eSkillKey.SkillTwo, sdata.skillTwoList , sdata.skillList);
		AddSkillByList(eSkillKey.SkillThree, sdata.skillThreeList , sdata.skillList);
		AddSkillByList(eSkillKey.SkillFour, sdata.skillFourList , sdata.skillList);
	}
	
	protected void AddSkillByList(eSkillKey key , List<int> indexList , List<SRoleSkillItem> skillList)
	{
		foreach(int index in indexList)
		{
			if(skillList.Count > index)
			{
				SRoleSkillItem item = skillList[index];
				AddSkillItem(key,item);
			}
			else
			{
				Debug.Log("[error][CRoleSkill SetSkill] can not find role "+key+" skill index "+index);
			}
		}
	}

	public void AddSkillItem(eSkillKey key, SRoleSkillItem skillItem)
	{
		CRoleSkillGroup group = null;
		if(!m_skillDict.TryGetValue(key,out group))
		{
			group = new CRoleSkillGroup();
			m_skillDict.Add(key,group);
		}
		group.AddSkill(skillItem);
	}

	public CRoleSkillGroup GetSkillGroup(eSkillKey key)
	{
		CRoleSkillGroup group = null;
		m_skillDict.TryGetValue(key,out group);
		if(group == null)
		{
			group = new CRoleSkillGroup();
			m_skillDict.Add(key,group);
		}
		return group;
	}
}

public class CRoleSkillGroup
{
	protected int m_curIndex = -1;

	public List<CRoleSkillItem> skillList = new List<CRoleSkillItem>();

	public void AddSkill(SRoleSkillItem sItem)
	{
		CRoleSkillItem cItem = new CRoleSkillItem(sItem);
		skillList.Add(cItem);
		skillList.Sort();
	}

	public int NextSkillIndex()
	{
		if(skillList.Count > 0)
		{
			if((m_curIndex + 1) < skillList.Count)
			{
				return m_curIndex + 1;
			}
			else return 0;
		}
		else return -1;
	}
	
	public CRoleSkillItem MoveToIndex(int index)
	{
		if(index >= 0 && index < skillList.Count)
		{
			m_curIndex = index;
			return skillList[m_curIndex];
		}
		else return null;
	}

	public bool IsLastIndex
	{
		get
		{
			if(m_curIndex == (skillList.Count - 1))
			{
				return true;
			}
			else return false;
		}
	}

	public void Reset()
	{
		m_curIndex = -1;
	}
}

public class CRoleSkillItem : IComparable
{
	public int skillIndex = -1;
	public int skillId = -1;
	public float durationTime = -1;
	public eHitMethod hitMethod = eHitMethod.Not_Use;

	public List<SkillProcessEvent> skillEvents = new List<SkillProcessEvent>();
	
	public CRoleSkillItem(SRoleSkillItem sItem)
	{
		skillIndex = sItem.skillIndex;
		skillId = sItem.skillId;
		durationTime = sItem.durationTime;
		hitMethod = sItem.hitMethod;

		AddEventFromList(sItem.aniList);
		AddEventFromList(sItem.posList);
		AddEventFromList(sItem.alphaList);
		AddEventFromList(sItem.scaleList);
		AddEventFromList(sItem.attributeList);
		AddEventFromList(sItem.effectList);
		AddEventFromList(sItem.hitBoundList);
		AddEventFromList(sItem.magicList);
		AddEventFromList(sItem.cameraList);

		skillEvents.Sort();
	}

	private void AddEventFromList(IEnumerable list)
	{
		foreach(SkillProcessEvent item in list)
		{
			skillEvents.Add(item);
		}
	}

	public int CompareTo(object obj)
	{
		int res = 0;
		try
		{
			CRoleSkillItem item = (CRoleSkillItem)obj;
			if(this.skillIndex > item.skillIndex)
			{
				res = 1;
			}
			else if(this.skillIndex < item.skillIndex)
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
}