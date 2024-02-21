using System;
using UnityEngine;

namespace Camera
{
    [Serializable]
    public class CameraSettings
    {
        [field: SerializeField] public float MinFov { get; private set; } = 15;
        [field: SerializeField] public float DefaultFov { get; private set; } = 40;
        [field: SerializeField] public float MaxFov { get; private set; } = 60;
        [field: SerializeField] public float FovResetTime { get; private set; } = 2;
        [field: SerializeField] public float FovResetSpeed { get; private set; } = 1;
        [field: SerializeField] public bool CanResetFov { get; private set; } = true;
    }
}