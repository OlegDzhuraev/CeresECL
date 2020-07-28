using System;
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
        
        static string[] TypesNamesToStrings(Type[] types)
        {
            var strings = new string[types.Length];
            
            for (var i = 0; i < types.Length; i++)
                strings[i] = types[i].Name;

            return strings;
        }
    }
}