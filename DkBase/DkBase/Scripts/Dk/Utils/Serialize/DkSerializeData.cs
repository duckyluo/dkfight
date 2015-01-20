using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dk.Util
{
	public class DkSerializeData : MonoBehaviour
	{
		[SerializeField]
		protected byte[] bytes;
		
		protected void DoSerialize(object obj)
		{
			if(obj != null)
			{
				IFormatter formatter = new BinaryFormatter();
				using(MemoryStream outstream = new MemoryStream())
				{
					formatter.Serialize(outstream,obj);
					bytes = outstream.ToArray();
				}
			}
		}
		
		protected object GetDeSerialiezeObj()
		{
			object obj = null;
			if(bytes != null && bytes.Length > 0)
			{
				IFormatter formatter = new BinaryFormatter();
				using(MemoryStream outstream = new MemoryStream(bytes))
				{
					obj = formatter.Deserialize(outstream);
				}
			}
			return obj;
		}
	}
}
