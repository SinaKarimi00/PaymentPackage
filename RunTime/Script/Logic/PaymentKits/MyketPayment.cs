using System;
using UnityEngine;
#if MYKET
using MyketPlugin;
#endif
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using Mirka.Payment.Runtime.Script.Tools;
using Mirka.Payment.Runtime.Script.Logic.PaymentKits.Base;

namespace Mirka.Payment.Runtime.Script.Logic.PaymentKits
{
    public class MyketPayment : MonoBehaviour, IPaymentKit
    {
        private readonly List<string> _allSku = new();
        private bool _isDisposed;
        private PaymentKitConfig _payKitConfig;
        private Action<Product> _onPurchaseSuccess;
        private Action<Product> _onPurchaseFailed;


#if !UNITY_EDITOR && MYKET
        public async void Initialize(PaymentKitConfig payment)
        {
            _payKitConfig = payment;
            var productCollection = PaymentContainer.AllPurchaseButtons;
            foreach (var keyValuePair in productCollection)
                _allSku.Add(keyValuePair.Key);

            Debug.Log("MyketPayKit initialized");
            MyketIAB.querySkuDetails(_allSku.ToArray());
            Debug.Log("QuerySkuDetails initialized");
        }

        public void Purchase(Product product, Action<Product> onPurchaseSuccess, Action<Product> onPurchaseFailed)
        {
            _onPurchaseSuccess = onPurchaseSuccess;
            _onPurchaseFailed = onPurchaseFailed;
            MyketIAB.purchaseProduct(product.id);
        }

        private void Awake()
        {
            _payKitConfig = Addressables.LoadAssetAsync<PaymentKitConfig>("MyketPaymentKitConfig").WaitForCompletion();
        }

        private void OnDestroy()
        {
            Addressables.Release(_payKitConfig);
        }

        private void OnEnable()
        {
            Debug.Log("MyketPayKit enabled");
            IABEventManager.querySkuDetailsSucceededEvent += OnSkuDetailsReceived;
            IABEventManager.querySkuDetailsFailedEvent += OnSkuDetailsFailed;
            IABEventManager.purchaseSucceededEvent += PurchaseSuccess;
            IABEventManager.purchaseFailedEvent += PurchaseFailed;

            MyketIAB.init(_payKitConfig.MarketKey);
            Debug.Log("MyketPayKit initialized in onEnable");
        }

        private void OnSkuDetailsReceived(List<MyketSkuInfo> skuDetails)
        {
            Debug.Log("OnSkuDetailsReceived");
            Debug.Log(skuDetails.Count + " : ***");
            foreach (var sku in skuDetails)
            {
                var productPrice = sku.Price;
                var convertedPrice = PriceConvertor.ExtractPriceFromPersianMarkets(productPrice);
                Debug.Log(convertedPrice + " : converted ***");
                Debug.Log(sku.ProductId + " : product id");

                PaymentContainer.GetPurchaseButton(sku.ProductId).Product.price = convertedPrice;
                PaymentContainer.GetPurchaseButton(sku.ProductId).UpdatePrice(convertedPrice);
            }
        }

        private void OnSkuDetailsFailed(string error)
        {
            Debug.Log("OnSkuDetailsFailed ");
        }

        private void PurchaseSuccess(MyketPurchase product)
        {
            var id = product.ProductId;

            var price = PaymentContainer.GetPurchaseButton(id).Product.price;
            price = price.Replace(",", "");
            var exactRevenue = float.Parse(price);

            var localProduct = new Product()
            {
                id = product.ProductId,
                price = exactRevenue.ToString(),
            };

            _onPurchaseSuccess?.Invoke(localProduct);

            Debug.Log("MyketPayKit successfully purchased");

            MyketIAB.consumeProduct(id);
        }


        private void PurchaseFailed(string sku)
        {
            var localProduct = new Product()
            {
                id = sku,
            };
            _onPurchaseFailed?.Invoke(localProduct);
            Debug.Log("MyketPayKit failed to purchase");
        }


        private void OnDisable()
        {
            IABEventManager.querySkuDetailsSucceededEvent -= OnSkuDetailsReceived;
            IABEventManager.querySkuDetailsFailedEvent -= OnSkuDetailsFailed;
            IABEventManager.purchaseSucceededEvent -= PurchaseSuccess;
            IABEventManager.purchaseFailedEvent -= PurchaseFailed;
        }

#else
        public async void Initialize(PaymentKitConfig payment)
        {
        }

        public void Purchase(Product product, Action<Product> onPurchaseSuccess, Action<Product> onPurchaseFailed)
        {
        }
#endif
    }
}