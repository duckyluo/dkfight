using System;
using UnityEngine;

public class SceneObj : IFsmReceiver
{
	
	protected SceneObjInfo m_info = null;
	public SceneObjInfo Info
	{
		get{ return m_info;}
	}

	protected bool m_inited = false;
	public bool Inited
	{
		get{return Inited;}
	}

	public SceneObj(SceneObjInfo info)
	{
		m_info = info;
	}


	public virtual void Destroy(){}

	public virtual void Initalize(){}

	public virtual void Start(){}

	public virtual void Update(){}

	public virtual void OnFsmReceive(IFsmMsg msg)
	{
	}
}


public class SceneObjInfo
{
	public int index = 0;
	public int prefabId = 0;
	public eSceneObjType type = eSceneObjType.Role;
	public eSceneTeamType team = eSceneTeamType.Not_Use;
	public Vector3 initPos = Vector3.zero;
	//public bool isMe = true;
	public string nick = string.Empty;
}

