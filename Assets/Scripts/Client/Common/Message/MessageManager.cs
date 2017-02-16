using System;
using System.Collections.Generic;
using UnityEngine;


namespace Client
{
	public class MessageManager : Singleton<MessageManager>
	{
		private Dictionary<string, List<MessageEvent>> dicMessDelegate = null;

		public override void Init ()
		{
			dicMessDelegate = new Dictionary<string, List<MessageEvent>>();
		}
		#region Add & Remove Listener

		public void AddListener(string messageName, MessageEvent messageEvent)
		{
			List<MessageEvent> list = null;
            if (messageName == null)
            {
                Debug.LogError("AddListener messageName is null");
                return;
            }

			if (dicMessDelegate.ContainsKey(messageName))
			{
				list = dicMessDelegate[messageName];
			}
			else
			{
				list = new List<MessageEvent>();
				dicMessDelegate.Add(messageName, list);
			}
			if (!list.Contains(messageEvent))
			{
				list.Add(messageEvent);
			}
		}

		public void RemoveListener(string messageName, MessageEvent messageEvent)
		{
			if (dicMessDelegate.ContainsKey(messageName))
			{
				List<MessageEvent> list = dicMessDelegate[messageName];
				if (list.Contains(messageEvent))
				{
					list.Remove(messageEvent);
				}
				if (list.Count <= 0)
				{
					dicMessDelegate.Remove(messageName);
				}
			}
		}

		public void RemoveAllListener()
		{
			dicMessDelegate.Clear();
		}

		#endregion

		#region Send Message

		public void SendMessage(Message message)
		{
			MessageDispatcher(message);
		}

		public void SendMessage(string name, object sender)
		{
			SendMessage(new Message(name, sender));
		}

		public void SendMessage(string name, object sender, object content)
		{
			SendMessage(new Message(name, sender, content));
		}

		public void SendMessage(string name, object sender, object content, params object[] dicParams)
		{
			SendMessage(new Message(name, sender, content, dicParams));
		}

		private void MessageDispatcher(Message message)
		{
			if (dicMessDelegate == null || !dicMessDelegate.ContainsKey(message.Name))
				return;
			List<MessageEvent> list = dicMessDelegate[message.Name];
			for (int i=0; i<list.Count; i++)
			{
				MessageEvent messageEvent = list[i];
				if (null != messageEvent)
				{
					messageEvent(message);
				}
			}
		}

		#endregion

	}
}

