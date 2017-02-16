using System;
using UnityEngine;

namespace Client
{
	public abstract class Singleton<T> where T : class, new() {

		protected static T _Instance = null;

		public static T Instance
		{
			get
			{ 
				if (null == _Instance)
				{
					_Instance = new T();
				}
				return _Instance; 
			}
		}
		
		protected Singleton()
		{
            if (null != _Instance){
                Debug.LogError("something wrong with Singleton");
                UnityEngine.Debug.Break();
            }
			Init ();
		}

		public virtual void Init() {}
	}
}
