using System;
using System.Collections.Generic;

namespace Mirka.Payment.Runtime.Script.Logic
{
    [Serializable]
    public class Product
    {
        public string id;
        public bool isConsumable;
        public List<Payout> payouts;
        public ProductType productType;
        public string price;
        
        public Product(string id, bool isConsumable, List<Payout> payouts, ProductType productType, string price)
        {
            this.id = id;
            this.isConsumable = isConsumable;
            this.payouts = payouts;
            this.productType = productType;
            this.price = price;
        }
        public Product()
        {
        }

    }
}