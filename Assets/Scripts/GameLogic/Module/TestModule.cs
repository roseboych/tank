using System;
using Client;
using UnityEngine;

public class TestModule : BaseModule
{
    public int Gold { get; private set; }

    public TestModule()
    {
        this.AutoRegister = true;
    }

    protected override void OnLoad()
    {
        MessageManager.Instance.AddListener(MessageType.testOne, UpdateGold);
        base.OnLoad();
    }

    protected override void OnRelease()
    {
        MessageManager.Instance.RemoveListener(MessageType.testTwo, UpdateGold);
        base.OnRelease();
    }

    private void UpdateGold(Message message)
    {
        int gold = (int)message["gold"];
        if (gold >= 0)
        {
            Gold = gold;
            Debug.Log("TestModule UpdateGold : " + gold);
        }
    }
}

