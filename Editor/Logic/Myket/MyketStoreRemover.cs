#if UNITY_EDITOR

using Mirka.Payment.Editor.Editor.Tools;

namespace Mirka.Payment.Editor.Editor.Logic.Myket
{
    public static class MyketStoreRemover
    {
        public static void Remove()
        {
            HideMyketFiles();
            RemoveAndroidManifestDependencies();
        }

        private static void RemoveAndroidManifestDependencies()
        {
            FileTools.SetFileHidden("Assets/Plugins/Android/AndroidManifest.xml", true);
            FileTools.ToggleXmlCommentBlockByTagName("Assets/Plugins/Android/AndroidManifest.xml", "<queries>",
                "</queries>",
                true);
            FileTools.ToggleXmlCommentBlockByTagName("Assets/Plugins/Android/AndroidManifest.xml",
                "<receiver android:name=\"com.myket.util.IABReceiver\" android:exported=\"true\">",
                "</receiver",
                true);
            FileTools.ToggleXmlCommentByKeyword("Assets/Plugins/Android/AndroidManifest.xml",
                "ir.mservices.market.BILLING", true);
        }

        private static void HideMyketFiles()
        {
            FileTools.SetFileHidden("Assets/MyketIAB", true);
            FileTools.SetFileHidden("Assets/Plugins/Android/MyketIAB.jar", true);
        }
    }
}
#endif