using System;
using UnityEngine;

public enum eFsmMsgType
{
	Not_Use,
	Role,
}

public interface IFsmMsg
{
	eFsmMsgType GetMsgType();
}

public class FsmMsg : IFsmMsg
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

public class RoleFsmMessage : FsmMsg
{
	public int  receiveIndex = -1;
	public float remainTime = -1f;
	public float startTime = -1f;

	public eCommandType cmdType = eCommandType.Not_Use;
	public eActionType actionType = eActionType.Not_Use;
	public Vector3 curPos = Vector3.zero;
	public eLookDirection lookDirection = eLookDirection.Not_Use;

	public eMoveMethod moveMethod = eMoveMethod.Not_Use;
	public eJumpDirection jumpDirection = eJumpDirection.Not_Use;

	public eSkillKey skillKey = eSkillKey.Not_Use;
	public int skillIndex = -1;
	
	public eDamageType damageType = eDamageType.Not_Use;
	public int hitId = -1;
	public Vector3 hitSpeed = Vector3.zero;
	
	public RoleFsmMessage()
	{
		m_msgType = eFsmMsgType.Role;
	}
}

