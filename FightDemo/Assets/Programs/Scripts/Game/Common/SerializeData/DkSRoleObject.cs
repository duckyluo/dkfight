using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DkSRoleObject : ScriptableObject 
{
	public DkSRoleData data = new DkSRoleData();

	public string assetPath = "";

	public void SetAssetPath(string path)
	{
		assetPath = path;
	}
}
