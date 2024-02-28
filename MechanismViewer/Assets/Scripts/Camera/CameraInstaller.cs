using Cinemachine;
using UnityEngine;
using Zenject;

namespace Camera
{
    public class CameraInstaller : MonoInstaller<CameraInstaller>
    {
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private CameraSettings _settings;
        
        public override void InstallBindings()
        {
            Container.Bind<CinemachineBrain>().FromInstance(_cinemachineBrain).AsSingle();
            Container.Bind<CameraSettings>().FromInstance(_settings).AsSingle();
            Container.BindInterfacesTo<CameraZoom>().AsSingle().NonLazy();
        }
    }
}