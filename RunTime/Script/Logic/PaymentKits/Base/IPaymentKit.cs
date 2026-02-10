using System;

namespace Mirka.Payment.Runtime.Script.Logic.PaymentKits.Base
{
    public interface IPaymentKit
    {
        public void Initialize(PaymentKitConfig paymentKitConfig);
        public void Purchase(Product product, Action<Product> onPurchaseSuccess, Action<Product> onPurchaseFailed);
    }
}