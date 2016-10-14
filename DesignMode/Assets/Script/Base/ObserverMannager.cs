using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObserverMannger 
{
    // 观察者池
    private static Dictionary<string, List<Observer>> _observerPond = new Dictionary<string, List<Observer>>();
    // 观察者通知池
    private static Dictionary<string, List<Observer.Notifier.Proc>> _observerInformPond = new Dictionary<string, List<Observer.Notifier.Proc>>();

    /// <summary>
    /// 观察者注册
    /// </summary>
    public static void ObserverRegister(Observer observer_)
    {
        List<Observer.Notifier> notifier = observer_.GetNote();

        for (int i = 0; i < notifier.Count; ++i)
        {
            if (_observerInformPond.ContainsKey(notifier[i].name))
            {
                List<Observer.Notifier.Proc> notifierList = _observerInformPond[notifier[i].name];
                notifierList.Add(notifier[i].proc);
            }
            else
            {
                List<Observer.Notifier.Proc> notifierList = new List<Observer.Notifier.Proc>();
                notifierList.Add(notifier[i].proc);
                _observerInformPond.Add(notifier[i].name, notifierList);
            }
        }

    }

    /// <summary>
    /// 观察者撤销
    /// </summary>
    public static void ObserverUnRegister()
    {

    }

    /// <summary>
    /// 观察者广播
    /// </summary>
    public static void ObserverBroadcast(string mesgName_)
    {

    }
}
