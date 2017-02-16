using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Client;

public class TestOne : BaseUI
{
    private TestModule oneModule;

    public override EnumUIType GetUIType()
    {
        return EnumUIType.MainUI;
    }

    void Start()
    {
        oneModule = ModuleManager.Instance.Get<TestModule>();
    }

    protected override void OnAwake()
    {
        MessageManager.Instance.AddListener("AutoUpdateGold", UpdateGold);
        base.OnAwake();
    }

    protected override void OnRelease()
    {
        MessageManager.Instance.RemoveListener("AutoUpdateGold", UpdateGold);
        base.OnRelease();
    }

    private void UpdateGold(Message message)
    {
        int gold = (int)message["gold"];
        Debug.Log("TestOne UpdateGold : " + gold);
    }

    private void OnClickBtn()
    {
        UIManager.Instance.OpenUICloseOthers(EnumUIType.MainUI);
    }
}

