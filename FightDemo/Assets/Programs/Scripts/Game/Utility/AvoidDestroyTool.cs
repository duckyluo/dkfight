using UnityEngine;

public class AvoidDestroyTool : MonoBehaviour
{
	void Awake()
	{
		DontDestroyOnLoad(this);
	}
}

