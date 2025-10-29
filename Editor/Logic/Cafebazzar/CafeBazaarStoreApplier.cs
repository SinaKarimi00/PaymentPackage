#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Mirka.Payment.Editor.Editor.Tools;

namespace Mirka.Payment.Editor
{
    public static class CafeBazaarStoreApplier
    {
        public static void Apply()
        {
            SetSymbol();
            UnhideCafebazaarFiles();
            ReturnBazaarToGradle();
        }

        private static void SetSymbol()
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "CAFEBAZAAR");
            Debug.Log("CAFEBAZAAR flag set.");

            var currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);

            if (currentSymbols.Contains("MYKET"))
            {
                currentSymbols = currentSymbols.Replace("MYKET", "");

                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, currentSymbols);
            }
            else
            {
                Debug.LogWarning("MYKET flag not found.");
            }
        }

        private static void ReturnBazaarToGradle()
        {
            FileTools.ToggleGradleCommentsByKeyword(
                "Assets/Plugins/Android/mainTemplate.gradle",
                "Bazaar",
                false);
        }

        private static void UnhideCafebazaarFiles()
        {
            FileTools.SetFileHidden("Packages/PaymentPackage-0.1.4/Packages/.Bazaar", false);
        }
    }
}
#endif