using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleInputMoveCN : RoleBaseSelectorNode
{
	public override void Initalize ()
	{
		this.m_name = "InputMove";
		this.m_isDebug = false;
		base.Initalize ();
		this.AddChild(new RoleAskMoveAN());
	}

	public override bool Evaluate (DkBtInputParam input)
	{
		if(InputManager.HasDirectKey && InputManager.IsConsumeKeyEnalbe && GetRunTimeData.MoveEnable)
		{
			if(GetRunTimeData.StateType == eStateType.State_Idle ||
			   GetRunTimeData.StateType == eStateType.State_Move ||
			   GetRunTimeData.StateType == eStateType.State_Attack)
			{
				//return true;
				if(InputManager.GetInputDirect == eInputDirect.LEFT && GetRunTimeData.MoveDirection != eMoveDirection.Left)
				{
					return true;
				}
				else if(InputManager.GetInputDirect == eInputDirect.RIGHT && GetRunTimeData.MoveDirection != eMoveDirection.Right)
				{
					return true;
				}
				else return false;
			}
			else return false;
		}
		else return false;
	}
}
