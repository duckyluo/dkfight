using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dk.Event
{

	public class DkEventDispatch
	{
		private class DictItem
		{
			public string msg;

			public Action<DkEvent> func;

			public void destroy()
			{
				msg = null;
				func = null;
			}
		}

		private List<DictItem> list = new List<DictItem>();

		virtual public void Destroy()
		{
			if(list != null)
			{
				foreach(DictItem item in list)
				{
					item.destroy();
				}

				list.Clear();
				list = null;
			}
		}

		public void AddEventListen(string msg,Action<DkEvent> func)
		{
			bool canAdd = true;
			foreach(DictItem listItem in list)
			{
				if(listItem.msg == msg && listItem.func == func)
				{
					canAdd = false;
				}
			}

			if(canAdd)
			{
				DictItem item = new DictItem();
				item.msg = msg;
				item.func = func;
				list.Add(item);
			}
		}

		public void RemoveEventListen(string msg,Action<DkEvent> func)
		{
			for(int i=0; i < list.Count; i++)
			{
				DictItem item = list[i];
				if(item.msg == msg && item.func == func)
				{
					list.Remove(item);
					break;
				}
			}
		}

		public void DispatchEvent(DkEvent evt)
		{
			List<DictItem> evtList = new List<DictItem>();
			for(int i=0; i < list.Count; i++)
			{
				DictItem item = list[i];
				if(evt.msg == item.msg)
				{
					evtList.Add(item);
				}
			}

			foreach(DictItem dict in evtList)
			{
				DkEvent eventItem = new DkEvent(evt.msg,evt.data);
				eventItem.target = this;
				CallMethod(dict.func,eventItem);
			}
		}

		private void CallMethod<T>(Action<T> func,T item)
		{
			try
			{
				func(item);
			}
			catch(Exception e)
			{
				Debug.LogError("[error] : "+e.Message +"\n"+ e.StackTrace);
			}
			finally
			{
				
			}
		}

	}

}





