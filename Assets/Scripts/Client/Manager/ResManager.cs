using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Client
{
	

	public class ResManager : Singleton<ResManager>
	{
		private Dictionary<string, AssetData> dicAssetData = null;

		public override void Init ()
		{
			dicAssetData = new Dictionary<string, AssetData>();
		}

		public UnityEngine.Object LoadInstance(string _path)
		{
			UnityEngine.Object _obj = Load(_path);
			return Instantiate(_obj);
		}


		public void LoadCoroutineInstance(string _path, Action<UnityEngine.Object> _loaded)
		{
			LoadCoroutine(_path, (_obj)=>{ Instantiate(_obj, _loaded); });
		}

		public void LoadAsyncInstance(string _path, Action<UnityEngine.Object> _loaded)
		{
			LoadAsync(_path, (_obj)=>{ Instantiate(_obj, _loaded); });
		}

		public void LoadAsyncInstance(string _path, Action<UnityEngine.Object> _loaded, Action<float> _progress)
		{
			LoadAsync(_path, (_obj)=>{ Instantiate(_obj, _loaded); } , _progress);
		}

		public UnityEngine.Object Load(string _path)
		{
			AssetData _AssetData = GetAssetData(_path);
			if (null != _AssetData)
				return _AssetData.AssetObject;
			return null;
		}


		public void LoadCoroutine(string _path, Action<UnityEngine.Object> _loaded)
		{
			AssetData _AssetData = GetAssetData(_path, _loaded);
			if (null != _AssetData)
				CoroutineController.Instance.StartCoroutine(_AssetData.GetCoroutineObject(_loaded));
		}



		public void LoadAsync(string _path, Action<UnityEngine.Object> _loaded)
		{
			LoadAsync(_path, _loaded, null);
		}


		public void LoadAsync(string _path, Action<UnityEngine.Object> _loaded, Action<float> _progress)
		{
			AssetData _AssetData = GetAssetData(_path, _loaded);
			if (null != _AssetData)
				CoroutineController.Instance.StartCoroutine(_AssetData.GetAsyncObject(_loaded, _progress));
		}




		private AssetData GetAssetData(string _path)
		{
			return GetAssetData(_path, null);
		}

		private AssetData GetAssetData(string _path, Action<UnityEngine.Object> _loaded)
		{
			if (string.IsNullOrEmpty(_path))
			{
				Debug.LogError("Error: null _path name.");
				if (null != _loaded)
					_loaded(null);
			}

			AssetData _AssetData = null;
			if (!dicAssetData.TryGetValue(_path, out _AssetData))
			{
				_AssetData = new AssetData();
				_AssetData.Path = _path;
				dicAssetData.Add(_path, _AssetData);
			}
			_AssetData.RefCount++;
			return _AssetData;
		}

		private UnityEngine.Object Instantiate(UnityEngine.Object _obj)
		{
			return Instantiate(_obj, null);
		}

		private UnityEngine.Object Instantiate(UnityEngine.Object _obj, Action<UnityEngine.Object> _loaded)
		{
			UnityEngine.Object _retObj = null;
			if (null != _obj)
			{
				_retObj = MonoBehaviour.Instantiate(_obj);
				if (null != _retObj)
				{
					if (null != _loaded)
					{
						_loaded(_retObj);
						return null;
					}
					return _retObj;
				}
				else
				{
					Debug.LogError("Error: null Instantiate _retObj.");
				}
			}
			else
			{
				Debug.LogError("Error: null Resources Load return _obj.");
			}
			return null;
		}


	}
}

