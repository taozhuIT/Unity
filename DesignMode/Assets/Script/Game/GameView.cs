using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameView : Observer 
{
    /// <summary>
    /// 制造通知
    /// </summary>
    public Notifier MakeNotify(string msgName_, Notifier.Proc proc_)
    {
        return new Notifier(msgName_, proc_);
    }

    /// <summary>
    /// 消息注册
    /// </summary>
    public void Register()
    {
        List<Notifier> notifierList = new List<Notifier>();
        foreach (object obj in this.GetNote())
            notifierList.Add((Notifier)obj);

        SetNote(notifierList);
        ObserverMannger.ObserverRegister(this);
    }

    /// <summary>
    /// 消息撤销
    /// </summary>
    public void UnRegister()
    {

    }

    /// <summary>
    /// 初始
    /// </summary>
    protected virtual void Awake()
    {
        Register();
    }

    /// <summary>
    /// 起始
    /// </summary>
	protected virtual void Start () 
    {
	
	}

    /// <summary>
    /// 更新
    /// </summary>
    protected virtual void Update()
    {
	
	}

    /// <summary>
    /// 得到消息
    /// </summary>
    protected virtual object[] GetNote()
    {
        return null;
    }
}
