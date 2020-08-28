using System;
using UnityEditor;
using UnityEngine;

namespace CeresECL
{
    [CustomEditor(typeof(Entity), true)]
    public class EntityEditor : Editor
    {
        GUIStyle panelStyle, headerPanelStyle;

        int selectedLogic;

        void OnEnable()
        {
            panelStyle = new GUIStyle();
            panelStyle.padding = new RectOffset(5, 5, 5, 5);
            var panelColor = EditorGUIUtility.isProSkin ? new Color(0.25f, 0.25f, 0.25f) : new Color(0.9f, 0.9f, 0.9f);
            panelStyle.normal.background = EditorHelpers.GetColoredTexture(panelColor);
            
            headerPanelStyle = new GUIStyle(panelStyle);
            var headerPanelColor = EditorGUIUtility.isProSkin ? new Color(0.3f, 0.275f, 0.4f) :new Color(0.8f, 0.8f, 0.9f);
            headerPanelStyle.normal.background = EditorHelpers.GetColoredTexture(headerPanelColor);
        }

        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                GUILayout.BeginVertical(panelStyle);
                GUILayout.Label("Entity active only in runtime", EditorStyles.boldLabel);
                GUILayout.EndVertical();
                return;
            }
            
            var entity = target as Entity;

            DrawTags(entity);
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

            if (!Application.isPlaying || entity.Tags == null)
            {
                GUILayout.Label("Tags work only on scene and in runtime", EditorStyles.boldLabel);
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
    }
}