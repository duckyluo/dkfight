using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Dk.Effect
{
	public class DkColorLight : MonoBehaviour
	{
		public Color color = Color.white;
		
		public float strength = 0.3f;
		
		private Shader newShader = null;
			
		private bool init = false;
		
		private bool destroyed = false;

		public static void AddEffect(GameObject obj , Color color)
		{
			DkColorLight.AddEffect(obj,color,0.3f);
		}
		
		public static void AddEffect(GameObject obj , Color color , float strength)
		{
			DkColorLight effect = obj.GetComponent<DkColorLight>();
			if(effect == null)
			{
				effect = obj.AddComponent<DkColorLight>();
			}
			
			if(effect.Initalize())
			{
				effect.color = color;
				effect.strength = strength;
				effect.EnableRun(true);
			}
		}
		
		public static void RemoveEffect(GameObject obj)
		{
			DkColorLight effect = obj.GetComponent<DkColorLight>();
			
			if(effect != null)
			{
				effect.Destroy();
				UnityEngine.Object.Destroy(effect);
			}
		}
			
		public void Destroy()
		{
			if(destroyed)
			{
				return; 
			}
			destroyed = true;
			EnableRun(false);
			newShader = null;
		}
		
		public bool Initalize()
		{
			if(init == false)
			{
				newShader = Shader.Find("Dk/DkColorLight");
				
				if(newShader == null)
				{
					Debug.Log("[warn] Shader :Dk/DkColorLight cannot find");
					init = false;
				}
				else
				{
					init = true;
				}
			}
			
			return init;
		}
		
		private void EnableRun(bool value)
		{
			if(!init)
			{
				return;
			}

			changeAllShader(value);
		}
		
		private void changeAllShader(bool change)
		{
			Dictionary<Material,Shader> dict;
			
			if(change)
			{
				if(DkEffect.GetObjectEffectType(this.gameObject) == eDkEffectType.Color_Light
				   || newShader == null)
				{
					return;
				}
				
				DkEffect.AddObjectMat(this.gameObject, eDkEffectType.Color_Light);
				
				dict = DkEffect.GetObjectMat(this.gameObject);
				
				foreach(KeyValuePair<Material,Shader> pair in dict)
				{
					Material mat = pair.Key;
					mat.shader = newShader;
					mat.SetColor("_Color",color);
					mat.SetFloat("_Strength",strength);
				}
			}
			else if(DkEffect.GetObjectEffectType(this.gameObject) == eDkEffectType.Color_Light)
			{
				dict = DkEffect.GetObjectMat(this.gameObject);
				
				if(dict == null)
				{
					return;
				}
				
				foreach(KeyValuePair<Material,Shader> pair in dict)
				{
					pair.Key.shader = pair.Value;
				}
				
				DkEffect.removeObjectMat(this.gameObject);
			}
		}
	}
}
