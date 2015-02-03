using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager
{
	protected Dictionary<CameraDef,CameraUIS> m_cameraDict = new Dictionary<CameraDef, CameraUIS>();

	protected Dictionary<CameraMode,List<CameraDef>> m_modeDict = new Dictionary<CameraMode,List<CameraDef>>();

	protected CameraMode m_curMode = CameraMode.Not_Use;

	protected List<CameraUIS> m_curCameras = new List<CameraUIS>();
	
	private static CameraManager s_instance = null;
	
	public static CameraManager Instance
	{
		get
		{
			if (s_instance == null)
			{
				s_instance = new CameraManager();
			}
			
			return s_instance;
		}
	}

	public void Initalize()
	{
		List<CameraDef> sceneMode = new List<CameraDef>{CameraDef.Scene, CameraDef.SceneBg};
		m_modeDict.Add(CameraMode.SceneMode,sceneMode);

		List<CameraDef>  playerMode = new List<CameraDef>{CameraDef.Player, CameraDef.SceneBg};
		m_modeDict.Add(CameraMode.PlayerMode,playerMode);

		ChangeCurMode(CameraMode.SceneMode);
	}
	
	public void destroy()
	{
		m_cameraDict.Clear();
	}
	
	public void addCamera(CameraUIS value)
	{
		if(m_cameraDict.ContainsKey(value.type) == false)
		{
			m_cameraDict.Add(value.type,value);
		}
		else
		{
			Debug.Log("camera is add again! type : "+value.type);
		}
	}
	
	public CameraUIS getCamera(CameraDef type)
	{
		if(m_cameraDict.ContainsKey(type))
		{
			return m_cameraDict[type];
		}
		else return null;
	}
	
	public void removeCamera(CameraDef type)
	{
		m_cameraDict.Remove(type);
	}

	public void ChangeCurMode(CameraMode mode)
	{
		if(m_curMode != mode)
		{
			m_curMode = mode;
			UpdateCurMode();
			UpdateFollow();
		}
	}
	
	protected void UpdateCurMode()
	{
		List<CameraDef> mode = null;

		m_curCameras.Clear();
		if(m_modeDict.TryGetValue(m_curMode,out mode))
		{
			foreach(KeyValuePair<CameraDef,CameraUIS> pair in m_cameraDict)
			{
				if(mode.Contains(pair.Key))
				{
					pair.Value.SetCameraEnalbe(true);
					m_curCameras.Add(pair.Value);
				}
				else
				{
					pair.Value.SetCameraEnalbe(false);
				}
			}
		}
		else
		{
			Debug.LogError("Can not find camera mode "+m_curMode);
		}
	}

	protected void UpdateFollow()
	{
		foreach(CameraUIS item in m_curCameras)
		{
			if(item.type == CameraDef.Player)
			{
				GameObject obj = TagDef.getObjectByTag(TagDef.Me);
				item.StartFollow(obj,CameraFollowMode.Free);
			}
			else if(item.type == CameraDef.Scene)
			{
				GameObject obj = TagDef.getObjectByTag(TagDef.Me);
				item.StartFollow(obj,CameraFollowMode.Limit);
			}
		}
	}
}
