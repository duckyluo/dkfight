using UnityEngine;
using System.Collections;

public class InputManager
{
//	#region singleton

//	#endregion

	private const float KEY_INPUT_CACHE_TIME = 0.5f;
	private const float KEY_INPUT_COOL_TIME = 0.25f;
	private const float KEY_CONSUME_COOL_TIME = 0f;

	private static float m_keyCacheLastTime = 0;
	private static float m_keyCoolLastTime = 0;
	private static float m_keyConsumeCoolTime = 0;

	public static bool KeyJumpEnalbe = true;

	private static eInputDirect m_direct = eInputDirect.NONE;
	public static eInputDirect GetInputDirect
	{
		get{return m_direct;}
	}

	private static eInputActiveKey m_activeKey = eInputActiveKey.None;
	public static eInputActiveKey GetInputActiveKey
	{
		get{return m_activeKey;}
	}
	
	public static void Initalize()
	{
		m_keyCacheLastTime = 0;
		m_keyCoolLastTime = 0;
		m_direct = eInputDirect.NONE;
		m_activeKey = eInputActiveKey.None;
	}
	
	public static void Update () 
	{
		updateTimeRemain();
		updateActiveKey();
		checkDirectInput();
		checkKeyInput();
	}

	private static void updateTimeRemain()
	{
		float deltaTime = Time.deltaTime;
		
		if(m_keyCacheLastTime > 0)
		{
			m_keyCacheLastTime -= deltaTime;
		}
		
		if(m_keyCoolLastTime > 0)
		{
			m_keyCoolLastTime -= deltaTime;
		}

		if(m_keyConsumeCoolTime > 0)
		{
			m_keyConsumeCoolTime -= deltaTime;
		}
	}

	private static void updateActiveKey()
	{
		if(m_keyCacheLastTime <= 0)
		{
			resetActiveKey();
		}
	}

	private static eInputDirect checkKeyboardDirectKeyInput()
	{
		eInputDirect direct = m_direct;
		if(direct == eInputDirect.LEFT)
		{
			if(!Input.GetKey(KeyCode.A))
			{
				if(Input.GetKey(KeyCode.D))
				{
					direct = eInputDirect.RIGHT;
				}
				else
				{
					direct = eInputDirect.NONE;
				}
			}
		}
		else if(direct == eInputDirect.RIGHT)
		{
			if(!Input.GetKey(KeyCode.D))
			{
				if(Input.GetKey(KeyCode.A))
				{
					direct = eInputDirect.LEFT;
				}
				else
				{
					direct = eInputDirect.NONE;
				}
			}
		}
		else
		{
			if(Input.GetKey(KeyCode.D))
			{
				direct = eInputDirect.RIGHT;
			}
			else if(Input.GetKey(KeyCode.A))
			{
				direct = eInputDirect.LEFT;
			}
		}
		return direct;
	}

	private static void checkDirectInput()
	{
		eInputDirect direct= checkKeyboardDirectKeyInput();
		if(m_direct != direct)
		{
			m_direct = direct;
		}
	}

	private static void checkKeyInput()
	{
		if(Input.GetKeyDown(KeyCode.J))
		{
			CacheKeyInput(eInputActiveKey.Attack);
		}
		else if(Input.GetKeyDown(KeyCode.Y))
		{
			CacheKeyInput(eInputActiveKey.Skill_One);
		}
		else if(Input.GetKeyDown(KeyCode.U))
		{
			CacheKeyInput(eInputActiveKey.Skill_Two);
		}
		else if(Input.GetKeyDown(KeyCode.I))
		{
			CacheKeyInput(eInputActiveKey.Skill_Three);
		}
		else if(Input.GetKeyDown(KeyCode.O))
		{
			CacheKeyInput(eInputActiveKey.Skill_Four);
		}
		else if(Input.GetKeyDown(KeyCode.Space)&&KeyJumpEnalbe)
		{
			CacheKeyInput(eInputActiveKey.Jump);
		}
	}

	private static void resetActiveKey()
	{
		m_keyCacheLastTime = 0;
		m_activeKey = eInputActiveKey.None;

	}
	
	public static void CacheKeyInput(eInputActiveKey key)
	{
		if(m_keyCoolLastTime <= 0)
		{
			m_keyCacheLastTime = KEY_INPUT_CACHE_TIME;
			m_keyCoolLastTime = KEY_INPUT_COOL_TIME;
			m_activeKey = key;
		}
	}

	public static eInputActiveKey ConsumeActiveKey()
	{
		eInputActiveKey key = eInputActiveKey.None;
		if(m_keyConsumeCoolTime <= 0 && HasActiveKey)
		{
			m_keyConsumeCoolTime = KEY_CONSUME_COOL_TIME;
			key = GetInputActiveKey;
			resetActiveKey();
		}
		return key;
	}

	public static eInputDirect ConsumeDirect()
	{
		eInputDirect key = eInputDirect.NONE;
		if(m_keyConsumeCoolTime <= 0 && HasDirectKey)
		{
			m_keyConsumeCoolTime = KEY_CONSUME_COOL_TIME;
			key = GetInputDirect;
		}
		return key;
	}

	public static bool IsConsumeKeyEnalbe
	{
		get{return (m_keyConsumeCoolTime <= 0);}
	}

	public static bool HasActiveKey
	{
		get{return (m_keyCacheLastTime > 0);}
	}

	public static bool HasDirectKey
	{
		get{return (m_direct != eInputDirect.NONE);}
	}

	public static bool HasJumpKey
	{
		get
		{
			if(m_keyCacheLastTime > 0
			   && m_activeKey == eInputActiveKey.Jump)
			{
				return true;
			}
			else return false;
		}
	}

	public static bool HasSkillKey
	{
		get
		{
			if(m_keyCacheLastTime > 0 
			   && m_activeKey != eInputActiveKey.None 
			   && m_activeKey != eInputActiveKey.Jump 
			   && m_activeKey != eInputActiveKey.Attack)
			{
				return true;
			}
			else return false;
		}
	}

	public static bool HasAttackKey
	{
		get
		{
			if(m_keyCacheLastTime > 0
			   && m_activeKey == eInputActiveKey.Attack)
			{
				return true;
			}
			else return false;
		}
	}
}
