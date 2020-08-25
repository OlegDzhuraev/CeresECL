using UnityEditor;
using UnityEngine;

namespace CeresECL.Misc
{
    [CustomPropertyDrawer(typeof(RuntimeOnlyAttribute))]
    public class RuntimeOnlyAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var previousGuiState = GUI.enabled;
            
            if (!Application.isPlaying)
                GUI.enabled = false;
            
            EditorGUI.PropertyField(position, property, label);

            GUI.enabled = previousGuiState;
        }
    }
}