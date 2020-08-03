using System;
using System.Reflection;
using CeresECL.Misc;
using UnityEditor;
using UnityEngine;

namespace CeresECL
{
    [CustomEditor(typeof(Entity))]
    public class EntityEditor : Editor
    {
        GUIStyle panelStyle, headerPanelStyle;

        int selectedLogic;

        void OnEnable()
        {
            panelStyle = new GUIStyle();
            panelStyle.padding = new RectOffset(5, 5, 5, 5);
            panelStyle.normal.background = EditorHelpers.GetColoredTexture(new Color(0.9f, 0.9f, 0.9f));
            
            headerPanelStyle = new GUIStyle(panelStyle);
            headerPanelStyle.normal.background = EditorHelpers.GetColoredTexture(new Color(0.8f, 0.8f, 0.9f));
        }

        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                GUILayout.BeginVertical(panelStyle);
                GUILayout.Label("Adding Entity component in inspector currently isn't supported. Do it from code.", EditorStyles.boldLabel);
                GUILayout.EndVertical();
                return;
            }
            
            var entity = target as Entity;
            var componentsList = entity.Components.GetListEditor();
            var tagsList = entity.Tags.GetTagsCopy();
            var logicsList = entity.Logics.GetListEditor();
            
            DrawHeader("Tags");
            
            GUILayout.BeginVertical(panelStyle);

            foreach (var keyValuePair in tagsList)
            {
                var tagName = keyValuePair.Key.ToString();
                
                if (CeresSettings.Instance.TagsEnum != null)
                    tagName = Enum.Parse(CeresSettings.Instance.TagsEnum, tagName).ToString();
                
                GUILayout.Label(tagName + ", Count: " + keyValuePair.Value, EditorStyles.boldLabel);
            }

            if (tagsList.Count == 0)
                GUILayout.Label("No tags on object", EditorStyles.boldLabel);
            
            GUILayout.EndVertical();
            
            DrawHeader("Components");
            
            for (var i = 0; i < componentsList.Count; i++)
            {
                GUILayout.BeginVertical(panelStyle);
                var component = componentsList[i];
                var type = component.GetType();
                
                GUILayout.Label(type.Name, EditorStyles.boldLabel);

                foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
                    DrawField(field, component);

                GUILayout.EndVertical();
            }

            if (componentsList.Count == 0)
            {
                GUILayout.BeginVertical(panelStyle);
                GUILayout.Label("No components on object", EditorStyles.boldLabel);
                GUILayout.EndVertical();
            }
            
            DrawHeader("Logics");
            
            for (var i = 0; i < logicsList.Count; i++)
            {
                GUILayout.BeginVertical(panelStyle);
                GUILayout.Label(logicsList[i].GetType().Name, EditorStyles.boldLabel);
                GUILayout.EndVertical();
            }
            
            if (logicsList.Count == 0)
            {
                GUILayout.BeginVertical(panelStyle);
                GUILayout.Label("No logics on object", EditorStyles.boldLabel);
                GUILayout.EndVertical();
            }
            
            /* For future improvements of editor
            var listOfLogics = (
                from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where assemblyType.IsSubclassOf(typeof(Logic))
                      && ! assemblyType.IsAbstract
                select assemblyType).ToArray();

            GUILayout.BeginHorizontal();
            
            var listOfStrings = Misc.EditorHelpers.TypesNamesToStrings(listOfLogics);
            selectedLogic = EditorGUILayout.Popup("Add Logic", selectedLogic, listOfStrings);
            if (GUILayout.Button("Add"))
            {
                entity.AddLogic<MoveLogic>(); // debug
                EditorUtility.SetDirty(target);
            }

            GUILayout.EndHorizontal();
            */
        }
        
        void DrawHeader(string title)
        { 
            GUILayout.BeginVertical(headerPanelStyle);
            GUILayout.Label(title, EditorStyles.boldLabel);
            GUILayout.EndVertical();
        }

        void DrawField(FieldInfo field, object obj)
        {
            var fieldValue = field.GetValue(obj);
            var fieldType = field.FieldType;
                    
            if (fieldType == typeof (UnityEngine.Object) || fieldType.IsSubclassOf (typeof (UnityEngine.Object))) 
            {
                GUILayout.BeginHorizontal ();
                GUILayout.Label (field.Name);
                var guiEnabled = GUI.enabled;
                GUI.enabled = false;
                EditorGUILayout.ObjectField (fieldValue as UnityEngine.Object, fieldType, false);
                GUI.enabled = guiEnabled;
                GUILayout.EndHorizontal ();
                return;
            }
                    
            var strVal = fieldValue != null ? string.Format (System.Globalization.CultureInfo.InvariantCulture, "{0}", fieldValue) : "null";
                    
            GUILayout.BeginHorizontal();
            GUILayout.Label(field.Name);
            GUILayout.Label(strVal);
            GUILayout.EndHorizontal();
        }
    }
}