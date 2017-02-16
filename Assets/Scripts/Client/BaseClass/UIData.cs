using UnityEngine;
using System.Collections;
using System;

namespace Client{
    class UIData
    {
        public EnumUIType UIType { get; private set; }

        public Type ScriptType { get; private set; }

        public string Path { get; private set; }

        public object[] UIParams { get; private set; }
        public UIData(EnumUIType _uiType, string _path, params object[] _uiParams)
        {
            this.UIType = _uiType;
            this.Path = _path;
            this.UIParams = _uiParams;
            //this.ScriptType = UIPathDefines.GetUIScriptByType(this.UIType);
        }
    }
}

