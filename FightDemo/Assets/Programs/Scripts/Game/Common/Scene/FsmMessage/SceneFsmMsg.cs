using System;
using UnityEngine;

public enum eFsmMsgType
{
	Not_Use,
	Role,
}

public interface ISceneFsmMsg
{
	eFsmMsgType GetMsgType();
}

public class SceneFsmMsg : ISceneFsmMsg
{
	protected eFsmMsgType m_msgType = eFsmMsgType.Not_Use;
	public eFsmMsgType GetMsgType()
	{
		return m_msgType;
	}

	protected bool m_isLocalMsg = true;
	public bool IsLocalMsg
	{
		set{m_isLocalMsg = value;}
		get{return m_isLocalMsg;}
	}
}

public class RoleFsmMessage : SceneFsmMsg
{
	public int  targetId = -1;
	public float remainTime = -1f;
	public float startTime = -1f;
	public eCommandType cmdType = eCommandType.Not_Use;
	//public eStateType forceState = eStateType.Not_Use;

	public eMoveMethod moveMethod = eMoveMethod.Not_Use;
	public ePostureType postureType = ePostureType.Not_Use;
	public eLookDirection lookDirection = eLookDirection.Not_Use;
	public eMoveDirection moveDirection = eMoveDirection.Not_Use;
	public Vector3 targetPostion = Vector3.zero;
	public eUseGravity useGravity = eUseGravity.Not_Use;

	public int skillId = -1;
	public int attackerId = -1;
	public bool attackSuccess = false;
	public int hitId = -1;
	public eDamageType damageType = eDamageType.Not_Use;
	public eDamageForce damageForce = eDamageForce.Not_Use;
	
	public RoleFsmMessage()
	{
		m_msgType = eFsmMsgType.Role;
	}
}

