using UnityEngine;
using System.Collections;

public class UisUvEffectObj : MonoBehaviour 
{
	public enum eEffectWrapMode
	{
		ONCE = 0,
		LOOP,
	}

	protected enum eEffectRunStatus
	{
		None,
		Start,
		Run,
		End,
	}

	public eEffectWrapMode AnimationWrapMode = eEffectWrapMode.ONCE;

	public int CountX = 1;
	public int CountY = 1;
	
	public int StartIndex = 0;
	public int EndIndex = 0;

	public float DurationTime = 1f;
	public float DelayTime = 0f;

	public bool DestroyAtEnd = true;
	public bool RandomStart = false;
	public bool PauseEffect = false;
	
	protected Material m_material;

	protected float m_timeCount = 0f;
	protected int m_curIndex = 0;
	protected Vector2 m_curScale = Vector2.one;
	protected int m_posX = 0;
	protected int m_posY = 0;

	protected eEffectRunStatus m_status = eEffectRunStatus.None;

	protected bool m_isFinish = false;
	
	void Awake () 
	{
		m_material = renderer.material;
		
		Vector2 textureScale = m_material.mainTextureScale;
		m_curScale.x  = textureScale.x / CountX;
		m_curScale.y  = textureScale.y / CountY;
		
		m_material.mainTextureScale = m_curScale;

	}
	
	void Start()
	{
		if( RandomStart )
		{
			m_curIndex = Random.Range( StartIndex, EndIndex );
		}
		else
		{
			m_curIndex = StartIndex;
		}

		UpdatePos();

		if(this.DurationTime > 0)
		{
			this.m_status = eEffectRunStatus.Start;
		}
	}
	
	void Update () 
	{
		if(this.m_status == eEffectRunStatus.Start)
		{
			m_timeCount += Time.deltaTime;
			if(m_timeCount >= DelayTime)
			{
				this.m_status = eEffectRunStatus.Run;
				m_timeCount = 0f;
			}
		}
		else if(m_status != eEffectRunStatus.Run || PauseEffect)
		{
			return;
		}

		m_timeCount += Time.deltaTime;

		int stepCount = Mathf.FloorToInt((EndIndex - StartIndex + 1)* m_timeCount / DurationTime);

		if( AnimationWrapMode == eEffectWrapMode.ONCE)
		{
			m_curIndex = stepCount + StartIndex;
			if(m_curIndex > EndIndex)
			{
				this.m_status = eEffectRunStatus.End;
				if(this.DestroyAtEnd)
				{
					DestroyImmediate(this.gameObject);
				}
				return;
			}
		}
		else
		{
			m_curIndex = stepCount % (EndIndex - StartIndex + 1) + StartIndex; 
		}


		UpdatePos();
	}
	
	protected void UpdatePos()
	{
		m_posX = m_curIndex % CountX;
		m_posY = m_curIndex / CountX;
		
		UpdateOffSet();
	}
	
	protected void UpdateOffSet()
	{
		Vector2 offect = new Vector2( m_posX * m_curScale.x, 1 - (m_posY * m_curScale.y + m_curScale.y ));
		if( m_material != null)
		{
			m_material.mainTextureOffset = offect;
		}
	}
}
