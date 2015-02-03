using System;
using System.Collections;
using System.Collections.Generic;

public enum CameraDef
{
	Not_Use = 0,
	None = 1,
	Default = 2,
	Player = 3,
	Scene = 4,
	SceneBg = 5,
//	MapFront,
//	MapMid,
//	MapBg,
//	Portrait,
}

public enum CameraMode
{
	Not_Use,
	None,
	SceneMode,
	PlayerMode,
}

public enum CameraFollowMode
{
	Not_Use,
	None,
	Free,
	Limit,
}
