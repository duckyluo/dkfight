using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleNotInputCN : RoleBaseSelectorNode
{
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetRunTimeData.StateType == eStateType.Not_Use 
		   || GetRunTimeData.StateType == eStateType.State_None)
		{
			return false;
		}
		else if(!InputManager.HasDirectKey || !InputManager.HasActiveKey)
		{
			return true;
		}
		else return false;
	}
	
	public override void Initalize ()
	{
		base.Initalize ();
		this.AddChild(new RoleAskStopAN());
	}
}
