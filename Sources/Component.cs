using UnityEngine;

namespace CeresECL
{
    /// <summary> Base class to derive your components from. Component should contain only data, no any logics implementations. </summary>
    public class Component : ScriptableObject
    {
        /// <summary> Parent entity of this component. </summary>
        [HideInInspector] public Entity Entity;
    }
}