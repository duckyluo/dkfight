using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dk.Event;

public class AnimationCtrl : DkEventDispatch
{
	public const float DefaultSpeed = 1.0f;

//	public string m_defaultAniName = AnimationNameDef.Idle;
//	
//	public WrapMode m_defaultPlayMode = WrapMode.Loop;
	
	protected Animation m_animation = null;

	protected string m_curAniName = string.Empty;
	public string GetCurAniName
	{
		get{return m_curAniName;}
	}

	protected WrapMode m_curPlayMode = WrapMode.Once;

	protected float m_curSpeed = 1.0f;
		
	protected bool m_checkEnd = false;
	
	protected bool m_isDestroyed = false;

	protected bool m_inited = false;
	
	protected bool m_isPaused= false;
	public bool IsPaused
	{
		get{ return m_isPaused;}
	}
	
	public AnimationCtrl()
	{

	}

	public void Initalize(Animation animation)
	{
		m_animation = animation;
		if(m_animation != null)
		{
			m_inited = true;
		}
		else
		{
			m_inited = false;
			Debug.Log("[error] AnimationCtrl init failed");
		}
	}

	public override void Destroy()
	{
		base.Destroy();

		m_isDestroyed = true;
		m_curAniName = "";
		m_animation = null;
	}
		
	public void Update()
	{
		if(m_inited == false || m_isDestroyed)
		{return;}

		if(m_checkEnd && IsCurAniFinish)
		{
			m_checkEnd = false;
			this.DispatchEvent(new DkEvent(DkEventDef.EVENT_FINSH));
		}
	}

	public bool IsPlaying(string name)
	{
		if(HasState(name))
		{
			return m_animation.IsPlaying(name);
		}
		else return false;
	}

	public AnimationState GetCurState
	{
		get
		{
			if(m_inited)
			{
				return m_animation[m_curAniName];
			}
			else return null;
		}
	}

	public bool IsCurAniPlaying
	{
		get{return IsPlaying(m_curAniName);}
	}

	public bool IsCurAniFinish
	{
		get
		{
			if(!HasState(m_curAniName)){ return true;}
			bool playing = true;
			playing = playing && m_animation.IsPlaying(m_curAniName);
			playing = playing && m_animation[m_curAniName].time < m_animation[m_curAniName].length;
			return !playing;
		}
	}

	public bool IsCurLoopPlay
	{
		get{return m_curPlayMode == WrapMode.Loop;}
	}

	protected void checkPlayMode(WrapMode mode)
	{
		if(mode == WrapMode.Once || mode == WrapMode.ClampForever)
		{
			m_checkEnd = true;
		}
		else
		{
			m_checkEnd = false;
		}
	}

	public void Play(string name , WrapMode mode)// , bool replay)
	{
		if(m_inited == false)
		{
			return;
		}
//		else if(replay == false && IsPlaying(name))
//		{
//			return;
//		}
		
		if(HasState(name))
		{
			checkPlayMode(mode);

			m_curPlayMode = m_animation[name].wrapMode = mode;
			m_animation[name].speed = m_curSpeed;
			m_animation.Stop();
			m_animation.Play(name);
			m_curAniName = name;
		}
	}
	
	public void Pause()
	{
		if(HasState(m_curAniName))
		{
			AnimationState state = m_animation[m_curAniName];
			state.speed = 0;
			m_isPaused = true;
		}
	}

	public void SetCurSpeed(float value)
	{
		if(HasState(m_curAniName))
		{
			AnimationState state = m_animation[m_curAniName];
			state.speed = value;
		}
		m_curSpeed = value;
	}

	public void Resume()
	{
		if(HasState(m_curAniName))
		{
			AnimationState state = m_animation[m_curAniName];
			state.speed = m_curSpeed;
			m_isPaused = false;
		}
	}

//	public void Start()
//	{
//		Play(m_curAniName,m_curPlayMode,true);
//	}

	public bool HasState(string name)
	{
		if(m_inited == false || m_isDestroyed || string.IsNullOrEmpty(name))
		{
			return false;
		}

		if(m_animation == null )
		{
			Debug.Log("[error] animation is null");
			return false;
		}
		else if(m_animation[name] == null)
		{
			Debug.Log("[error] state "+ name + " can not find");
			return false;
		}
		else return true;
	}

}
