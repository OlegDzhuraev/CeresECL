using System;
using CeresECL.Misc;
using UnityEditor;
using UnityEngine;

namespace CeresECL
{
    [CustomEditor(typeof(Entity))]
    public class EntityEditor : Editor
    {
        GUIStyle panelStyle, headerPanelStyle;

        int selectedComponentType, selectedLogic;
        
        Type[] componentsTypes;
        string[] componentsNames;
        
        void OnEnable()
        {
            panelStyle = new GUIStyle();
            panelStyle.padding = new RectOffset(5, 5, 5, 5);
            panelStyle.normal.background = EditorHelpers.GetColoredTexture(new Color(0.9f, 0.9f, 0.9f));
            
            headerPanelStyle = new GUIStyle(panelStyle);
            headerPanelStyle.normal.background = EditorHelpers.GetColoredTexture(new Color(0.8f, 0.8f, 0.9f));
            
            componentsTypes = EditorHelpers.GetChildTypes<Component>();
            componentsNames =EditorHelpers.TypesNamesToStrings(componentsTypes);
        }

        public override void OnInspectorGUI()
        {
            var entity = target as Entity;

            DrawTags(entity);
            DrawComponents(entity);
            DrawLogics(entity);
        }
        
        void DrawHeader(string title)
        { 
            GUILayout.BeginVertical(headerPanelStyle);
            GUILayout.Label(title, EditorStyles.boldLabel);
            GUILayout.EndVertical();
        }
        
        void DrawTags(Entity entity)
        {
            DrawHeader("Tags");
            
            GUILayout.BeginVertical(panelStyle);

            if (!Application.isPlaying)
            {
                GUILayout.Label("Tags is not editable in editor now.", EditorStyles.boldLabel);
                GUILayout.EndVertical();
                return;
            }
            
            var tagsList = entity.Tags.GetTagsCopy();

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
        }

        void DrawComponents(Entity entity)
        {
            var componentsList = entity.Components.GetListEditor();

            DrawHeader("Components");

            for (var i = 0; i < componentsList.Count; i++)
            {
                var component = componentsList[i];
                
                GUILayout.BeginHorizontal(panelStyle);

                GUILayout.BeginVertical();
                var editor = CreateEditor(component);
                editor.DrawDefaultInspector();
                GUILayout.EndVertical();

                if (GUILayout.Button("Del"))
                {
                    entity.Components.Delete(component);
                    GUILayout.EndHorizontal();
                    break;
                }

                GUILayout.EndHorizontal();
            }

            if (componentsList.Count == 0)
            {
                GUILayout.BeginVertical(panelStyle);
                GUILayout.Label("No components on object", EditorStyles.boldLabel);
                GUILayout.EndVertical();
            }
            
            GUILayout.BeginHorizontal(panelStyle);
            
            selectedComponentType = EditorGUILayout.Popup("Add Component", selectedComponentType, componentsNames);
            
            if (GUILayout.Button("Add"))
            {
                entity.Components.Get(componentsTypes[selectedComponentType]);
                EditorUtility.SetDirty(target);
            }

            GUILayout.EndHorizontal();
        }

        void DrawLogics(Entity entity)
        {
            DrawHeader("Logics");
            
            if (!Application.isPlaying)
            {
                GUILayout.BeginVertical(panelStyle);
                GUILayout.Label("Logics is not editable in editor now.", EditorStyles.boldLabel);
                GUILayout.EndVertical();
                return;
            }
            
            var logicsList = entity.Logics.GetListEditor();

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
    }
}