using System;
using UnityEngine;

namespace RayCastSystem
{
    [Serializable]
    public class RayCastSettings
    {
        [field: SerializeField] public LayerMask LayerMask { get; private set; }
        [field: SerializeField] public float MaxDistance { get; private set; } = 10f;
    }
}