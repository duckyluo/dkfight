using UnityEngine;


public class UisSoundObj : MonoBehaviour
{
	public SoundDef soundName = SoundDef.Not_Use;

	public float delay = 0f;

	protected AudioSource m_audioSource = null;
	
	public void Awake()
	{
		m_audioSource = this.GetComponent<AudioSource>();
		if(m_audioSource == null || soundName == SoundDef.Not_Use)
		{
			Debug.LogError("Sound is Forgot to set!");
		}
		else
		{
			SoundManager.AddSound(soundName,this);
		}
	}

	public void PlaySound()
	{
		m_audioSource.PlayDelayed(delay);
	}

	public bool IsPlaying()
	{
		return m_audioSource.isPlaying;
	}

}

