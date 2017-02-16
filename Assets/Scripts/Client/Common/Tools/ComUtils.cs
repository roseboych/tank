using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;


public class CommonUtils
{
    /// <summary>
    /// 初始化Transform
    /// </summary>
    /// <param name="trans">Trans.</param>
    /// <param name="parent">Parent.</param>
    public static void SetParent(Transform trans, Transform parent)
    {
        if (trans != null && parent != null && trans.parent != parent)
        {
            trans.parent = parent;
            ResetTransform(trans);
        }
    }

    /// <summary>
    /// 重置Transform
    /// </summary>
    /// <param name="trans">Trans.</param>
    public static void ResetTransform(Transform trans)
    {
        if (trans == null)
            return;
        trans.localPosition = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = Vector3.one;
    }

    /// <summary>
    /// 设置层包含子节点
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="layer"></param>
    /// <param name="includeInactive"></param>
    public static void SetLayerIncludeChild(Transform trans, int layer, bool includeInactive = false)
    {
        if (trans == null)
        {
            //Logger.Message("trans is null.");
            return;
        }

        var all = trans.GetComponentsInChildren<Transform>(includeInactive);
        for (int i = 0, max = all.Length; i < max; i++)
        {
            all[i].gameObject.layer = layer;
        }
    }

    /// <summary>
    /// 销毁该节点上所有子节点
    /// </summary>
    /// <param name="trans"></param>
    public static void DestroyChildren(Transform trans)
    {
        int count = trans.childCount;
        for (int i = count - 1; i >= 0; i--)
        {
            var itemTemp = trans.GetChild(i);
            itemTemp.transform.parent = null;
            GameObject.Destroy(itemTemp.gameObject);
        }
    }


    /// <summary>
    /// 将颜色字符串(如CDCDCD，3EA93E)转成Color
    /// </summary>
    /// <param name="strColor"></param>
    /// <returns></returns>
    public static Color GetColorBy16String(string strColor)
    {
        int r = 0;
        int g = 0;
        int b = 0;

        if (strColor != null)
        {
            if (strColor.Length >= 6)
            {
                string strR = strColor[0].ToString() + strColor[1].ToString();
                string strG = strColor[2].ToString() + strColor[3].ToString();
                string strB = strColor[4].ToString() + strColor[5].ToString();
                r = Convert.ToInt32(strR, 16);
                g = Convert.ToInt32(strG, 16);
                b = Convert.ToInt32(strB, 16);
            }
        }
        return new Color((float)r / 255, (float)g / 255, (float)b / 255);
    }

    public static Color GetColorByIntString(string strColor)
    {
        float r = 0;
        float g = 0;
        float b = 0;
        float a = 255;

        string[] pars = strColor.Split(',');
        if (pars.Length >= 3)
        {
            r = Convert.ToSingle(pars[0]);
            g = Convert.ToSingle(pars[1]);
            b = Convert.ToSingle(pars[2]);

            if (pars.Length > 3)
            {
                a = Convert.ToSingle(pars[3]);
            }
        }
        return new Color(r / 255, g / 255, b / 255, a / 255);
    }

    public static string GetTextMeshColorStr(string content, string colorInt)
    {
        int r = 0;
        int g = 0;
        int b = 0;
        int a = 255;

        string[] pars = colorInt.Split(',');
        if (pars.Length >= 3)
        {
            r = Convert.ToInt32(pars[0]);
            g = Convert.ToInt32(pars[1]);
            b = Convert.ToInt32(pars[2]);

            if (pars.Length > 3)
            {
                a = Convert.ToInt32(pars[3]);
            }
        }
        return GetTextMeshColorStr(content, r, g, b, a);
    }

    public static string GetTextMeshColorStr(string content, int r, int g, int b, int a)
    {
        string Rs = Convert.ToString(r, 16);
        string Gs = Convert.ToString(g, 16);
        string Bs = Convert.ToString(b, 16);
        string As = Convert.ToString(a, 16);

        string res = string.Format("<color=#{0}{1}{2}{3}>{4}</color>", Rs, Gs, Bs, As, content);
        return res;
    }
    public static string IntToDay(int num)
    {
        string str = "周日";
        switch (num)
        {
            case 0:
                str = "周日";
                break;
            case 1:
                str = "周一";
                break;
            case 2:
                str = "周二";
                break;
            case 3:
                str = "周三";
                break;
            case 4:
                str = "周四";
                break;
            case 5:
                str = "周五";
                break;
            case 6:
                str = "周六";
                break;
        }
        return str;
    }

    //从两点直线方程中返回给定Y值的X值
    public static float returnLineX(float x1, float x2, float y1, float y2, float y)
    {
        float x = ((x2 - x1) * y + x1 * (y2 - y1) - y1 * (x2 - x1)) / (y2 - y1);
        return x;
    }

    //点到直线的距离
    public static double returnDisFormPointToLine(float x1, float x2, float y1, float y2, float x, float y)
    {
        double testResoult = ((y2 - y1) * x - (x2 - x1) * y + (-x1 * (y2 - y1) + y1 * (x2 - x1))) * ((y2 - y1) * x - (x2 - x1) * y + (-x1 * (y2 - y1) + y1 * (x2 - x1)));
        double distance = Math.Sqrt(testResoult / (double)((y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1)));
        return distance;
    }

    public static System.TimeSpan GetTimeSpan(string timestr)
    {
        string[] strarr = timestr.Split(':');

        int hh = (strarr.Length > 0) ? Convert.ToInt32(strarr[0]) : 0;
        int mm = (strarr.Length > 1) ? Convert.ToInt32(strarr[1]) : 0;
        int ss = (strarr.Length > 2) ? Convert.ToInt32(strarr[2]) : 0;

        System.TimeSpan span = new TimeSpan(hh, mm, ss);

        return span;
    }


    //public static string GetCountDown(long targetTime)
    //{
    //    long chaTime = ServerTime.CountDown(targetTime);

    //    string str = "00:00:00";
    //    if (chaTime > 0)
    //    {
    //        long h = chaTime / 3600;
    //        long m = (chaTime % 3600) / 60;
    //        long s = chaTime % 60;
    //        str = string.Format("{0,2:D2}:{1,2:D2}:{2,2:D2}", h, m, s);
    //    }
    //    return str;
    //}

    /// <summary>
    /// 秒返回倒计时
    /// </summary>
    /// <param name="chaTime"></param>
    /// <returns></returns>
    public static string GetCountryDownByMs(int chaTime)
    {
        string str = "00:00:00";
        if (chaTime > 0)
        {
            long h = chaTime / 3600;
            long m = (chaTime % 3600) / 60;
            long s = chaTime % 60;
            str = string.Format("{0,2:D2}:{1,2:D2}:{2,2:D2}", h, m, s);
        }
        return str;
    }

    public static string GetTimeSpanStr(TimeSpan span)
    {
        string str = "00:00";
        if (span.TotalSeconds > 0)
        {
            str = string.Format("{0,2:D2}:{1,2:D2}:{2,2:D2}", span.Hours, span.Minutes, span.Seconds);
        }
        return str;
    }
    /// <summary>
    ///  为字符串添加颜色 
    /// </summary>
    /// <param name="color"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string GetValueAddColor(string color, string content)
    {
        return '[' + color + ']' + content + "[-]";
    }

    /// <summary>
    /// 铜钱格式转换 
    /// </summary>
    /// <param name="money"></param>
    /// <returns></returns>
    public static string GetMoneyStr(double money)
    {
        string str = "";

        float wen = (float)(money % 1000);
        float liangVa = (float)((money % 1000000d) / 1000d);
        float liang = Mathf.Floor(liangVa);
        float ding = Mathf.Floor((float)(money / 1000000d));

        if (ding > 0)
        {
            str += ding.ToString() + "锭";
        }
        if (liang > 0)
        {
            str += liang.ToString() + "两";
        }
        if (wen > 0)
            str += wen.ToString() + "文";

        if (string.IsNullOrEmpty(str))
        {
            str = "0文";
        }
        return str;
    }

    public static void SetLabelText(UILabel label, ref string text)
    {
        int index = -1;
        int endIndex = -1;
        try
        {
            index = text.IndexOf("[size:");
            if (index >= 0)
            {
                char ch0 = text[index + 6];
                char ch1 = text[index + 7];

                if (ch0 >= '0' && ch0 <= '9' && ch1 >= '0' && ch1 <= '9')
                {
                    endIndex = text.IndexOf("]");
                    string sizeStr = text.Substring(index + 6, 2);
                    int size = Convert.ToInt32(sizeStr);
                    label.fontSize = size;
                    text = text.Remove(index, endIndex - index + 1);
                }
                label.MakePixelPerfect();
            }

            index = text.IndexOf("[sh]");
            if (index >= 0)
            {
                text = text.Remove(index, 4);
                label.effectStyle = UILabel.Effect.Shadow;
                //label.effectColor = shadowColor;
            }


        }
        catch (Exception excep)
        {
            //Logger.Error(excep.ToString());
        }
    }
    //从prefab创建游戏物体
    public static GameObject creatObjFramePreb(GameObject preb, Transform parent)
    {
        if (preb == null) return null;

        GameObject obj = (GameObject)GameObject.Instantiate(preb);
        obj.transform.parent = parent;
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localScale = new Vector3(1, 1, 1);

        return obj;
    }

    //从prefab创建游戏物体
    public static GameObject creatObjFramePreb(GameObject preb, Transform parent, Vector3 position, Vector3 scale)
    {
        if (preb == null) return null;

        GameObject obj = (GameObject)GameObject.Instantiate(preb);
        obj.transform.parent = parent;
        obj.transform.localPosition = position;
        obj.transform.localScale = scale;

        return obj;
    }

    //从prefab创建游戏物体
    public static T GetItemFromGrid<T>(Transform ParentGrid, bool First) where T : Component
    {
        GameObject obj = ParentGrid.GetChild(0).gameObject;
        if (!First)
        {
            obj = ParentGrid.GetChild(0).gameObject;
            obj = (GameObject)GameObject.Instantiate(obj);
            obj.transform.parent = ParentGrid;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
        }

        return obj.GetComponent<T>();
    }
   
    public static string GetWan(long num)
    {
        if (num < 10000)
        {
            return num.ToString();
        }
        else
        {
            double temp = (double)num / 10000;
            return string.Format("{0:N1}万", temp);
        }
    }

    public static void SetGray(Transform form)
    {
        UISprite sp = form.GetComponent<UISprite>();
        if (sp != null)
        {
            sp.color = new Color(0, 0, 0, 1);
        }

        if (form.childCount > 0)
        {
            for (int i = 0; i < form.childCount; i++)
            {
                SetGray(form.GetChild(i));
            }

        }
    }
    
    public static void RemoveRepeatedElements<T>(List<T> list)
    {
        if (list != null && list.Count > 0)
        {
            int i = list.Count;
            while (--i > -1)
            {
                if (list.IndexOf(list[i]) != i)
                {
                    list.RemoveAt(i);
                }
            }
        }
    }


}
