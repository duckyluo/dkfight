using System;
using UnityEngine;

public enum eCommandType
{
	Not_Use,
	Cmd_Move = 1,
	Cmd_Skill = 2,
	Cmd_Hit = 3,
	Cmd_Appear = 4,
	Cmd_Disapper = 5,
	Cmd_Die = 6,
}

public class TimeLineMessage
{
	public float startTime = -1f;
	
	public float remainTime = -1f;
	
	protected eCommandType m_cmdType = eCommandType.Not_Use;
	public eCommandType GetCmdType
	{
		set{m_cmdType = value;}
		get{return m_cmdType;}
	}

//	protected eStateType m_forceState = eStateType.Not_Use;
//	public eStateType GetFroceState
//	{
//		set{m_forceState = value;}
//		get{return m_forceState;}
//	}
	
	protected bool m_isDataUsed = false;
	public bool IsDataUsed
	{
		set{m_isDataUsed = value;}
		get{return m_isDataUsed;}
	}
	
	public virtual void Init(RoleFsmMessage fsmMsg)
	{
	}

	public virtual void Destroy()
	{
	}

	public override string ToString ()
	{
		string str = "";
		str += " cmd "+m_cmdType;
		//str += " , fState "+m_forceState;
		return str;
	}
	
	/// <summary>
	/// Gets the TL message by fsm message.
	/// </summary>
	/// <returns>TimeLineMessage</returns>
	/// <param name="fsmMsg">Fsm message.</param>
	public static TimeLineMessage GetTLMsgByFsmMsg(RoleFsmMessage fsmMsg)
	{
		TimeLineMessage msg = null;
		switch(fsmMsg.cmdType)
		{
		case eCommandType.Cmd_Move:
			msg = new TMoveMessage();
			msg.Init(fsmMsg);
			break;
		}
		return msg;
	}
}

/// <summary>
/// T move message.
/// </summary>
public class TMoveMessage : TimeLineMessage
{
	public eMoveMethod moveMethod = eMoveMethod.Not_Use;
	public eMoveDirection moveDirection = eMoveDirection.Not_Use;
	public eLookDirection lookDirection = eLookDirection.Not_Use;
	public ePostureType posture = ePostureType.Not_Use;
	public Vector3 desPos = Vector3.zero;

	public TMoveMessage()
	{
		m_cmdType = eCommandType.Cmd_Move;
	}

	public override void Init(RoleFsmMessage fsmMsg)
	{
		moveMethod = fsmMsg.moveMethod;
		moveDirection = fsmMsg.moveDirection;
		lookDirection = fsmMsg.lookDirection;
		posture = fsmMsg.postureType;
		desPos = fsmMsg.targetPostion;
	}
}

/// <summary>
/// T skill message.
/// </summary>
public class TSkillMessage : TimeLineMessage
{
	public eLookDirection lookDirection = eLookDirection.Not_Use;
	public ePostureType posture = ePostureType.Not_Use;
	public Vector3 desPos = Vector3.zero;
	public int skillId = -1;
	public int attackId = -1;

	public TSkillMessage()
	{
		m_cmdType = eCommandType.Cmd_Skill;
	}

	public override void Init(RoleFsmMessage fsmMsg)
	{
		
	}
}

/// <summary>
/// T hit message.
/// </summary>
public class THitMessage : TimeLineMessage
{
	public int skillId = -1;
	public int attackId = -1;
	public eHitResultType hitResultType = eHitResultType.Not_Use;
	public eDamageType damageType = eDamageType.Not_Use;
	public eDamageForce damageForce = eDamageForce.Not_Use;

	public THitMessage()
	{
		m_cmdType = eCommandType.Cmd_Hit;
	}

	public override void Init(RoleFsmMessage fsmMsg)
	{
		
	}
}