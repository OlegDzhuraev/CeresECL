using System;
using UnityEngine;

namespace CeresECL
{
    [CreateAssetMenu(fileName = "CeresSettings", menuName = "Ceres ECL/Settings Asset")]
    public class CeresSettings : ScriptableObject
    {
	    public static CeresSettings Instance
        {
	        get
	        {
		        if (!instance)
			        instance = Resources.Load("CeresSettings") as CeresSettings;

		        return instance;
	        }
        }
        
        static CeresSettings instance;
        
        public Type TagsEnum;

        [Tooltip("Adds Quck Entity add button and shows null and empty fields.")]
        public bool UseCustomInspectorHeader = true;
        
        [Tooltip("Highlights in hierarchy objects with Components which have null and empty fields.")]
        public bool UseCustomHierarchy = true;
    }
}