using System.Collections.Generic;
using UnityEngine;

namespace RayCastSystem
{
    public abstract class TriggerProcessor<T> : ITriggerProcessor
            where T : IRayCastTrigger
    {
        bool ITriggerProcessor.CanProcess(IRayCastTrigger trigger) => trigger is T;
        
        void ITriggerProcessor.StartProcessing(IRayCastTrigger trigger)
        {
            if (trigger is not T genericTrigger)
            {
                Debug.LogError($"{this} can't process {trigger}");
                return;
            }
            
            StartProcessing(genericTrigger);
        }

        void ITriggerProcessor.AbortProcessing(IRayCastTrigger trigger)
        {
            if (trigger is not T genericTrigger)
            {
                Debug.LogError($"{this} can't process {trigger}");
                return;
            }
            
            AbortProcessing(genericTrigger);
        }

        protected abstract void StartProcessing(T trigger);
        protected abstract void AbortProcessing(T trigger);
    }
    
    public abstract class TriggerProcessor<TTrigger, TData> : TriggerProcessor<TTrigger>
            where TTrigger : IRayCastTrigger
    {
        private readonly Dictionary<TTrigger, TData> _data = new();
        
        protected abstract TData GetData(TTrigger trigger);

        protected  abstract void StartProcessing(TTrigger trigger, TData data);
        protected sealed override void StartProcessing(TTrigger trigger)
        {
            TData data = GetData(trigger);
            if (_data.TryGetValue(trigger, out TData _))
            {
                _data[trigger] = data;
            }
            else
            {
                _data.Add(trigger, data);
            }
            StartProcessing(trigger, _data[trigger]);
        }
        
        protected abstract void AbortProcessing(TTrigger trigger, TData data);
        protected sealed override void AbortProcessing(TTrigger trigger)
        {
            AbortProcessing(trigger, _data[trigger]);
            _data.Remove(trigger);
        }
    }
}