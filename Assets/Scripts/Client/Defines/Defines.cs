using UnityEngine;
using System.Collections;

namespace Client
{

    #region Defines static class & cosnt

    /// <summary>
    /// 路径定义。
    /// </summary>
    public static class UIPathDefines
    {
       
        // UI预设。
        public const string UI_PREFAB = "Prefabs/";

        // ui子页面预设。
        public const string UI_SUBUI_PREFAB = "UIPrefab/SubUI/";
        
        // icon路径
        public const string UI_IOCN_PATH = "UI/Icon/";

        public static string GetPrefabPathByType(EnumUIType _uiType)
        {
            string _path = string.Empty;
            switch (_uiType)
            {
                case EnumUIType.MainUI:
                    _path = UI_PREFAB + "MainUI";
                    break;
                case EnumUIType.Equipe:
                    _path = UI_PREFAB + "Equipe";
                    break;
                default:
                    Debug.LogError("Not Find EnumUIType! type: " + _uiType.ToString());
                    break;
            }
            return _path;
        }

        public static System.Type GetUIScriptByType(EnumUIType _uiType)
        {
            System.Type _scriptType = null;
            switch (_uiType)
            {
                case EnumUIType.MainUI:
                    //_scriptType = typeof(MainUI);
                    break;
                case EnumUIType.Equipe:
                    //_scriptType = typeof(Equipe);
                    break;
                default:
                    Debug.LogError("Not Find EnumUIType! type: " + _uiType.ToString());
                    break;
            }
            return _scriptType;
        }

    }

    #endregion


	#region Global enum 枚举

	// 对象当前状态枚举类
	public enum EnumObjectState
	{
		None,
		Initial,
		Loading,
		Ready,
		Disabled,
		Closing
	}

	/// <summary>
	/// UI面板类型
	/// </summary>
	public enum EnumUIType : int
	{
		None = -1,

		MainUI,

		Equipe
	}


	public enum EnumPropertyType : int
	{
		RoleName = 1, 
		Sex,   
		RoleID, 
		Gold, 
		Coin,    
		Level,
		Exp,    
		HP,     

	}

	public enum EnumActorType
	{
		None = 0,
		Player,
		Monster
	}

	public enum EnumSceneType
	{
		None = 0,
		StartGame,
		LoadingScene,
		LoginScene,
		MainScene,
		CopyScene,
		PVPScene,
		PVEScene,
	}
	
	#endregion

	
	
}
