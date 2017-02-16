using System;
using System.Collections.Generic;


namespace Client
{
	public class BaseScene : BaseModule
	{
		protected List<BaseActor> actorList = null;

		
		public BaseScene ()
		{
			actorList = new List<BaseActor> ();
		}

		public void AddActor(BaseActor actor)
		{
			if (null != actor && !actorList.Contains(actor))
			{
				actorList.Add(actor);
				actor.CurrentScene = this;
				actor.PropertyChanged += OnActorPropertyChanged;
				//actor.Load();
			}
		}

		public void RemoveActor(BaseActor actor)
		{
			if (null != actor && actorList.Contains(actor))
			{
				actorList.Remove(actor);
				actor.PropertyChanged -= OnActorPropertyChanged;
				//actor.Release();
				actor = null;
			}
		}

		public virtual BaseActor GetActorByID(int id)
		{
			if (null != actorList && actorList.Count > 0)
				for (int i=0; i<actorList.Count; i++)
					if (actorList[i].ID == id)
						return actorList[i];
			return null;
		}

		protected void OnActorPropertyChanged(BaseActor actor, int id, object oldValue, object newValue)
		{

		}
	}
}

