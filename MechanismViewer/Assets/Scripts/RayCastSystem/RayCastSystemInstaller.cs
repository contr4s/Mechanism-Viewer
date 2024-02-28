using Extensions;
using UnityEngine;
using Zenject;

namespace RayCastSystem
{
    public class RayCastSystemInstaller : MonoInstaller<RayCastSystemInstaller>
    {
        [SerializeField] private RayCastSettings _rayCastSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<RayCastSettings>().FromInstance(_rayCastSettings).AsSingle();
            Container.BindAllImplementationsOfType<ITriggerProcessor>();
            Container.BindInterfacesTo<RayCaster>().AsSingle().NonLazy();
        }
    }
}