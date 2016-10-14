using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 观察者
/// </summary>

public class Observer : MonoBehaviour
{
    // 消息通知
    public class Notifier
    {
        // 消息处理
        public delegate void Proc(object data_);

        // 名字
        public string name;
        // 处理
        public Proc proc;

        /// <summary>
        /// 构造
        /// </summary>
        public Notifier(string name_, Proc proc_)
        {
            name = name_;
            proc = proc_;
        }
    }

    // 消息
    private List<Notifier> _note = null;

    /// <summary>
    /// 设置消息
    /// </summary>
    public void SetNote(List<Notifier> notifier_)
    {
        _note = notifier_;
    }

    /// <summary>
    /// 获取消息
    /// </summary>
    public List<Notifier> GetNote()
    {
        return _note;
    }
}
