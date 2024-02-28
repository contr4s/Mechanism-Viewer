using System;
using UnityEngine;

namespace RayCastSystem.Highlight.Outline
{
    [Serializable]
    public struct OutlineData
    {
        [field: SerializeField] public OutlineView OutlinePrefab { get; set; }
        [field: SerializeField] public float Thickness { get; set; }
        [field: SerializeField] public Mesh Mesh { get; set; }
    }
}