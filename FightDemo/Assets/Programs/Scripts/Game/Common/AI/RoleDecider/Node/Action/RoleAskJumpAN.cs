using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleAskJumpAN : RoleAskActionNode
{
	protected RoleFsmMessage askMsg = null;

	public override void Initalize ()
	{
		this.m_name = "AskJump";
		base.Initalize ();
	}

	public override bool Evaluate (DkBtInputParam input)
	{
		if(InputManager.ConsumeActiveKey() == eInputActiveKey.Jump)
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
			askMsg.actionType = eActionType.Jump;
			askMsg.moveMethod = eMoveMethod.Jump;
			askMsg.curPos = GetRunTimeData.CurPos;

			if(InputManager.GetInputDirect == eInputDirect.LEFT)
			{
				askMsg.lookDirection = eLookDirection.Left;
				askMsg.moveDirection = eMoveDirection.Left;
			}
			else if(InputManager.GetInputDirect == eInputDirect.RIGHT)
			{
				askMsg.lookDirection = eLookDirection.Right;
				askMsg.moveDirection = eMoveDirection.Right;
			}
			else 
			{
				askMsg.lookDirection = GetRunTimeData.LookDirection;
				askMsg.moveDirection = eMoveDirection.None;
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
