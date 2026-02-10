using com.mirka.payment.RunTime.Script.HumbleObject;
using Mirka.Payment.Runtime.Script.Logic.PaymentKits.Base;

namespace Mirka.Payment.Runtime.Script.Logic
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class PaymentContainer
    {
        private static readonly Dictionary<string, PurchaseButton> purchaseButtons = new();
        private static IPaymentKit _paymentKit;
        private static PaymentWrapper _paymentWrapper;

        public static IPaymentKit PaymentKit
        {
            get => _paymentKit;
            set => _paymentKit ??= value;
        }

        public static PaymentWrapper PaymentWrapper;


        public static void RegisterPurchaseButton(string sku, PurchaseButton purchaseButton)
        {
            if (IsSkuNull(sku)) return;

            IsSkuExist(sku);

            purchaseButtons[sku] = purchaseButton;
        }

        private static void IsSkuExist(string sku)
        {
            if (purchaseButtons.ContainsKey(sku))
            {
                Debug.LogWarning($"Product with SKU '{sku}' is already registered. Overwriting it.");
            }
        }

        private static bool IsSkuNull(string sku)
        {
            if (string.IsNullOrEmpty(sku))
            {
                Debug.LogWarning("Product registration failed: SKU is empty.");
                return true;
            }

            return false;
        }

        public static PurchaseButton GetPurchaseButton(string sku)
        {
            purchaseButtons.TryGetValue(sku, out var purchaseButton);
            return purchaseButton;
        }

        public static IReadOnlyDictionary<string, PurchaseButton> AllPurchaseButtons => purchaseButtons;
    }
}