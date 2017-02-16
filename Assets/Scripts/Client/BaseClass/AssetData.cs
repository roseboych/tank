using UnityEngine;
using System.Collections;
using System;

namespace Client{
    public class AssetData
    {
        private UnityEngine.Object _Object;
        public Type AssetType { get; set; }
        public string Path { get; set; }
        public int RefCount { get; set; }
        public bool IsLoaded
        {
            get
            {
                return null != _Object;
            }
        }

        public UnityEngine.Object AssetObject
        {
            get
            {
                if (null == _Object)
                {
                    _ResourcesLoad();
                }
                return _Object;
            }
        }

        public IEnumerator GetCoroutineObject(Action<UnityEngine.Object> _loaded)
        {
            while (true)
            {
                yield return null;
                if (null == _Object)
                {
                    //yield return null;
                    _ResourcesLoad();
                    yield return null;
                }
                if (null != _loaded)
                    _loaded(_Object);
                yield break;
            }

        }

        private void _ResourcesLoad()
        {
            try
            {
                _Object = Resources.Load(Path);
                if (null == _Object)
                    Debug.Log("Resources Load Failure! Path:" + Path);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> _loaded)
        {
            return GetAsyncObject(_loaded, null);
        }

        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> _loaded, Action<float> _progress)
        {
            if (null != _Object)
            {
                _loaded(_Object);
                yield break;
            }

            ResourceRequest _resRequest = Resources.LoadAsync(Path);

            // 
            while (_resRequest.progress < 0.9)
            {
                if (null != _progress)
                    _progress(_resRequest.progress);
                yield return null;
            }


            while (!_resRequest.isDone)
            {
                if (null != _progress)
                    _progress(_resRequest.progress);
                yield return null;
            }


            _Object = _resRequest.asset;
            if (null != _loaded)
                _loaded(_Object);

            yield return _resRequest;
        }
    }
}

