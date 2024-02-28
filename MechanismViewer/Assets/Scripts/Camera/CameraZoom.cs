using System;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Camera
{
    public class CameraZoom : IInitializable, IDisposable
    {
        private readonly CinemachineBrain _cinemachineBrain;
        private readonly CameraSettings _settings;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        
        public CameraZoom(CinemachineBrain cinemachineBrain, CameraSettings settings)
        {
            _cinemachineBrain = cinemachineBrain;
            _settings = settings;
        }

        void IInitializable.Initialize()
        {
            Observable.EveryUpdate()
                      .Where(_ => Input.mouseScrollDelta.y != 0)
                      .Select(_ => Input.mouseScrollDelta.y)
                      .SubscribeOn(Scheduler.MainThreadEndOfFrame)
                      .Subscribe(ZoomCamera)
                      .AddTo(_disposable);
        
            ResetFov(_cts.Token).Forget();
        }

        private void ZoomCamera(float x)
        {
            if (!_cinemachineBrain.TryGetCurrentCamera(out CinemachineFreeLook cam))
            {
                Debug.LogError("Current camera is not freeLook");
                return;
            }
        
            float currentFov = cam.m_Lens.FieldOfView;
            cam.m_Lens.FieldOfView = Mathf.Clamp(currentFov - x, _settings.MinFov, _settings.MaxFov);
        }

        private async UniTaskVoid ResetFov(CancellationToken ct)
        {
            float lastZoomTime = Time.time;
            while (!ct.IsCancellationRequested)
            {
                if (!_settings.CanResetFov)
                {
                    continue;
                }
            
                if (Input.mouseScrollDelta.y != 0)
                {
                    lastZoomTime = Time.time;
                }
                else if (Time.time - lastZoomTime > _settings.FovResetTime)
                {
                    if (!_cinemachineBrain.TryGetCurrentCamera(out CinemachineFreeLook cam))
                    {
                        Debug.LogError("Current camera is not freeLook");
                        return;
                    }

                    cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, 
                                                        _settings.DefaultFov,
                                                        _settings.FovResetSpeed * Time.deltaTime);
                }
            
                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
