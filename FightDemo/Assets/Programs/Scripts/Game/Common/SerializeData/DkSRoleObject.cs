using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DkSRoleObject : ScriptableObject 
{
	public SRoleData data = new SRoleData();

	public string assetPath = "";

	public void SetAssetPath(string path)
	{
		assetPath = path;
	}
}
