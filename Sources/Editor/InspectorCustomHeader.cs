using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CeresECL.Misc
{
    [InitializeOnLoad]
    public static class InspectorCustomHeader
    {
        static InspectorCustomHeader()
        {
            Editor.finishedDefaultHeaderGUI += DrawCeresExtension;
        }

        static void DrawCeresExtension(Editor editor)
        {
            if (editor.target is GameObject gameObject)
            {
                var entity = gameObject.GetComponent<Entity>();

                if (entity)
                {
                    DrawEmptyFieldWarning(gameObject);
                    return;
                }

                if (GUILayout.Button("Make it Entity"))
                {
                    gameObject.AddComponent<Entity>();
                    EditorUtility.SetDirty(editor.target);
                }
            }
        }

        static void DrawEmptyFieldWarning(GameObject gameObject)
        {
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
                    
                    if (Attribute.IsDefined(field, typeof(RuntimeOnlyAttribute)))
                        continue;
                    
                    var value = field.GetValue(monoBehs[i]);
                    
                    if (value == null || value.Equals(null))
                    {
                        var style = new GUIStyle();
                        style.normal.textColor = new Color(1f, 0.75f, 0f);
                        style.fontStyle = FontStyle.Bold;
                        
                        GUILayout.Label("Field " + field.Name + " of " + type.Name + " is Empty.", style);
                    }
                }
            }
        }
    }
}