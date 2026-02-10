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
        public Product Product;
        
        protected override void Awake()
        {
            base.Awake();
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
        }

        public void Initialize(Product product)
        {
            Product = product;
            PaymentContainer.RegisterPurchaseButton(product.id, this);
            onClick.AddListener(PurchaseProduct);
        }

        
        private void PurchaseProduct()
        {
            PaymentContainer.PaymentWrapper.PurchaseProduct(product);
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(PurchaseButton))]
        public class CustomIabButtonEditor : ButtonEditor
        {
            private SerializedProperty _product;

            protected override void OnEnable()
            {
                base.OnEnable();
                _product = serializedObject.FindProperty("product");
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                serializedObject.Update();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("PurchaseButton Settings", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(_product, new GUIContent("Product"));

                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}