using UnityEngine;
using System.Collections;

public class UIMainView : GameView
{
    /// <summary>
    /// 得到消息
    /// </summary>
    protected override object[] GetNote()
    {
        return new object[]
        {
            MakeNotify("1", test1),
            MakeNotify("2", test2)
        };
    }

    /// <summary>
    /// 起始
    /// </summary>
    protected override void Start()
    {
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void Update()
    {
    }

    private void test1(object obj_)
    {

    }

    protected void test2(object obj_)
    {

    }
}
