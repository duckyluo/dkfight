
public enum eMoveMethod
{
	Not_Use,
	None,
	Direction,
	Jump,
	RootPoint,
	Position,
	Path,
}

public enum ePostureType
{
	Not_Use,
	Pose_None = 0,
	Pose_Idle = 1,
	Pose_RUN = 2,
	Pose_Lie = 3,
	Pose_Standup = 4,
	Pose_JumpUp = 5,
	Pose_JumpFloat = 6,
	Pose_JumpDown = 7,
	Pose_JumpAttack = 8,
	Pose_Attack = 9,
	Pose_Hit = 10,
	Pose_HitBack = 11,
	Pose_HitFly = 12,
}

public enum eStateType
{
	Not_Use,
	State_None = 0,
	State_Idle = 1,
	State_Move = 2,
	//State_Attack = 2,
	State_Skill = 3,
	State_Hit = 4,
	//State_Jump = 4,
	State_Hide = 5,
	State_Die = 6,
	State_Appear = 7,
	State_Disappear = 8,

}

public enum eLookDirection
{
	Not_Use,
	None,
	Left,
	Right,
}

public enum eMoveDirection
{
	Not_Use,
	None,
	Left,
	Right,
}

public enum eTargetType
{
	Not_Use,
	None,
	Character,
	Monster,
}

public enum eUseGravity
{
	Not_Use,
	None,
	No,
	Yes,
}

public class GravityDef
{
	public const float Nomal = 10.0f;
	public const float Air = 30.0f;
}


