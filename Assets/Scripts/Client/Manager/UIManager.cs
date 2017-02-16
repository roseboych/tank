using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
	public class UIManager : Singleton<UIManager>
	{

		private Dictionary<EnumUIType, GameObject> dicOpenUIs = null;

		private Stack<UIData> stackOpenUIs = null;

		public override void Init ()
		{
			dicOpenUIs = new Dictionary<EnumUIType, GameObject>();
			stackOpenUIs = new Stack<UIData>();
		}


		public T GetUI<T>(EnumUIType _uiType) where T : BaseUI
		{
			GameObject _retObj = GetUIObject(_uiType);
			if (_retObj != null)
			{
				return _retObj.GetComponent<T>();
			}
			return null;
		}

		public GameObject GetUIObject(EnumUIType _uiType)
		{
			GameObject _retObj = null;
			if (!dicOpenUIs.TryGetValue(_uiType, out _retObj))
				throw new Exception("dicOpenUIs TryGetValue Failure! _uiType :" + _uiType.ToString());
			return _retObj;
		}		

		public void PreloadUI(EnumUIType[] _uiTypes)
		{
			for (int i=0; i<_uiTypes.Length; i++)
			{
				PreloadUI(_uiTypes[i]);
			}
		}
		
		public void PreloadUI(EnumUIType _uiType)
		{
			string path = UIPathDefines.GetPrefabPathByType(_uiType);
			Resources.Load(path);
			//ResManager.Instance.ResourcesLoad(path);
		}
		
		public void OpenUI(EnumUIType[] uiTypes)
		{
			OpenUI(false, uiTypes, null);
		}

		public void OpenUI(EnumUIType uiType, params object[] uiObjParams)
		{
			EnumUIType[] uiTypes = new EnumUIType[1];
			uiTypes[0] = uiType;
			OpenUI(false, uiTypes, uiObjParams);
		}

		public void OpenUICloseOthers(EnumUIType[] uiTypes)
		{
			OpenUI(true, uiTypes, null);
		}

		public void OpenUICloseOthers(EnumUIType uiType, params object[] uiObjParams)
		{
			EnumUIType[] uiTypes = new EnumUIType[1];
			uiTypes[0] = uiType;
			OpenUI(true, uiTypes, uiObjParams);
		}

		private void OpenUI(bool _isCloseOthers, EnumUIType[] _uiTypes, params object[] _uiParams)
		{
			// Close Others UI.
			if (_isCloseOthers)
			{
				CloseUIAll();
			}

			// push _uiTypes in Stack.
			for (int i=0; i<_uiTypes.Length; i++)
			{
				EnumUIType _uiType = _uiTypes[i];
				if (!dicOpenUIs.ContainsKey(_uiType))
				{
					string _path = UIPathDefines.GetPrefabPathByType(_uiType);
					stackOpenUIs.Push(new UIData(_uiType, _path, _uiParams));
				}
			}

			// Open UI.
			if (stackOpenUIs.Count > 0)
			{
				CoroutineController.Instance.StartCoroutine(AsyncLoadData());
			}
		}


		private IEnumerator<int> AsyncLoadData()
		{
			UIData _UIData = null;
			UnityEngine.Object _prefabObj = null;
			GameObject _uiObject = null;

			if (stackOpenUIs != null && stackOpenUIs.Count > 0)
			{
				do 
				{
					_UIData = stackOpenUIs.Pop();
					_prefabObj = Resources.Load(_UIData.Path);
					if (_prefabObj != null)
					{
						//_uiObject = NGUITools.AddChild(Game.Instance.mainUICamera.gameObject, _prefabObj as GameObject);
						_uiObject = MonoBehaviour.Instantiate(_prefabObj) as GameObject;
						BaseUI _baseUI = _uiObject.GetComponent<BaseUI>();
						if (null == _baseUI)
						{
							_baseUI = _uiObject.AddComponent(_UIData.ScriptType) as BaseUI;
						}
						if (null != _baseUI)
						{
							_baseUI.SetUIWhenOpening(_UIData.UIParams);
						}
						dicOpenUIs.Add(_UIData.UIType, _uiObject);
					}

				} while(stackOpenUIs.Count > 0);
			}
			yield return 0;
		}



		public void CloseUI(EnumUIType _uiType)
		{
			GameObject _uiObj = null;
			if (!dicOpenUIs.TryGetValue(_uiType, out _uiObj))
			{
				Debug.Log("dicOpenUIs TryGetValue Failure! _uiType :" + _uiType.ToString());
				return;
			}
			CloseUI(_uiType, _uiObj);
		}

		public void CloseUI(EnumUIType[] _uiTypes)
		{
			for (int i=0; i<_uiTypes.Length; i++)
			{
				CloseUI(_uiTypes[i]);
			}
		}
		
		public void CloseUIAll()
		{
			List<EnumUIType> _keyList = new List<EnumUIType>(dicOpenUIs.Keys);
			foreach (EnumUIType _uiType in _keyList)
			{
				GameObject _uiObj = dicOpenUIs[_uiType];
				CloseUI(_uiType, _uiObj);
			}
			dicOpenUIs.Clear();
		}

		private void CloseUI(EnumUIType _uiType, GameObject _uiObj)
		{
			if (_uiObj == null)
			{
				dicOpenUIs.Remove(_uiType);
			}
			else
			{
				BaseUI _baseUI = _uiObj.GetComponent<BaseUI>();
				if (_baseUI != null)
				{
					_baseUI.StateChanged += CloseUIHandler;
					_baseUI.Release();
				}
				else
				{
					GameObject.Destroy(_uiObj);
					dicOpenUIs.Remove(_uiType);
				}
			}
		}

		private void CloseUIHandler(object _sender, EnumObjectState _newState, EnumObjectState _oldState)
		{
			if (_newState == EnumObjectState.Closing)
			{
				BaseUI _baseUI = _sender as BaseUI;
				dicOpenUIs.Remove(_baseUI.GetUIType());
				_baseUI.StateChanged -= CloseUIHandler;
			}
		}
	}
}

