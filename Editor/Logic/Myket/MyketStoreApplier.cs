#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Mirka.Payment.Editor.Editor.Tools;

namespace Mirka.Payment.Editor.Editor.Logic.Myket
{
    public static class MyketStoreApplier
    {
        public static void Apply()
        {
            UnHideMyketFiles();
            ReturnManifestDependencies();
            SetSymbol();
        }

        private static void SetSymbol()
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "MYKET");
            Debug.Log("MYKET flag set.");

            var currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);

            if (currentSymbols.Contains("CAFEBAZAAR"))
            {
                currentSymbols = currentSymbols.Replace("CAFEBAZAAR", "");

                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, currentSymbols);
            }
            else
            {
                Debug.LogWarning("CAFEBAZAAR flag not found.");
            }
        }

        private static void ReturnManifestDependencies()
        {
            FileTools.SetFileHidden("Assets/Plugins/Android/AndroidManifest.xml", false);
            FileTools.ToggleXmlCommentBlockByTagName("Assets/Plugins/Android/AndroidManifest.xml", "<queries>",
                "</queries>",
                false);
            FileTools.ToggleXmlCommentBlockByTagName("Assets/Plugins/Android/AndroidManifest.xml",
                "<receiver android:name=\"com.myket.util.IABReceiver\" android:exported=\"true\">",
                "</receiver",
                false);
            FileTools.ToggleXmlCommentByKeyword("Assets/Plugins/Android/AndroidManifest.xml",
                "ir.mservices.market.BILLING", false);
        }

        private static void UnHideMyketFiles()
        {
            FileTools.SetFileHidden("Packages/PaymentPackage-0.1.2/Packages/.MyketIAB", false);
            FileTools.SetFileHidden("Assets/Plugins/Android/.MyketIAB.jar", false);
        }
    }
}
#endif