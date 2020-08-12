using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CeresECL.Misc
{
    public static class EditorHelpers
    {
        public static Texture2D GetColoredTexture(Color color)
        {
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();

            return texture;
        }
        
        public static void LoadAssetsToList<T>(List<T> listToAddIn, string searchFilter) where T : UnityEngine.Object
        {
            listToAddIn.Clear();
            
            var assets = AssetDatabase.FindAssets(searchFilter);

            for (int i = 0; i < assets.Length; i++)
            {
                var asset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assets[i]), typeof(T)) as T;
                listToAddIn.Add(asset);
            }
        }

        public static Type[] GetChildTypes<T>()
        {
            var types = (
                from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where assemblyType.IsSubclassOf(typeof(T)) && ! assemblyType.IsAbstract
                select assemblyType).ToArray();

            return types;
        }

        public static string[] TypesNamesToStrings(Type[] types)
        {
            var names = new string[types.Length];

            for (var i = 0; i < types.Length; i++)
                names[i] = types[i].Name;
            
            return names;
        }
    }
}