using System;
using System.Collections.Generic;
using CeresECL.Misc;
using UnityEditor;
using UnityEngine;

namespace CeresECL
{
    [CustomEditor(typeof(Entity))]
    public class EntityEditor : Editor
    {
        GUIStyle panelStyle, headerPanelStyle, foldoutStyle;

        int selectedComponentType, selectedLogic;
        
        Type[] componentsTypes;
        string[] componentsNames;

        bool addComponentButtonEnabled;

        readonly Dictionary<Component, bool> foldedComponents = new Dictionary<Component, bool>();
        
        void OnEnable()
        {
            panelStyle = new GUIStyle();
            panelStyle.padding = new RectOffset(5, 5, 5, 5);
            var panelColor = EditorGUIUtility.isProSkin ? new Color(0.25f, 0.25f, 0.25f) : new Color(0.9f, 0.9f, 0.9f);
            panelStyle.normal.background = EditorHelpers.GetColoredTexture(panelColor);
            
            foldoutStyle = new GUIStyle();
            var foldoutHeaderColor = EditorGUIUtility.isProSkin ? new Color(0.325f, 0.29f, 0.25f) : new Color(0.9f, 0.8f, 0.7f);
            foldoutStyle.normal.background = EditorHelpers.GetColoredTexture(foldoutHeaderColor);
            foldoutStyle.fontStyle = FontStyle.Bold;
            foldoutStyle.normal.textColor = EditorGUIUtility.isProSkin ? new Color(0.75f, 0.75f, 0.75f) : Color.black;
            foldoutStyle.padding = new RectOffset(5, 5, 5, 5);
            
            headerPanelStyle = new GUIStyle(panelStyle);
            var headerPanelColor = EditorGUIUtility.isProSkin ? new Color(0.3f, 0.275f, 0.4f) :new Color(0.8f, 0.8f, 0.9f);
            headerPanelStyle.normal.background = EditorHelpers.GetColoredTexture(headerPanelColor);
            
            componentsTypes = EditorHelpers.GetChildTypes<Component>();
            componentsNames = EditorHelpers.TypesNamesToStrings(componentsTypes);
        }

        public override void OnInspectorGUI()
        {
            var entity = target as Entity;

            DrawTags(entity);
            EditorGUILayout.Space();
            DrawComponents(entity);
            EditorGUILayout.Space();
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
            var componentsList = entity.Components.GetComponentsEditor();

            DrawHeader("Components");

            for (var i = 0; i < componentsList.Count; i++)
            {
                var component = componentsList[i];

                if (!foldedComponents.ContainsKey(component))
                    foldedComponents.Add(component, true);

                if (GUILayout.Button(component.GetType().Name, foldoutStyle))
                    foldedComponents[component] = !foldedComponents[component];
                
                if (foldedComponents[component])
                {
                    GUILayout.BeginHorizontal(panelStyle);

                    GUILayout.BeginVertical();
                    var editor = CreateEditor(component);
                    editor.DrawDefaultInspector();
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.MaxWidth(82), GUILayout.Width(82), GUILayout.ExpandWidth(false));
                    if (GUILayout.Button("Del"))
                    {
                        entity.Components.Delete(component);
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                        break;
                    }

                    if (i > 0)
                    {
                        if (GUILayout.Button("Move Up"))
                        {
                            componentsList[i] = componentsList[i - 1];
                            componentsList[i - 1] = component;

                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();
                            break;
                        }
                    }

                    if (i < componentsList.Count - 1)
                    {
                        if (GUILayout.Button("Move Down"))
                        {
                            componentsList[i] = componentsList[i + 1];
                            componentsList[i + 1] = component;

                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();
                            break;
                        }
                    }

                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();
                }
            }

            if (componentsList.Count == 0)
            {
                GUILayout.BeginVertical(panelStyle);
                GUILayout.Label("No components on object", EditorStyles.boldLabel);
                GUILayout.EndVertical();
            }
            
            GUILayout.BeginHorizontal(panelStyle);
            
            var prevActive = GUI.enabled;
            
            EditorGUI.BeginChangeCheck();
            selectedComponentType = EditorGUILayout.Popup("Add Component", selectedComponentType, componentsNames);

            if (EditorGUI.EndChangeCheck())
                addComponentButtonEnabled = !entity.Components.Have(componentsTypes[selectedComponentType]);

            GUI.enabled = addComponentButtonEnabled;
            
            if (GUILayout.Button("Add"))
            {
                entity.Components.Get(componentsTypes[selectedComponentType]);
                EditorUtility.SetDirty(target);
                
                addComponentButtonEnabled = false;
            }
            
            GUI.enabled = prevActive;
            
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