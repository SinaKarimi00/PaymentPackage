using System;

namespace Mirka.Payment.Runtime.Script.Logic
{
    [Serializable]
    public class Payout
    {
        public string subtype;
        public int quantity;

        public Payout(string subtype, int quantity)
        {
            this.subtype = subtype;
            this.quantity = quantity;
        }
    }
}