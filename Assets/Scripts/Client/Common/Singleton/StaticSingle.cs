using UnityEngine;
using System.Collections;

//切场景不会销毁的静态变量
public abstract class StaticSingle<T> : MonoBehaviour where T : StaticSingle<T>
{
	protected static T _Instance = null;
	
	public static T Instance
	{
		get{
			if (null == _Instance)
			{
                GameObject go = GameObject.Find("StaticSingleObject");
				if (null == go)
				{
                    go = new GameObject("StaticSingleObject");
					DontDestroyOnLoad(go);
				}
				_Instance = go.AddComponent<T>();

			}
			return _Instance;
		}
	}

	private void OnApplicationQuit ()
	{
		_Instance = null;
	}
}

