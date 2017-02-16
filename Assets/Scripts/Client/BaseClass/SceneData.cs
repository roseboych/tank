using UnityEngine;
using System.Collections;
using System;

namespace Client{

    public class SceneData  {
        	public Type SceneType { get; private set; }

			public string SceneName { get; private set; }

			public object[] Params { get; private set; }

			public SceneData(string _sceneName, Type _sceneType, params object[] _params)
			{
				this.SceneType = _sceneType;
				this.SceneName = _sceneName;
				this.Params = _params;
			}
	}
}

