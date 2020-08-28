using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace CeresECL
{
    public static class TemplatesGenerator
    {
        [MenuItem ("Assets/Create/Ceres ECL/Component template", false, -199)]
        static void CreateComponentTemplate() => CreateAnyFromTemplate("Component");
        
        [MenuItem ("Assets/Create/Ceres ECL/Init Logic template", false, -199)]
        static void CreateInitLogicTemplate() => CreateAnyFromTemplate("InitLogic");
        
        [MenuItem ("Assets/Create/Ceres ECL/Run Logic template", false, -199)]
        static void CreateRunLogicTemplate() => CreateAnyFromTemplate("RunLogic");

        [MenuItem ("Assets/Create/Ceres ECL/Event template", false, -199)]
        static void CreateEventTemplate() => CreateAnyFromTemplate("Event");
        
        [MenuItem ("Assets/Create/Ceres ECL/Launcher template", false, -199)]
        static void CreateLauncherTemplate() => CreateAnyFromTemplate("GameLauncher"); 
        
        [MenuItem ("Assets/Create/Ceres ECL/Scriptable Object template", false, -99)]
        static void CreateScriptableObjectTemplate() => CreateAnyFromTemplate("ScriptableObject");
        
        static void CreateAnyFromTemplate(string templateName)
        {
            var fileName = $"{GetSelectionPath()}/Ceres{templateName}.cs";
            CreateAndRenameAsset(fileName, GetIcon(), name => CreateTemplateInternal(GetTemplateContent(templateName), name));
        }

        static string CreateFromTemplate(string template, string fileName) 
        {
            if (string.IsNullOrEmpty(fileName)) 
                return "Invalid filename";
            
            var namespaceName = EditorSettings.projectGenerationRootNamespace.Trim();
            
            if (string.IsNullOrEmpty(EditorSettings.projectGenerationRootNamespace)) 
                namespaceName = "CeresECL";
            
            template = template.Replace("#NAMESPACE#", namespaceName);
            template = template.Replace("#SCRIPTNAME#", SanitizeClassName(Path.GetFileNameWithoutExtension(fileName)));
            
            try 
            {
                File.WriteAllText(AssetDatabase.GenerateUniqueAssetPath(fileName), template);
            } 
            catch (Exception ex) 
            {
                return ex.Message;
            }
            
            AssetDatabase.Refresh ();
            
            return null;
        }
        
        static string SanitizeClassName(string className) 
        {
            var sb = new StringBuilder();
            var needUp = true;
            
            foreach (var c in className) 
            {
                if (char.IsLetterOrDigit(c)) 
                {
                    sb.Append (needUp ? char.ToUpperInvariant(c) : c);
                    needUp = false;
                } 
                else 
                {
                    needUp = true;
                }
            }
            
            return sb.ToString();
        }
        
        static string GetSelectionPath() 
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            
            if (!string.IsNullOrEmpty(path) && AssetDatabase.Contains(Selection.activeObject)) 
            {
                if (!AssetDatabase.IsValidFolder(path))
                    path = Path.GetDirectoryName(path);
            } 
            else 
            {
                path = "Assets";
            }
            
            return path;
        }
        
        static string CreateTemplateInternal(string template, string fileName) 
        {
            var result = CreateFromTemplate(template, fileName);
            if (result != null) 
                EditorUtility.DisplayDialog("Ceres ECL", result, "Close");
            
            return result;
        }

        static Texture2D GetIcon() => EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;

        static void CreateAndRenameAsset(string fileName, Texture2D icon, Action<string> onSuccess) 
        {
            var action = ScriptableObject.CreateInstance<CustomEndNameAction>();
            action.Callback = onSuccess;
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, action, fileName, icon, null);
        }
        
        static string GetTemplateContent(string templateName)
        {
            var templates = new List<TextAsset>();
            EditorHelpers.LoadAssetsToList(templates, "l: CeresTemplate");

            var template = templates.Find(t => t.name.Contains(templateName));

            return template ? template.text : "No template found";
        }

        sealed class CustomEndNameAction : EndNameEditAction 
        {
            [NonSerialized] public Action<string> Callback;

            public override void Action(int instanceId, string pathName, string resourceFile) 
            {
                Callback?.Invoke(pathName);
            }
        }
    }
}