using ObjectPool;
using RayCastSystem.Highlight.Outline;
using UnityEngine;

namespace RayCastSystem.Highlight
{
    public class HighlightTriggerProcessor : TriggerProcessor<IHighlightTrigger, OutlineView>
    {
        private readonly IPoolableObjectProvider _poolableObjectProvider;
        
        public HighlightTriggerProcessor(IPoolableObjectProvider poolableObjectProvider)
        {
            _poolableObjectProvider = poolableObjectProvider;
        }

        protected override OutlineView GetData(IHighlightTrigger trigger) 
            => _poolableObjectProvider.GetFromPool(trigger.OutlineData.OutlinePrefab);

        protected override void StartProcessing(IHighlightTrigger trigger, OutlineView data)
        {
            data.ApplyTo(trigger.Transform, trigger.OutlineData);
        }

        protected override void AbortProcessing(IHighlightTrigger trigger, OutlineView data)
        {
            _poolableObjectProvider.ReturnToPool(data);
        }
    }
}