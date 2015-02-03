using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TagDef
{
	public const string ROOT_MAPS = "RootMaps";
	public const string ROOT_OBJECTS = "RootObjects";
	public const string ROOT_MANAGERS = "RootManagers";
	public const string ROOT_MAPCAMERAS = "RootMapCameras";

	public const string CAMERA_FRONT = "CameraFront";
	public const string CAMERA_MAIN = "CameraMain";
	public const string CAMERA_Bg = "CameraBg";
	public const string MAP_COLLIDER = "MapCollision";

	public const string Me = "Me";
	public const string Enemy = "Enemy";
	
	public static Dictionary<string,GameObject> dict = new Dictionary<string, GameObject>();
	
	public static GameObject getObjectByTag(string tag)
	{
		if(dict.ContainsKey(tag))
		{
			return dict[tag];
		}
		else
		{
			GameObject obj = GameObject.FindGameObjectWithTag(tag);
			if(obj != null)
			{
				dict.Add(tag,obj);
			}
			return obj;
		}
	}
}