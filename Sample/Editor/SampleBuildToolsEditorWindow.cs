#if UNITY_EDITOR

using Mirka.Payment.Editor.Editor.Logic;
using Mirka.Payment.Editor.Editor.Model;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace Mirka.Payment.Sample.Editor
{
    public class SampleBuildToolsEditorWindow : EditorWindow
    {
        private const float AspectRatio = 4f / 2f;

        [MenuItem("Mirka Payment Tools/Sample Build Tools")]
        public static void ShowWindow()
        {
            var window = GetWindow<SampleBuildToolsEditorWindow>();
            window.titleContent = new GUIContent("Sample Build Tools");
            window.minSize = new Vector2(350, 350 / AspectRatio);
            window.Show();
        }

        private void OnGUI()
        {
            ShowBaseBuildToolsInfo();
        }

        private void ShowBaseBuildToolsInfo()
        {
            GUILayout.BeginArea(new Rect(0, 0, 350, 750));
            GUILayout.BeginVertical();

            GUILayout.Space(30);
            PaymentSwitcherTool.Store =
                (Store)EnumPopup("Game Store:", PaymentSwitcherTool.Store, GUILayout.Width(350));

            GUILayout.Space(15);

            if (GUILayout.Button("Apply payment to the Build", GUILayout.Width(350), GUILayout.Height(22)))
                PaymentSwitcherTool.ChangeGamePublishStore(PaymentSwitcherTool.Store);

            GUI.backgroundColor = Color.black;

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
#endif