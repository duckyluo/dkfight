using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dk.Event;

public class AnimationCtrl : DkEventDispatch
{
	public static float GameSpeed = 1.0f;

	protected Animation m_animation = null;

	protected string m_curAniName = string.Empty;
	public string GetCurAniName
	{
		get{return m_curAniName;}
	}
	
	protected WrapMode m_curPlayMode = WrapMode.Once;

	protected float m_curAniSpeed = 1.0f;
		
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
			return GetState(m_curAniName);
		}
	}

	public AnimationState GetState(string animationName)
	{
		if(HasState(animationName))
		{
			return m_animation[animationName];
		}
		else return null;
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

	public void Play(string name , WrapMode mode, float speed)// , bool replay)
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
			m_animation[name].speed = (speed*GameSpeed);
			m_animation.Stop();
			m_animation.Play(name);
			m_curAniName = name;
			m_curAniSpeed = speed;
		}
	}

	public void SampleCurAtTime(float time)
	{
		if(GetCurState.length < time)
		{
			time = GetCurState.length;
		}
		GetCurState.time = time;
		GetCurState.enabled = true;
		m_animation.Sample();
	}

	public void SetGameSpeed(float value)
	{
		GameSpeed = value;
		if(HasState(m_curAniName))
		{
			AnimationState state = m_animation[m_curAniName];
			state.speed = m_curAniSpeed*GameSpeed;
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
	
	public void Resume()
	{
		if(HasState(m_curAniName))
		{
			AnimationState state = m_animation[m_curAniName];
			state.speed = m_curAniSpeed*GameSpeed;
			m_isPaused = false;
		}
	}
	
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
