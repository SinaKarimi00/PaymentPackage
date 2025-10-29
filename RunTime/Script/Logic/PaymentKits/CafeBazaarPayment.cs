using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using Mirka.Payment.Runtime.Script.Tools;
using Mirka.Payment.Runtime.Script.Logic.PaymentKits.Base;
#if CAFEBAZAAR
using Bazaar.Data;
using Bazaar.Poolakey;
using Bazaar.Poolakey.Data;
#endif

namespace Mirka.Payment.Runtime.Script.Logic.PaymentKits
{
    public class CafeBazaarPayment : MonoBehaviour, IPaymentKit
    {
#if !UNITY_EDITOR && CAFEBAZAAR
        private Bazaar.Poolakey.Payment _payment;
        private readonly List<string> _allSku = new();
        private PaymentKitConfig _payKitConfig;

        private void Awake()
        {
            _payKitConfig = Addressables.LoadAssetAsync<PaymentKitConfig>("CafebazaarPaymentKitConfig").WaitForCompletion();
        }

        public async void Initialize()
        {
            Debug.Log("Try CafeBazaarPayKit initialized");
            try
            {
                Debug.Log("CafeBazaarPayKit initialized");
                var securityCheck = SecurityCheck.Enable(_payKitConfig.MarketKey);
                var paymentConfig = new PaymentConfiguration(securityCheck);
                _payment = new Bazaar.Poolakey.Payment(paymentConfig);

                await _payment.Connect();
                await QuerySkuDetails();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async Task QuerySkuDetails()
        {
            var productCollection = PaymentContainer.AllPurchaseButtons;
            foreach (var keyValuePair in productCollection)
            {
                _allSku.Add(keyValuePair.Key);
                await _payment.GetSkuDetails(keyValuePair.Key, SKUDetails.Type.all, OnReceiveSkuDetails);
            }
        }

        private void OnReceiveSkuDetails(Result<List<SKUDetails>> result)
        {
            foreach (var detail in result.data)
            {
                var revenuePrice = PriceConvertor.ExtractPriceFromPersianMarkets(detail.price, 10);
                PaymentContainer.GetProduct(detail.sku).Product.price = revenuePrice;
                PaymentContainer.GetProduct(detail.sku).UpdatePrice(revenuePrice);
            }
        }

        public async void Purchase(Product product, Action<Product> onPurchaseSuccess, Action<Product> onPurchaseFailed)
        {
            try
            {
                var purchaseResult = await _payment.Purchase(product.id);
                if (purchaseResult.data.purchaseState == PurchaseInfo.State.Purchased)
                {
                    var consumeResult = await _payment.Consume(purchaseResult.data.purchaseToken);
                    if (consumeResult.status == Status.Success)
                    {
                        onPurchaseSuccess?.Invoke(product);
                    }
                    else
                    {
                        Debug.Log("consumeResult failed");
                        onPurchaseFailed?.Invoke(product);
                    }
                }
                else
                {
                    Debug.Log("purchaseResult failed");
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void OnDisable()
        {
            _payment.Disconnect();
        }

        private void OnDestroy()
        {
            // Addressables.Release(_payKitConfig);
        }
#else
        public void Initialize()
        {
        }

        public void Purchase(Product product, Action<Product> onPurchaseSuccess, Action<Product> onPurchaseFailed)
        {
        }
#endif
    }
}