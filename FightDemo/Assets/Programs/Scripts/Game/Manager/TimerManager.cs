using UnityEngine;
using System.Collections;

public class TimerManager : MonoBehaviour 
{
	public enum eTimerManagerRuningMode
	{
		Stop,
		Start,
		Runing,
		Pause,
	}

	#region singleton
	private static TimerManager s_instance = null;
	
	public static TimerManager Instance
	{
		get
		{
			if(!s_instance)
			{
				s_instance = GameObject.FindObjectOfType(typeof(TimerManager)) as TimerManager;
				if (!s_instance)
				{
					GameObject container = new GameObject();
					container.name = "TimerManager";
					s_instance = container.AddComponent(typeof(TimerManager)) as TimerManager;
				}
			}
			return s_instance;
		}
	}
	#endregion

	protected eTimerManagerRuningMode curMode = eTimerManagerRuningMode.Stop;

	public void Initalize()
	{
		//DkTimer.Reset();
		ChangeRuningMode(eTimerManagerRuningMode.Start);
	}

	protected void Start () 
	{
		DontDestroyOnLoad(this);
	}

	protected void Update () 
	{
		switch(curMode)
		{
		case eTimerManagerRuningMode.Start:
			DkTimer.Start();
			curMode = eTimerManagerRuningMode.Runing;
			break;
		case eTimerManagerRuningMode.Runing:
			DkTimer.Update();
			InputManager.Update();
			SceneManager.Update();
			break;
		case eTimerManagerRuningMode.Pause:
			DkTimer.Pause();
			break;
		case eTimerManagerRuningMode.Stop:
			break;
		}
	}

	public void ChangeRuningMode(eTimerManagerRuningMode mode)
	{
		if(curMode != mode)
		{
			curMode = mode;
		}
	}

	public float GetDeltaTime
	{
		get{return DkTimer.DeltaTime;}
	}

	public float GetElapsedTime
	{
		get{return DkTimer.ElapsedTime;}
	}
}
