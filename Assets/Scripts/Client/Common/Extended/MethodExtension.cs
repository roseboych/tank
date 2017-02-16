using System;
using UnityEngine;


namespace Client
{
	static public class MethodExtension
	{
		static public T GetOrAddComponent<T>(this GameObject go) where T : Component
		{
			T ret = go.GetComponent<T>();
			if (null == ret)
				ret = go.AddComponent<T>();
			return ret;
		}

	}
}

