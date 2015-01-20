using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Dk.Effect
{
	public enum eDkEffectType
	{
		NULL = 0,
		Color_Light = 1,
		Flaht_Light = 2,
	}

	internal class DkEffect
	{
		private static Dictionary<GameObject,eDkEffectType> effectDict = new Dictionary<GameObject, eDkEffectType>();

		private static Dictionary<GameObject,Dictionary<Material,Shader>> objDict = new Dictionary<GameObject, Dictionary<Material, Shader>>();

		public static bool AddObjectMat(GameObject obj , eDkEffectType type)
		{
			if(obj == null)
			{
				return false;
			}

			effectDict[obj] = type;

			if(!objDict.ContainsKey(obj))
			{
				Renderer[] renders = obj.GetComponentsInChildren<Renderer>();
				if(renders != null && renders.Length > 0)
				{
					Dictionary<Material,Shader> dict = new Dictionary<Material, Shader>();
					foreach(Renderer render in renders)
					{
						Material[] mats = render.materials;
						foreach(Material mat in mats)
						{
							Shader oldShader = mat.shader;
							if(oldShader != null)
							{
								dict.Add(mat,oldShader);
							}
						}
					}
					objDict.Add(obj,dict);
					return true;
				}
				else
				{
					return false;
				}
			}
			else return false;
		}

		public static bool removeObjectMat(GameObject obj)
		{
			if(obj == null)
			{
				return false;
			}

			effectDict.Remove(obj);

			if(objDict.ContainsKey(obj))
			{
				Dictionary<Material,Shader> dict = objDict[obj];
				dict.Clear();
				objDict.Remove(obj);
				return true;
			}

			else return false;
		}

		public static Dictionary<Material,Shader> GetObjectMat(GameObject obj)
		{
			if(obj == null)
			{
				return null;
			}

			if(objDict.ContainsKey(obj))
			{
				return objDict[obj];
			}
			else return null;
		}

		public static eDkEffectType GetObjectEffectType(GameObject obj)
		{
			if(obj == null)
			{
				return eDkEffectType.NULL;
			}
			
			if(effectDict.ContainsKey(obj))
			{
				return effectDict[obj];
			}
			else return eDkEffectType.NULL;
		}

	}
}
