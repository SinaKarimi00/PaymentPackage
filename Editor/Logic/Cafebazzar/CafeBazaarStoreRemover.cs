#if UNITY_EDITOR
using Mirka.Payment.Editor.Editor.Tools;

namespace Mirka.Payment.Editor.Logic.Cafebazzar
{
    public static class CafeBazaarStoreRemover
    {
        public static void Remove()
        {
            HideCafebazaarFiles();
            RemoveBazaarFromGradle();
        }

        private static void RemoveBazaarFromGradle()
        {
            FileTools.ToggleGradleCommentsByKeyword(
                "Assets/Plugins/Android/mainTemplate.gradle",
                "Bazaar",
                true);
        }

        private static void HideCafebazaarFiles()
        {
            FileTools.SetFileHidden("Packages/PaymentPackage-0.1.7/Packages/Bazaar", true);
        }
    }
}
#endif