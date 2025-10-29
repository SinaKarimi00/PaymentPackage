using UnityEngine;

namespace Mirka.Payment.Runtime.Script.Logic
{
    [CreateAssetMenu(fileName = "PaymentKitConfig", menuName = "Payment/PaymentKitConfig")]
    public class PaymentKitConfig : ScriptableObject
    {
        [SerializeField] private string marketKey;
        public string MarketKey => marketKey;
    }
}