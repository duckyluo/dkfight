using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleAskJumpAN : RoleAskAction
{
	protected RoleFsmMessage askMsg = null;

	public override void Initalize ()
	{
		this.m_name = "AskJump";
		base.Initalize ();
	}

	public override bool Evaluate (DkBtInputParam input)
	{
		if(InputManager.HasJumpKey)
		{
			return true;
		}
		else return false;
	}
	
	protected override void Exectue (DkBtInputParam input)
	{
		if(askMsg == null)
		{
			if(InputManager.ConsumeActiveKey() != eInputActiveKey.Jump)
			{
				Debug.Log("[error][RoleAskJumpAN] the input thing is error");
				this.m_status = eDkBtRuningStatus.End;
				return;
			}

			askMsg = new RoleFsmMessage();
			askMsg.receiveIndex = GetRoleBBData.DataInfo.index;
			askMsg.cmdType = eCommandType.Cmd_Move;
			askMsg.actionType = eActionType.Jump;
			askMsg.moveMethod = eMoveMethod.Jump;
			askMsg.curPos = GetRunTimeData.CurPos;

			if(InputManager.GetInputDirect == eInputDirect.LEFT)
			{
				askMsg.lookDirection = eLookDirection.Left;
				askMsg.jumpDirection = eJumpDirection.Left;
			}
			else if(InputManager.GetInputDirect == eInputDirect.RIGHT)
			{
				askMsg.lookDirection = eLookDirection.Right;
				askMsg.jumpDirection = eJumpDirection.Right;
			}
			else 
			{
				askMsg.lookDirection = GetRunTimeData.LookDirection;
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
