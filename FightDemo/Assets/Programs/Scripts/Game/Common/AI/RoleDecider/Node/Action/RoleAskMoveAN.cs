using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleAskMoveAN : RoleAskAction
{
	protected RoleFsmMessage askMsg = null;

	public override void Initalize ()
	{
		this.m_name = "AskMove";
		//this.m_isDebug = false;
		base.Initalize ();
	}

	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetRunTimeData.IsGround)
		{
			return true;
		}
		else return false;
	}
	
	protected override void Exectue (DkBtInputParam input)
	{
		if(askMsg == null)
		{
			askMsg = new RoleFsmMessage();

			askMsg.receiveIndex = GetRoleBBData.DataInfo.index;
			askMsg.cmdType = eCommandType.Cmd_Move;
			askMsg.actionType = eActionType.Move;
			askMsg.moveMethod = eMoveMethod.Direction;
			askMsg.curPos = GetRunTimeData.CurPos;

			eInputDirect inputDirect = InputManager.ConsumeDirect();
			
			if(inputDirect == eInputDirect.LEFT)
			{
				askMsg.lookDirection = eLookDirection.Left;
			}
			else if(inputDirect == eInputDirect.RIGHT)
			{
				askMsg.lookDirection = eLookDirection.Right;
			}

			GetMsgCtrl.AddLocalFsmMsg(askMsg);
		}
		else
		{
			askMsg = null;
			this.m_status = eDkBtRuningStatus.End;
		}
	}
	
}

