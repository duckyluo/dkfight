using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleAskStopAN : RoleAskActionNode
{
	protected RoleFsmMessage askMsg = null;

	public override void Initalize ()
	{
		this.m_name = "AskMove";
		this.m_isDebug = false;
		base.Initalize ();
	}

	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetRunTimeData.StateType == eStateType.State_Move 
		   || GetRunTimeData.StateType == eStateType.State_Skill)
		{
			if(GetRunTimeData.MoveMethod == eMoveMethod.Direction 
			   && !InputManager.HasDirectKey)
			{
				return true;
			}
			else return false;
		}
		else return false;
	}
	
	protected override void Exectue (DkBtInputParam input)
	{
		if(askMsg == null)
		{
			askMsg = new RoleFsmMessage();

			askMsg.targetId = GetRoleBBData.DataInfo.index;
			askMsg.cmdType = eCommandType.Cmd_Move;
			askMsg.moveMethod = eMoveMethod.None;
			askMsg.moveDirection = eMoveDirection.None;
			askMsg.lookDirection = GetRunTimeData.LookDirection;
			askMsg.targetPostion = GetRunTimeData.CurPos;

			GetMsgCtrl.AddLocalFsmMsg(askMsg);
		}
		else
		{
			askMsg = null;
			this.m_status = eDkBtRuningStatus.End;
		}
	}
}
