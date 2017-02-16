using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


namespace Client
{
	public class SceneManager : Singleton<SceneManager>
	{

		private Dictionary<EnumSceneType, SceneData> dicSceneInfos = null;

		private BaseScene currentScene = new BaseScene();

		public EnumSceneType LastSceneType { get; set; }

		public EnumSceneType ChangeSceneType { get; private set; }

		private EnumUIType sceneOpenUIType = EnumUIType.None;
		private object[]   sceneOpenUIParams = null; 

		public BaseScene CurrentScene
		{
			get { return currentScene; }
			set 
			{ 
				currentScene = value; 
			}
		}

		public override void Init ()
		{
			dicSceneInfos = new Dictionary<EnumSceneType, SceneData> ();
		}

		public void RegisterAllScene()
		{
			RegisterScene(EnumSceneType.StartGame, "StartGameScene", null, null);
			RegisterScene(EnumSceneType.LoginScene, "LoginScene", typeof(BaseScene), null);
			RegisterScene(EnumSceneType.MainScene, "MainScene", null, null);
			RegisterScene(EnumSceneType.CopyScene, "CopyScene", null, null);
		}

		public void RegisterScene(EnumSceneType sceneType, string sceneName, Type type, params object[] _params)
		{
			if ( type== null || type.BaseType != typeof(BaseScene))
			{
				throw new Exception("Register scene type must not null and extends BaseScene");
			}
			if (!dicSceneInfos.ContainsKey(sceneType))
			{
				SceneData sceneInfo = new SceneData(sceneName, type, _params);
				dicSceneInfos.Add(sceneType, sceneInfo);
			}
		}

		internal BaseScene GetBaseScene(EnumSceneType _sceneType)
		{

			SceneData sceneInfo = GetSceneInfo(_sceneType);
			if (sceneInfo == null || sceneInfo.SceneType == null)
			{
				return null;
			}
			BaseScene scene = System.Activator.CreateInstance(sceneInfo.SceneType) as BaseScene;
			return scene;
		}

		public SceneData GetSceneInfo(EnumSceneType sceneType)
		{
			if (dicSceneInfos.ContainsKey(sceneType))
			{
				return dicSceneInfos[sceneType];
			}
			Debug.LogError("This Scene is not register! ID: " + sceneType.ToString());
			return null;
		}

		public string GetSceneName(EnumSceneType sceneType)
		{
			if (dicSceneInfos.ContainsKey(sceneType))
			{
				return dicSceneInfos[sceneType].SceneName;
			}
			Debug.LogError("This Scene is not register! ID: " + sceneType.ToString());
			return null;
		}

		public void ClearScene()
		{
			dicSceneInfos.Clear();
		}

		public void ChangeSceneDirect(EnumSceneType _sceneType)
		{
			UIManager.Instance.CloseUIAll();
            if (LastSceneType == _sceneType)
            {
                Debug.LogError("ChangeSceneDirect error: LastSceneType == _sceneType");
                return;
            }
			if (CurrentScene != null)
			{
				CurrentScene.Release();
				CurrentScene = null;
			}

			LastSceneType = ChangeSceneType;
			ChangeSceneType = _sceneType;
			string sceneName = GetSceneName(_sceneType);
            if(sceneName != null)
			    CoroutineController.Instance.StartCoroutine(AsyncLoadScene(sceneName));
		}

		private IEnumerator<AsyncOperation> AsyncLoadScene(string sceneName)
		{
			AsyncOperation oper = Application.LoadLevelAsync(sceneName);
			yield return oper;
            
			if (sceneOpenUIType != EnumUIType.None)
			{
				UIManager.Instance.OpenUI(sceneOpenUIType, sceneOpenUIParams);
				sceneOpenUIType = EnumUIType.None;
			}
		}
	
		public void ChangeScene(EnumSceneType _sceneType)
		{
			UIManager.Instance.CloseUIAll();
			
			if (CurrentScene != null)
			{
				CurrentScene.Release();
				CurrentScene = null;
			}
			
			LastSceneType = ChangeSceneType;
			ChangeSceneType = _sceneType;
			CoroutineController.Instance.StartCoroutine(AsyncLoadOtherScene());
		}
		
		public void ChangeScene(EnumSceneType _sceneType, EnumUIType _uiType, params object[] _params)
		{
			sceneOpenUIType = _uiType;
			sceneOpenUIParams = _params;
			if (LastSceneType == _sceneType)
			{
				if (sceneOpenUIType == EnumUIType.None)
				{
					return;
				}
				UIManager.Instance.OpenUI( sceneOpenUIType, sceneOpenUIParams);
				sceneOpenUIType = EnumUIType.None;
			}else
			{
				ChangeScene(_sceneType);
			}
		}
		
		private IEnumerator AsyncLoadOtherScene()
		{
			string sceneName = GetSceneName(EnumSceneType.LoadingScene);
			AsyncOperation oper = Application.LoadLevelAsync(sceneName);
			yield return oper;
			if (oper.isDone)
			{
                   //something to do
			}
		}

	}
}

