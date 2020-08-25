using CeresECL.Misc;
using UnityEngine;

namespace CeresECL.Example
{
    public class MoveComponent : MonoBehaviour
    {
        public float Speed;
        [RuntimeOnly] public Vector3 Direction;
    }
}