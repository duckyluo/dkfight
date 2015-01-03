using UnityEngine;
using System.Collections;
using Dk.Effect;

public class DkEffectTest : MonoBehaviour 
{
	public GameObject target;

	public Color color = Color.white;

	//Color.white 0.2
	//Color.red  0.7
	public float strength = 0.5f;

	private bool flashBol = false;

	private bool colorBol = false;

	private int timeCount = 0;

	void OnGUI()
	{
		if(GUI.Button(new Rect(10,10,90,40),"Flash"))
		{
			if(target == null)
			{
				return;
			}

			flashBol = !flashBol;
			if(flashBol)
			{
				colorBol = false;
			}
			changeFlash(flashBol);
		}

		if(GUI.Button(new Rect(110,10,90,40),"Color"))
		{
			if(target == null)
			{
				return;
			}

			colorBol = !colorBol;
			if(colorBol)
			{
				flashBol = false;
			}
			changeColor(colorBol);
		}
	}
	
	void Start () 
	{
	
	}
	

	void Update () 
	{
		if(colorBol)
		{
			if(timeCount == 0)
			{
				changeColor(colorBol);
				timeCount++;
			}
			else if(timeCount > 15)
			{
				colorBol = false;
				changeColor(colorBol);
				timeCount = 0;
			}
			else
			{
				timeCount++;
			}
		}
	}

	private void changeColor(bool value)
	{
		if(value)
		{
			Debug.Log("show color");
			DkColorLight.AddEffect(target,color,strength);
		}
		else
		{
			DkColorLight.RemoveEffect(target);
		}
	}

	private void changeFlash(bool value)
	{
		if(value)
		{
			Debug.Log("show flash");
			DkFlashLight.AddEffect(target,color, strength);
		}
		else
		{
			DkFlashLight.RemoveEffect(target);
		}
	}
	
	
}
