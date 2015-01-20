
namespace Dk.Event
{
	public class DkEventDef
	{
		public const string EVENT_FINSH = "event_finish";
		public const string EVENT_DESTROY = "event_destroy";
		public const string LOAD_COMPLETE = "load_complete";	
		public const string DATA_INIT = "data_init";
		public const string DATA_ADD = "data_add";
		public const string DATA_REMOVE = "data_remove";
		public const string DATA_CHANGE = "data_change";
	}

	public class DkEvent
	{
		protected string _msg;
		
		protected object _data;
		
		public object target;

		public DkEvent()
		{
			
		}

		public DkEvent(string msg)
		{
			this._msg = msg;
		}
		
		public DkEvent(string msg,object data)
		{
			this._msg = msg;
			this._data = data;
		}
		
		public string msg
		{
			get
			{
				return _msg;
			}
		}
		
		public object data
		{
			get
			{
				return _data;
			}
		}
		
		public DkEvent clone()
		{
			return new DkEvent(this.msg,this.data);
		}
		
	}
}

