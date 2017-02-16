using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Client;
using System;

public class GameController : StaticSingle<GameController>
{

    void Start()
    {

        ModuleManager.Instance.RegisterAllModules();

        SceneManager.Instance.RegisterAllScene();

        //UIManager.Instance.OpenUI(EnumUIType.MainUI);
        //GameObject go1 = Instantiate(Resources.Load<GameObject>("Prefabs/MainUI.prefab"));

        //GameController.Instance.StartCoroutine(AsyncLoadData());

        int time = System.Environment.TickCount; 
        
        GameObject go = null;
        for (int i = 1; i < 10; i++)
        {

            go = Instantiate(Resources.Load<GameObject>("Prefabs/Tank"));
            go.transform.position = UnityEngine.Random.insideUnitSphere * 20;

            go = ResManager.Instance.LoadInstance("Prefabs/Tank") as GameObject;
            go.transform.position = UnityEngine.Random.insideUnitSphere * 20;

            ResManager.Instance.LoadAsyncInstance("Prefabs/Tank", (_obj) =>
            {
                go = _obj as GameObject;
                go.transform.position = UnityEngine.Random.insideUnitSphere * 20;
            });

            ResManager.Instance.LoadCoroutineInstance("Prefabs/Tank", (_obj) =>
            {
                go = _obj as GameObject;
                go.transform.position = UnityEngine.Random.insideUnitSphere * 20;
            });
        }

        Debug.Log("Cost Time " + (System.Environment.TickCount - time) * 1000);

        StartCoroutine(AutoUpdateGold());
    }

    private IEnumerator AutoUpdateGold()
    {
        int gold = 0;
        while (true)
        {
            gold++;
            yield return new WaitForSeconds(1.0f);
            Message message = new Message(MessageType.testOne.ToString(), this);
            message["gold"] = gold;
            MessageManager.Instance.SendMessage(message);
        }
    }

    //private IEnumerator<int> AsyncLoadData()
    //{
    //   //something to do
    //}

}
