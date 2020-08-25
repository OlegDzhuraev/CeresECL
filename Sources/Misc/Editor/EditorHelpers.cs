using System.Collections.Generic;
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
    }
}