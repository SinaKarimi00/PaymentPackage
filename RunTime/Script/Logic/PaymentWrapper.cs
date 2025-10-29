using System;
using Mirka.Payment.Runtime.Script.Logic.PaymentKits;
using UnityEngine;
using Mirka.Payment.Runtime.Script.Logic.PaymentKits.Base;

namespace Mirka.Payment.Runtime.Script.Logic
{
    public class PaymentWrapper : MonoBehaviour
    {
        private IPaymentKit _paymentKit;
        private Action<Product> _onPurchaseSuccess;
        private Action<Product> _onPurchaseFailed;

        private void Awake()
        {
            PaymentContainer.PaymentWrapper = this;
        }

        private void Start()
        {
#if MYKET
            _paymentKit = gameObject.AddComponent<MyketPayment>(); // Correct instantiation for MyketPayKit
#elif CAFEBAZAAR
            _paymentKit = gameObject.AddComponent<CafeBazaarPayment>(); // Correct instantiation for CafeBazaarPayKit
#endif
#if !UNITY_EDITOR
            _paymentKit.Initialize();
#endif
        }

        public void PurchaseProduct(Product product)
        {
            _paymentKit.Purchase(product, _onPurchaseSuccess, _onPurchaseFailed);
        }

        public void RegisterOnPurchaseSuccess(Action<Product> onPurchaseSuccess)
        {
            _onPurchaseSuccess += onPurchaseSuccess;
        }

        public void RegisterOnPurchaseFailed(Action<Product> onPurchaseFailed)
        {
            _onPurchaseFailed += onPurchaseFailed;
        }

        public void ResetOnPurchaseSuccess()
        {
            _onPurchaseSuccess = null;
        }

        public void ResetOnPurchaseFailed()
        {
            _onPurchaseFailed = null;
        }
    }
}