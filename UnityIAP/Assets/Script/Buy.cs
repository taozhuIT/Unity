using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;
using System.Collections.Generic;
using CompleteProject;

/// <summary>
/// 购买测试
/// </summary>
public class Buy : MonoBehaviour 
{
    // Unity统一支付
    PurchaserTao _purchaser = null;
    // 类型列表
    private List<GameObject> _typeList = new List<GameObject>();
    // 日志
    private Text _debug = null;
    // 购买money1
    private Button _money1 = null;
    // 购买money2
    private Button _money2 = null;
    // 购买money3
    private Button _money3 = null;
    // 购买money4
    private Button _money4 = null;
    // 购买money5
    private Button _money5 = null;
    // 购买money6
    private Button _money6 = null;
    // 购买vip1
    private Button _vip1 = null;
    // 购买vip30
    private Button _vip30 = null;
    // 购买vip90
    private Button _vip90 = null;
    // 清理日志
    private Button _removeB = null;
    // 打开网页
    private Button _openUrl = null;

	private bool isUis = false;

    /// <summary>
    /// 网络数据
    /// </summary>
    public class netInfo
    {
        // 地址
        public string url = null;
        // 第三方元数据
        public string receipt = null;
        // 成功回调
        public PurchaserTao.handler handler = null;

        /// <summary>
        /// 构造
        /// </summary>
        public netInfo(string url_, string receipt_ = null, PurchaserTao.handler handler_ = null)
        {
            url = url_;
            receipt = receipt_;
            handler = handler_;
        }
    }

    /// <summary>
    /// 初始
    /// </summary>
    private void Awake()
    {
        // 初始化UI
        Transform IAPShop = this.transform;

        _money1 = IAPShop.Find("money1").GetComponent<Button>();
        _money2 = IAPShop.Find("money2").GetComponent<Button>();
        _money3 = IAPShop.Find("money3").GetComponent<Button>();
        _money4 = IAPShop.Find("money4").GetComponent<Button>();
        _money5 = IAPShop.Find("money5").GetComponent<Button>();
        _money6 = IAPShop.Find("money6").GetComponent<Button>();
        _vip1 = IAPShop.Find("vip7").GetComponent<Button>();
        _vip30 = IAPShop.Find("vip30").GetComponent<Button>();
        _vip90 = IAPShop.Find("vip90").GetComponent<Button>();

        _removeB = IAPShop.parent.Find("RomeDebug").GetComponent<Button>();
        _openUrl = IAPShop.parent.Find("OpenUrl").GetComponent<Button>();

        _debug = IAPShop.parent.Find("Debug").GetComponent<Text>();

        // 事件监听
        _money1.onClick.AddListener(BuyMoney1);
        _money2.onClick.AddListener(BuyMoney2);
        _money3.onClick.AddListener(BuyMoney3);
        _money4.onClick.AddListener(BuyMoney4);
        _money5.onClick.AddListener(BuyMoney5);
        _money6.onClick.AddListener(BuyMoney6);
        _vip1.onClick.AddListener(BuyVip7);
        _vip30.onClick.AddListener(BuyVip30);
        _vip90.onClick.AddListener(BuyVip90);

		_removeB.onClick.AddListener(OnInit);
        _openUrl.onClick.AddListener(OpenUrl);
    }

    /// <summary>
    /// 起始
    /// </summary>
	private void Start () 
    {

	}

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
		// 移动平台
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			if (Input.touchCount == 1) {
				
				// 手指离开
				if (Input.GetTouch (0).phase == TouchPhase.Ended && isUis) {
					DebugBack ("手指离开");
				} else {
					isUis = !EventSystem.current.IsPointerOverGameObject (Input.GetTouch (0).fingerId);
				}

			}
		}
	}

    /// <summary>
    /// 初始化
    /// </summary>
	private void OnInit()
	{
		_purchaser = new PurchaserTao();
		_purchaser.Init(DebugBack, OnBuyFinish);
		DebugBack("start");
	}

    /// <summary>
    /// 打开网页
    /// </summary>
    private void OpenUrl()
    {
        Application.OpenURL("http://www.seagm.com/ewar");
    }

    /// <summary>
    /// 购买商品money1
    /// </summary>
    private void BuyMoney1()
    {
        _purchaser.BuyConsumable(PurchaserTao.money1);
    }

    /// <summary>
    /// 购买商品money2
    /// </summary>
    private void BuyMoney2()
    {
        _purchaser.BuyConsumable(PurchaserTao.money2);
    }

    /// <summary>
    /// 购买商品money3
    /// </summary>
    private void BuyMoney3()
    {
        _purchaser.BuyConsumable(PurchaserTao.money3);
    }

    /// <summary>
    /// 购买商品money4
    /// </summary>
    private void BuyMoney4()
    {
        _purchaser.BuyConsumable(PurchaserTao.money4);
    }

    /// <summary>
    /// 购买商品money5
    /// </summary>
    private void BuyMoney5()
    {
        _purchaser.BuyConsumable(PurchaserTao.money5);
    }

    /// <summary>
    /// 购买商品money6
    /// </summary>
    private void BuyMoney6()
    {
        _purchaser.BuyConsumable(PurchaserTao.money6);
    }

    /// <summary>
    /// 购买商品vip7
    /// </summary>
    private void BuyVip7()
    {
        _purchaser.BuyConsumable(PurchaserTao.vip7);
    }

    /// <summary>
    /// 购买商品vip30
    /// </summary>
    private void BuyVip30()
    {
        _purchaser.BuyConsumable(PurchaserTao.vip30);
    }

    /// <summary>
    /// 购买商品vip90
    /// </summary>
    private void BuyVip90()
    {
        _purchaser.BuyConsumable(PurchaserTao.vip90);
    }
    
    /// <summary>
    /// 支付完成回调
    /// </summary>  
    private void OnBuyFinish(object obj_)
    {
        PurchaserTao.payData paydata = (PurchaserTao.payData)obj_;

        string receipt = paydata.receipt;
        string url = "http://pay.mmoup.net/iap/google?player_id=48138";
        DebugBack(url);
        netInfo info = new netInfo(url, receipt, null);
        StartCoroutine(netWebPost(info));
    }

    /// <summary>
    /// 确认支付
    /// </summary>
    private IEnumerator netWebPost(netInfo netInfo_)
    {
        WWWForm form = new WWWForm();
        form.AddField("data", netInfo_.receipt);
        WWW net = new WWW(netInfo_.url, form);

        yield return net;

        if (net.error == null)
        {
            DebugBack("游戏服务器确认完成");
            DebugBack(net.text);
            ConfirmPending();
        }
        else
        {
            DebugBack("游戏服务器确认错误" + net.error);
        }
    }

    /// <summary>
    /// 确认未完成的交易
    /// </summary>
    private void ConfirmPending()
    {
        _purchaser.ConfirmPending();
    }

    /// <summary>
    /// 清理日志
    /// </summary>
    private void OnClickDebug()
    {
        _debug.text = "";
    }

    /// <summary>
    /// 日志输出回调
    /// </summary>
    private void DebugBack(object obj_)
    {
        string debug = (string)obj_;
        _debug.text = _debug.text + "\n" + debug;
    }
}
