using ObjectPool.PoolExpansionStrategies;
using UnityEngine;
using Zenject;

namespace ObjectPool
{
    public class PoolInstaller : MonoInstaller<PoolInstaller>
    {
        [SerializeField] private PoolContainersHolder _containersHolder;
        [SerializeField] private int _constantPoolExpansion = 1;
        
        public override void InstallBindings()
        {
            Container.Bind<PoolContainersHolder>().FromInstance(_containersHolder).AsSingle();
            Container.Bind<IPoolExpansionStrategy>().FromInstance(new ConstantPoolExpansion(_constantPoolExpansion))
                     .WhenInjectedInto<PoolableObjectProvider>();
            Container.BindInterfacesTo<PoolableObjectProvider>().AsSingle();
        }
    }
}