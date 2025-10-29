using Mirka.Payment.Runtime.Script.Logic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.mirka.payment.RunTime.Script.HumbleObject
{
    public class PurchaseButton : Button
    {
        [SerializeField] protected Product product;
        [SerializeField] private RTLTextMeshPro priceText;

        public Product Product => product;

        protected override void Awake()
        {
            base.Awake();
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
            PaymentContainer.RegisterPurchaseButton(product.id, this);

            onClick.AddListener(PurchaseProduct);
        }

        private void PurchaseProduct()
        {
            PaymentContainer.PaymentWrapper.PurchaseProduct(product);
        }

        public void UpdatePrice(string price)
        {
            Debug.Log("UpdatePrice called");
            priceText.text = price;
        }


#if UNITY_EDITOR
        [CustomEditor(typeof(PurchaseButton))]
        public class CustomIabButtonEditor : ButtonEditor
        {
            private SerializedProperty _product;
            private SerializedProperty _priceText;

            protected override void OnEnable()
            {
                base.OnEnable();
                _product = serializedObject.FindProperty("product");
                _priceText = serializedObject.FindProperty("priceText");
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                serializedObject.Update();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("PurchaseButton Settings", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(_product, new GUIContent("Product"));
                EditorGUILayout.PropertyField(_priceText, new GUIContent("Price Text"));

                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}