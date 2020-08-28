using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CeresECL.Misc
{
    [InitializeOnLoad]
    public class CeresHierarchy : MonoBehaviour
    {
        static CeresHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }
        
        static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            if (!CeresSettings.Instance || !CeresSettings.Instance.UseCustomHierarchy)
                return;
            
            var gameObject = EditorUtility.InstanceIDToObject (instanceID) as GameObject;

            if (!gameObject)
                return;
            
            var monoBehs = gameObject.GetComponents<MonoBehaviour>();
                    
            for (var i = 0; i < monoBehs.Length; i++)
            {
                var type = monoBehs[i].GetType();
                
                if (type.IsAssignableFrom(typeof(ExtendedBehaviour))) // we ignoring logics
                    continue;
               
                foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (Attribute.IsDefined(field, typeof(HideInInspector)))
                        continue;
                    
                    if (Attribute.IsDefined(field, typeof(RuntimeOnlyAttribute)) || Attribute.IsDefined(field, typeof(ReadOnlyAttribute)))
                        continue;

                    var value = field.GetValue(monoBehs[i]);
                    
                    if (value == null || value.Equals(null))
                    {
                        EditorGUI.DrawRect(selectionRect, new Color(1f, 0.5f, 0f, 0.25f));
                        return;
                    }
                }
            }
        }
    }
}