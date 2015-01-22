
public enum eMoveMethod
{
	Not_Use,
	None,
	Gravity,
	Direction,
	Jump,
	ForceSpeed,
	RootPoint,
	Position,
	Path,
	Motion,
}

public enum ePostureType
{
	Not_Use,
	Pose_None = 0,
	Pose_Appear = 1,
	Pose_Disappear = 2,
	Pose_Die = 3,

	Pose_Lie = 11,
	Pose_Standup = 12,	
	Pose_Stand = 13,
	Pose_Alert = 14,
	
	Pose_Hide = 21,
	Pose_RUN = 22,
	Pose_Escape = 23,
	Pose_JumpUp = 25,
	Pose_JumpFloat = 26,
	Pose_JumpDown = 27,

	Pose_Hit = 31,
	Pose_HitLie = 32,
	Pose_HitBack = 33,
	Pose_HitDown = 34,
	Pose_HitFlyUp = 35,
	Pose_HitFlyFloat = 36,
	Pose_HitFlyDown = 37,
	Pose_HitGround = 38,
	
	Pose_Attack = 101,
	Pose_JumpAttack = 102,
	Pose_Skill = 103,
}

public enum eStateType
{
	Not_Use,
	State_None = 0,
	State_Idle = 1,
	State_Move = 2,
	State_Attack = 3,
	State_Hit = 4,
	State_Hide = 5,
	State_Die = 6,
	State_Appear = 7,
	State_Disappear = 8,
}

public enum eActionType
{
	Not_Use,
	None,
	Idle,
	Move,
	Stop,
	Jump,
	JumpDown,
	Attack,
	JumpAttack,
	Skill,
	Die,
	ForceHit,
	ForceFloatHit,
	//ForceFloatDown,
	ForceBack,
	ForceFly,
	ForceFallDown,
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


