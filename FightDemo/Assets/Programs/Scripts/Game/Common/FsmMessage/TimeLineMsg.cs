using System;
using UnityEngine;

public enum eCommandType
{
	Not_Use,
	Cmd_Move = 1,
	Cmd_Attack = 2,
	Cmd_Hit = 3,
	Cmd_Born = 4,
	Cmd_Die = 5,
	Cmd_Action = 6,
}

public class TimeLineMessage
{
	public float startTime = -1f;
	
	public float remainTime = -1f;
	
	protected eCommandType m_cmdType = eCommandType.Not_Use;
	public eCommandType GetCmdType
	{
		set{m_cmdType=value;}
		get{return m_cmdType;}
	}

	protected eActionType m_actionType = eActionType.Not_Use;
	public eActionType GetActionType
	{
		set{m_actionType=value;}
		get{return m_actionType;}
	}

	protected Vector3 m_curPos = Vector3.zero;
	public Vector3 GetCurPos
	{
		set{m_curPos=value;}
		get{return m_curPos;}
	}

	protected eLookDirection m_lookDirection = eLookDirection.Not_Use;
	public eLookDirection GetLookDirection
	{
		set{m_lookDirection=value;}
		get{return m_lookDirection;}
	}

	public virtual void Init(RoleFsmMessage fsmMsg)
	{
		m_actionType = fsmMsg.actionType;
		m_curPos = fsmMsg.curPos;
		m_lookDirection = fsmMsg.lookDirection;
	}

	public virtual void Destroy()
	{
	}

	public override string ToString ()
	{
		string str = "";
		str += " cmd "+m_cmdType;
		return str;
	}

	public static TimeLineMessage GetTLMsgByFsmMsg(RoleFsmMessage fsmMsg)
	{
		TimeLineMessage msg = null;
		switch(fsmMsg.cmdType)
		{
		case eCommandType.Cmd_Move:
			msg = new TMoveMessage();
			break;
		case eCommandType.Cmd_Attack:
			msg = new TAttackMessage();
			break;
		case eCommandType.Cmd_Hit:
			msg = new THitMessage();
			break;
		case eCommandType.Cmd_Action:
			msg = new TActionMessage();
			break;
		}

		if(msg != null)
		{
			msg.Init(fsmMsg);
		}
		else 
		{
			Debug.LogError("[TimeLineMessage][GetTLMsgByFsmMsg] have error ");
		}
		return msg;
	}
}

//============================================================================//

public class TMoveMessage : TimeLineMessage
{
	public eMoveMethod moveMethod = eMoveMethod.Not_Use;
	public eJumpDirection jumpDirection = eJumpDirection.Not_Use;
	
	public TMoveMessage()
	{
		m_cmdType = eCommandType.Cmd_Move;
	}

	public override void Init(RoleFsmMessage fsmMsg)
	{
		base.Init(fsmMsg);
		moveMethod = fsmMsg.moveMethod;
		jumpDirection = fsmMsg.jumpDirection;
	}
}

public class TAttackMessage : TimeLineMessage
{
	public eSkillKey skillKey = eSkillKey.Not_Use;
	public int skillIndex = -1;

	public TAttackMessage()
	{
		m_cmdType = eCommandType.Cmd_Attack;
	}

	public override void Init(RoleFsmMessage fsmMsg)
	{
		base.Init(fsmMsg);
		skillKey = fsmMsg.skillKey;
		skillIndex = fsmMsg.skillIndex;
	}
}

public class THitMessage : TimeLineMessage
{
	public eDamageType damageType = eDamageType.Not_Use;
	public int hitId = -1;
	public Vector3 hitSpeed = Vector3.zero;

	//public eHitResultType hitResultType = eHitResultType.Not_Use;
	//public eDamageForce damageForce = eDamageForce.Not_Use;

	public THitMessage()
	{
		m_cmdType = eCommandType.Cmd_Hit;
	}

	public override void Init(RoleFsmMessage fsmMsg)
	{
		base.Init(fsmMsg);
		damageType = fsmMsg.damageType;
		hitId = fsmMsg.hitId;
		hitSpeed = fsmMsg.hitSpeed;
	}
}

public class TActionMessage : TimeLineMessage
{
	public TActionMessage()
	{
		m_cmdType = eCommandType.Cmd_Action;
	}

	public override void Init(RoleFsmMessage fsmMsg)
	{
		base.Init(fsmMsg);
	}
}

//=============================================================//

public class TNextMessage : TimeLineMessage
{
	public Vector3 hitSpeed = Vector3.zero;
	public eImpactMothod impactMothod = eImpactMothod.Not_Use;
}