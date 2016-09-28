using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class productdanmu : MonoBehaviour
{
    public GameObject textsdanmu;//弹幕预设

    private GameObject texts;//生成的弹幕物体
    private float production_timer;//生成弹幕的时间间隔

    private int[] numb = new int[] { -439, -339, -239, -139 - 39, 0, 39, 139, 239, 339, 439 };//四行弹幕的位置

    private int Row_index;//弹幕行数变化
    private int a;////弹幕行数变化中间变量（桥的作用）

    private Vector3 textpositon;//生成弹幕位置
    private Quaternion textrotation;//生产弹幕角度
    private string content;//弹幕文字内容



    void Start()
    {
        textpositon = new Vector3(96, Row_index, 0);//弹幕默认位置
        textrotation.eulerAngles = new Vector3(0, 0, 0);
        production_timer = 2f;
    }


    void Update()
    {
        production_timer -= Time.deltaTime;

        if (production_timer <= 0f)
        {
            int index = UnityEngine.Random.Range(0, DanMuStrings.Length);//弹幕的随机内容
            int order = 0;

            UnityEngine.Random.seed = (int)(Time.realtimeSinceStartup * 100);
            order = UnityEngine.Random.Range(0, 10);


            Row_index = numb[order]; // 弹幕位置随机
            content = DanMuStrings[index];

            createDanMuEntity();        //调用生成弹幕方法

            production_timer = 0.5f;//每隔2秒生成一个弹幕
        }
    }

    [HideInInspector]

    #region 
    public string[] DanMuStrings =
    {
        "这个剧情也太雷人了吧！",
        "还是好莱坞的电影经典啊，这个太次了还是好莱坞的电影经典啊，这个太次了",
        "是电锯惊魂的主角，尼玛",
        "这个游戏还是很良心的么",
        "卧槽还要花钱，这一关也太难卧槽还要花钱，这一关也太难了卧槽还要花钱，这一关也太难了卧槽还要花钱，这一关也太难了了",
        "这个游戏好棒偶",
        "全是傻逼",
        "求约：13122785566",
        "最近好寂寞啊，还是这个游戏好啊是胸再大点就更是胸再大点就更是胸再大点就更",
        "难道玩游戏还能撸",
        "办证：010 - 888888",
        "为什么女主角没有死？",
        "好帅呦，你这个娘们儿",
        "欠揍啊，东北人不知道啊",
        "我去都是什么人啊，请文明用语还是好莱坞的电影经典啊，这个太次了是胸再大点就更",
        "这个还是不错的",
        "要是胸再大点就更好了",
        "这个游戏必须顶啊",
        "还是好莱坞的电影经典啊，这个太次了还是好莱坞的电影经典啊，这个太次了怎么没有日本动作爱情片中的角色呢？",
        "好吧，这也是醉了！",
        "他只想做一个安静的美男子！"
    };
    #endregion
    public void createDanMuEntity()//生成弹幕和移动弹幕
    {
        texts = Instantiate(textsdanmu) as GameObject;
        texts.transform.position = textpositon;
        texts.transform.rotation = textrotation;

        if (texts != null)
        {
            texts.transform.SetParent(this.transform);//设置父物体

            texts.transform.localScale = new Vector3(1, 1, 1);
            textrotation.eulerAngles = new Vector3(0, 0, 0);
            texts.transform.localRotation = textrotation;

            texts.transform.localPosition = new Vector3(1150, Row_index, 0);//--弹幕移动的起始位置X           

            texts.transform.GetComponent<Text>().text = content;    //弹幕内容  

            StartCoroutine(move(texts));
        }
    }

    /// <summary>
    /// 弹幕移动
    /// </summary>
    private IEnumerator move(GameObject texts_)
    {
        Vector3 target = new Vector3(-15f, texts_.transform.position.y, texts_.transform.position.z);

        while(true)
        {
            if (texts_.transform.position.x > target.x)
            {
                Vector3 cu = texts_.transform.position;
                texts_.transform.position = Vector3.MoveTowards(texts_.transform.position, target, Time.deltaTime * 2f);
            }
            else
            {
                Destroy(texts_);
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}