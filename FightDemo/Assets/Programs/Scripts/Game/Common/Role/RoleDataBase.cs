using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dk.Util;


public class RoleDataBase : DkSerializeData
{
	protected DkSRoleData _sRoleData = null;
	public void SetSRoleData(DkSRoleData value)
	{
		if(_sRoleData != value)
		{
			_sRoleData = value;
			this.DoSerialize(_sRoleData);
		}
	}

	public void Awake()
	{
		if(this.bytes != null && this.bytes.Length > 0)
		{
			_sRoleData = this.GetDeSerialiezeObj() as DkSRoleData;
		}
	}
}

[System.Serializable]
public class DkSRoleData
{
	public int roleId = 0;
	
	public string roleName = "Dk";
	
	public List<DkSSkillData> skillList = new List<DkSSkillData>();
}

[System.Serializable]
public class DkSSkillData
{
	public int skillId = 0;
	public float finishTime = 0f;
}