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
    }
}