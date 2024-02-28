using RayCastSystem.Highlight;
using RayCastSystem.Highlight.Outline;
using RayCastSystem.Tooltip;
using UnityEngine;

namespace RayCastSystem.Triggers
{
    public class MechanismPart : MonoBehaviour, IHighlightTrigger, ITooltipTrigger
    {
        [SerializeField] private OutlineData _outlineData;
        
        public Transform Transform => transform;
        public OutlineData OutlineData => _outlineData;
    }
}