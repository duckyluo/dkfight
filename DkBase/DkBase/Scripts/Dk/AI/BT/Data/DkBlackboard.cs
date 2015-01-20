using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Event;

namespace Dk.BehaviourTree
{
	public class DkBlackboard : DkEventDispatch
	{
		private Dictionary<string,object> dataStore = new Dictionary<string, object>();

		public override void Destroy()
		{
			base.Destroy();
			dataStore.Clear();
			dataStore = null;
		}
		
		public bool HasData
		{
			get
			{
				return dataStore.Count > 0;
			}
		}

		public object GetData(string name)
		{
			return dataStore[name];
		}

		public T GetData<T>(string name)
		{
			if(!dataStore.ContainsKey(name))
			{
				return default(T);
			}
			else return (T)dataStore[name];
		}

		public void AddData(string name, object data)
		{
			if(!dataStore.ContainsKey(name))
			{
				dataStore.Add(name,data);
				//this.DispatchEvent(new DkEvent(DkEventDef.DATA_ADD));
			}
		}

		public object RemoveData(string name)
		{
			if(!dataStore.ContainsKey(name))
			{
				return null;
			}

			object obj = dataStore[name];
			dataStore.Remove(name);
			//this.DispatchEvent(new DkEvent(DkEventDef.DATA_REMOVE));
			return obj;
		}

		public void UpdateData(string name, object data)
		{
			if(!dataStore.ContainsKey(name))
			{
				return;
			}
			dataStore[name] = data;
			//this.DispatchEvent(new DkEvent(DkEventDef.DATA_CHANGE));
		}
	}
}
