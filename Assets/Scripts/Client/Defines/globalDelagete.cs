using UnityEngine;
using System.Collections;

namespace Client
{
    #region Global delegate 委托

    public delegate void StateChangedEvent(object sender, EnumObjectState newState, EnumObjectState oldState);

    public delegate void MessageEvent(Message message);
   
    public delegate void PropertyChangedHandle(BaseActor actor, int id, object oldValue, object newValue);

    #endregion
}

