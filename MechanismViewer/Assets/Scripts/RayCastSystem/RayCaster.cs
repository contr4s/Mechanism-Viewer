using System;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UnityEngine;
using Zenject;

namespace RayCastSystem
{
    public class RayCaster : IInitializable, IDisposable
    {
        private readonly List<ITriggerProcessor> _allProcessors;
        private readonly CinemachineBrain _cinemachineBrain;
        private readonly RayCastSettings _rayCastSettings;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly List<ITriggerProcessor> _currentProcessors = new List<ITriggerProcessor>();
        
        private IRayCastTrigger _currentTrigger;
        private Vector3 _lastMousePosition;
        
        public RayCaster(CinemachineBrain cinemachineBrain, IEnumerable<ITriggerProcessor> processors, RayCastSettings rayCastSettings)
        {
            _allProcessors = new List<ITriggerProcessor>(processors);
            _cinemachineBrain = cinemachineBrain;
            _rayCastSettings = rayCastSettings;
        }

        void IInitializable.Initialize()
        {
            Observable.EveryUpdate()
                      .Where(_ => Input.mousePosition != _lastMousePosition)
                      .Select(_ => Input.mousePosition)
                      .Subscribe(UpdateRayCaster)
                      .AddTo(_disposable);
        }
        
        private void UpdateRayCaster(Vector3 mousePos)
        {
            _lastMousePosition = mousePos;
            Ray ray = _cinemachineBrain.OutputCamera.ScreenPointToRay(mousePos);
            if (!Physics.Raycast(ray, out RaycastHit hit, _rayCastSettings.MaxDistance, _rayCastSettings.LayerMask))
            {
                ClearProcessors();
                return;
            }

            if (!hit.transform.TryGetComponent(out IRayCastTrigger rayCastTrigger))
            {
                ClearProcessors();
                return;
            }

            if (ReferenceEquals(_currentTrigger, rayCastTrigger))
            {
                return;
            }

            ClearProcessors();
            _currentTrigger = rayCastTrigger;
            foreach (ITriggerProcessor processor in _allProcessors)
            {
                if (processor.CanProcess(_currentTrigger))
                {
                    _currentProcessors.Add(processor);
                    processor.StartProcessing(_currentTrigger);
                }
            }
        }

        private void ClearProcessors()
        {
            foreach (ITriggerProcessor processor in _currentProcessors)
            {
                processor.AbortProcessing(_currentTrigger);
            }
            _currentProcessors.Clear();
            _currentTrigger = null;
        }

        void IDisposable.Dispose()
        {
            ClearProcessors();
            _disposable.Dispose();
        }
    }
}