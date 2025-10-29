using com.mirka.payment.RunTime.Script.HumbleObject;
using Mirka.Payment.Runtime.Script.Logic.PaymentKits.Base;

namespace Mirka.Payment.Runtime.Script.Logic
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class PaymentContainer
    {
        private static readonly Dictionary<string, PurchaseButton> products = new();
        private static IPaymentKit _paymentKit;
        private static PaymentWrapper _paymentWrapper;

        public static IPaymentKit PaymentKit
        {
            get => _paymentKit;
            set => _paymentKit ??= value;
        }

        public static PaymentWrapper PaymentWrapper
        {
            get => _paymentWrapper;
            set => _paymentWrapper ??= value;
        }

        public static void RegisterProduct(string sku, PurchaseButton product)
        {
            if (IsSkuNull(sku)) return;

            IsSkuExist(sku);

            products[sku] = product;
        }

        private static void IsSkuExist(string sku)
        {
            if (products.ContainsKey(sku))
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

        public static PurchaseButton GetProduct(string sku)
        {
            products.TryGetValue(sku, out var product);
            return product;
        }

        public static IReadOnlyDictionary<string, PurchaseButton> AllPurchaseButtons => products;
    }
}