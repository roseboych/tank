using UnityEngine;
using System.Collections;

namespace Client
{
	public abstract class BaseUI : MonoBehaviour
	{

		private Transform _CachedTransform;

		public Transform cachedTransform
		{
			get
			{
				if (!_CachedTransform)
				{
					_CachedTransform = this.transform;
				}
				return _CachedTransform;
			}
		}
		
		private GameObject _CachedGameObject;

		public GameObject cachedGameObject
		{
			get
			{
				if (!_CachedGameObject)
				{
					_CachedGameObject = this.gameObject;
				}
				return _CachedGameObject;
			}
		}

		protected EnumObjectState state = EnumObjectState.None;

		public event StateChangedEvent StateChanged;

		public EnumObjectState State
		{
			protected set
			{
				if (value != state)
				{
					EnumObjectState oldState = state;
					state = value;
					if (null != StateChanged)
					{
						StateChanged(this, state, oldState);
					}
				}
			}
			get { return this.state; }
		}
	
		public abstract EnumUIType GetUIType ();


		/// <summary>
		/// UI层级置顶
		/// </summary>
		protected virtual void SetDepthToTop()
		{
			
		}

		
		// Use this for initialization
		void Start ()
		{
			OnStart ();
		}

		void Awake()
		{
			this.State = EnumObjectState.Initial;
			OnAwake ();
		}
		
		// Update is called once per frame
		void Update ()
		{
			if (EnumObjectState.Ready == this.state) 
			{
				OnUpdate(Time.deltaTime);
			}
		}

		public void Release()
		{
			this.State = EnumObjectState.Closing;
			GameObject.Destroy (cachedGameObject);
			OnRelease ();
		}
		
		protected virtual void OnStart()
		{

		}

		protected virtual void OnAwake()
		{
			this.State = EnumObjectState.Loading;
			//播放音乐
			this.OnPlayOpenUIAudio();
		}
		
		protected virtual void OnUpdate(float deltaTime)
		{
			
		}

		protected virtual void OnRelease()
		{
			this.OnPlayCloseUIAudio();
		}

		
		/// <summary>
		/// 播放打开界面音乐
		/// </summary>
		protected virtual void OnPlayOpenUIAudio()
		{
			
		}
		
		/// <summary>
		/// 播放关闭界面音乐
		/// </summary>
		protected virtual void OnPlayCloseUIAudio()
		{
			
		}

		protected virtual void SetUI(params object[] uiParams)
		{
			this.State = EnumObjectState.Loading;
		}
		
		public virtual void SetUIparam(params object[] uiParams)
		{
			
		}
		
		
		protected virtual void OnLoadData()
		{
			
		}
		
		public void SetUIWhenOpening(params object[] uiParams)
		{
			SetUI(uiParams);
			CoroutineController.Instance.StartCoroutine(AsyncOnLoadData());
		}
		
		private IEnumerator AsyncOnLoadData()
		{
			yield return new WaitForSeconds(0);
			if (this.State == EnumObjectState.Loading)
			{
				this.OnLoadData();
				this.State = EnumObjectState.Ready;
			}
		}
	}
}

