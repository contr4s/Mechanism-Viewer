using ObjectPool.PoolExpansionStrategies;
using UnityEngine;

namespace ObjectPool
{
    public interface IPoolableObjectProvider
    {
        void RegisterPoolable<T>(T prefab, int startCount, IPoolExpansionStrategy poolExpansionStrategy = null)
                where T : MonoBehaviour, IPoolable;
        
        T GetFromPool<T>(T prefab) where T : MonoBehaviour, IPoolable;
        
        void ReturnToPool<T>(T poolable) where T : MonoBehaviour, IPoolable;
        void ReturnAllInstancesToPool<T>(T poolable) where T : MonoBehaviour, IPoolable;
    }
}