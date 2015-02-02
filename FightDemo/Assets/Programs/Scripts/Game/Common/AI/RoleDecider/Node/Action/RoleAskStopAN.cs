using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleAskStopAN : RoleAskAction
{
	protected RoleFsmMessage askMsg = null;

	public override void Initalize ()
	{
		this.m_name = "AskStop";
		//this.m_isDebug = false;
		base.Initalize ();
	}

	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetRunTimeData.ActionType == eActionType.Move && !InputManager.HasDirectKey)
		{
			return true;
		}
		else return false;
	}

	protected override void Enter (DkBtInputParam input)
	{
		this.m_isDebug = true;
		base.Enter (input);
	}
	
	protected override void Exectue (DkBtInputParam input)
	{
		if(askMsg == null)
		{
			askMsg = new RoleFsmMessage();

			askMsg.receiveIndex = GetRoleBBData.DataInfo.index;
			askMsg.cmdType = eCommandType.Cmd_Move;
			askMsg.actionType = eActionType.Stop;
			askMsg.moveMethod = eMoveMethod.Gravity;
			askMsg.lookDirection = GetRunTimeData.LookDirection;
			askMsg.curPos = GetRunTimeData.CurPos;

			GetMsgCtrl.AddLocalFsmMsg(askMsg);
		}
		else
		{
			askMsg = null;
			this.m_status = eDkBtRuningStatus.End;
		}
	}

	public override void Finish ()
	{
		this.m_isDebug = false;
		base.Finish ();
	}
}
