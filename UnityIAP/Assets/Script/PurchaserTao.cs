using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Purchasing;

/// <summary>
/// unityIAP 应用内部支付
/// </summary>

namespace CompleteProject
{
    // 从IStoreListener买方类得到Unity内购消息。
    public class PurchaserTao : IStoreListener
    {
        // 委托
        public delegate void handler(object obj);
        // 日志回调
        public event handler Debugback;

        // 商店管理器
        private static IStoreController m_StoreController;            
        // 商店扩展信息 (比如在需要取消支付的时候，苹果是需要明确给出取消状态的)
        private static IExtensionProvider m_StoreExtensionProvider;
   
        // 当前我确认的商品
        private Product _confirmPending = null;

        // Google商店服务器对应商品标识
        public static string money1 = "money1";
        public static string money2 = "money2";
        public static string money3 = "money3";
        public static string money4 = "money4";
        public static string money5 = "money5";
        public static string money6 = "money6";
        public static string vip7 = "vip7";
        public static string vip30 = "vip30";
        public static string vip90 = "vip90";

        // 成功回调
        public handler callFinish;
        // 失败回调
        public handler callFailed;

        // 支付数据
        public class payData
        {
            // serverId 
            public string serverId = null;
            // 第三方元数据
            public string receipt = null;
        }

        // 支付数据列表
        private static Dictionary<string, payData> _payDataDict = new Dictionary<string, payData>();
        

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(PurchaserTao.handler back_, PurchaserTao.handler cinish_)
        {
            Debugback = back_;
            callFinish = cinish_;

            InitializePurchasing();
        }

        public void InitializePurchasing()
        {
            // 创建一个Unity的内购商店
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // 设置GoogleKey
            builder.Configure<IGooglePlayConfiguration>().SetPublicKey("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAoDk6qPB3nc/UThUegNE4u2jBw+Uc8cS7R8cRW6a/+VnLBAwjMwNe8TEq1y49ZSEUQtwXioqzG8O1V9bHXt7r+XpJWnTWBzA95W2ENmbLrRIuiVg+sHMG5WhhYpbEA7UHaWhfD3TW9XEZ6K3Y3hP548Z0rHNXu3Ttmwx3G6mZmthqWZLNDSh0QZlSyXLnWJXbsoxzMwlzY8TtdFCLOA8P0xNyeXMffKpdGtY1xzviBTYXTAAux8zYRxPAw4DPwLEEArJx+z1H/w3Lbo/gRPnLQq8zvzev3+wXkOX0qmnTmahBDOxcSPCAljcso+lzfmwm6H5o4eoPxpc2lCsKoVckAwIDAQAB");

            // 添加可消耗的商品    
            builder.AddProduct(money1, ProductType.Consumable, new IDs() { { money1, GooglePlay.Name }, });
            builder.AddProduct(money2, ProductType.Consumable, new IDs() { { money2, GooglePlay.Name }, });
            builder.AddProduct(money3, ProductType.Consumable, new IDs() { { money3, GooglePlay.Name }, });
            builder.AddProduct(money4, ProductType.Consumable, new IDs() { { money4, GooglePlay.Name }, });
            builder.AddProduct(money5, ProductType.Consumable, new IDs() { { money5, GooglePlay.Name }, });
            builder.AddProduct(money6, ProductType.Consumable, new IDs() { { money6, GooglePlay.Name }, });
            builder.AddProduct(vip7, ProductType.Consumable, new IDs() { { vip7, GooglePlay.Name }, });
            builder.AddProduct(vip30, ProductType.Consumable, new IDs() { { vip30, GooglePlay.Name }, });
            builder.AddProduct(vip90, ProductType.Consumable, new IDs() { { vip90, GooglePlay.Name }, });

            // 采购初始化 {OnInitialized 回调 初始化完成} {OnInitializeFailed 回调 初始化失败}  
            UnityPurchasing.Initialize(this, builder);
        }

        /// <summary>
        /// 采购是否初始化
        /// </summary>
        private bool IsInitialized()
        {
            // 是否无初始状态 (都等于空为初始状态)
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }

        /// <summary>
        /// 购买商品
        /// </summary>
        public void BuyConsumable(string prop_)
        {
            _confirmPending = null;
            _payDataDict.Clear();

            // 实例化支付数据
            payData pay = new payData();
            pay.serverId = prop_;
            pay.receipt = null;

            _payDataDict.Add(prop_, pay);
            Debugback("实例化支付数据完成------>" + prop_);
            BuyProductID(prop_);
        }

        /// <summary>
        /// 确认未消费的商品
        /// </summary>
        public void ConfirmPending()
        {
            if(_confirmPending != null)
            {
                // 确认消费
                m_StoreController.ConfirmPendingPurchase(_confirmPending);

                Debugback(string.Format("确认购买商品完成"));
            }
            else
            {
                Debugback(string.Format("商品为空"));
            }
        }

        /// <summary>
        /// 执行购买
        /// </summary>
        void BuyProductID(string productId)
        {
            try
            {
                if (IsInitialized())
                {
                    // 查找产品标识符
                    Product product = m_StoreController.products.WithID(productId);

                    // 产品是否可购买  availableToPurchase = 是否可以购买
                    if (product != null && product.availableToPurchase)
                    {
                        Debugback("执行购买");

                        // {回调==>ProcessPurchase 购买完成}  {回调==>OnPurchaseFailed 购买失败}
                        m_StoreController.InitiatePurchase(product);
                    }
                    else
                    {
                        Debugback("没有找到可购买的产品");
                    }
                }
                else
                {
                    Debugback("采购没有初始化");
                }
            }
            catch (Exception e)
            {
                Debugback("支付异常 " + e);
            }
        }


        /// <summary>
        /// 取消购买
        /// 苹果目前需要明确购买IAP恢复
        /// </summary>
        public void RestorePurchases()
        {
            // 购买是否已经初始化
            if (!IsInitialized())
            {
                // 购买没有被初始化
                Debugback("RestorePurchases FAIL. Not initialized.");
                return;
            }

            // 如果是苹果支付则需要明确取消状态
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                // 开始重置
                Debug.Log("RestorePurchases started ...");

                // 获取苹果支付状态
                var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                // 将购买重置
                apple.RestoreTransactions((result) =>
                {
                    // 输出重置是否完成
                    Debugback("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                });
            }
            else
            {
                Debugback("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }


        // ------------------------ IStoreListener 商店监听 ------------------------------

        /// <summary>
        /// 商店初始化完成
        /// </summary>
		public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debugback("支付初始化成功");

            for (int i = 0; i < controller.products.all.Length; ++i)
            {
                Debugback("币种-->" + controller.products.all[i].metadata.isoCurrencyCode);
                Debugback("货币-->" + controller.products.all[i].metadata.localizedPriceString);
            }

            m_StoreController = controller;
            m_StoreExtensionProvider = extensions;
        }

        /// <summary>
        /// 商店初始化失败
        /// </summary>
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debugback("支付初始化失败" + error);
        } 

        /// <summary>
        /// 购买成功
        /// </summary>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            _confirmPending = args.purchasedProduct;

            payData data = null;

            // 支付完成
            if(_payDataDict.Count > 0)
            {
                Debugback("当次支付成功");
                Debugback(args.purchasedProduct.receipt);

                data = _payDataDict[money1];
                data.receipt = args.purchasedProduct.receipt;
            }
            else
            {
                data = new payData();
                data.receipt = args.purchasedProduct.receipt;
                Debugback("上次支付成功");
            }

            callFinish(data);

            return PurchaseProcessingResult.Complete;
        }

        /// <summary>
        /// 购买失败
        /// </summary>
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
            Debugback(string.Format("购买失败", product.definition.storeSpecificId, failureReason));
        }

        // ------------------------ IStoreListener 商店监听 ------------------------------
    }
}
