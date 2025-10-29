#if UNITY_EDITOR
using System;
using Mirka.Payment.Editor.Editor.Model;
using Mirka.Payment.Editor.Logic.Cafebazzar;
using Mirka.Payment.Editor.Editor.Logic.Myket;

namespace Mirka.Payment.Editor.Editor.Logic
{
    public static class PaymentSwitcherTool
    {
        public static Store Store { get; set; }

        public static void ChangeGamePublishStore(Store store)
        {
            try
            {
                switch (store)
                {
                    case Store.CafeBazaar:
                        MyketStoreRemover.Remove();
                        CafeBazaarStoreApplier.Apply();
                        break;
                    case Store.Myket:
                        CafeBazaarStoreRemover.Remove();
                        MyketStoreApplier.Apply();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(store), store, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
#endif