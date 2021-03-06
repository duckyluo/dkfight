using UnityEngine;
using System;

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
	State_Born = 5,
	State_Die = 6,
}

public enum eActionType
{
	Not_Use,
	None,
	Appear,
	Disappear,
	Idle,
	Shake,
	Hide,
	Move,
	Stop,
	Jump,
	JumpDown,
	Attack,
	JumpAttack,
	Skill,
	ForceHit,
	ForceFloatHit,
	ForceBack,
	ForceFly,
	ForceFallDown,
	ForceDown,
	ForceMotion,
	JoinImpact,
	JoinLie,
	JoinFallen,
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

public enum eJumpDirection
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

public class RoleSpeedDef
{
	public const float JumpXSpeed = 4f;
	public const float RunXSpeed = 2.5f;
}

public class RoleASpeedDef
{
	public const float GravityNomal = 10f;
	public const float GravityAir = 30f;
	public const float AirXResistance = 1f;
}

public class RoleHeightDef
{
	public const float JumpAtkHeight = 1f;
}

public class RoleRationDef
{
	public static Vector3 StandRotation = Vector3.zero;
	public static Vector3 FloatHitRotation = new Vector3(-30,0,0);
	public static Vector3 FallRotation = new Vector3(-70f,0,0);
	public static Vector3 LieRotation = new Vector3(-70f,0,0);
}


