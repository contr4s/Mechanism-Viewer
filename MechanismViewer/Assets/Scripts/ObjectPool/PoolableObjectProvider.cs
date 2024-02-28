using System;
using System.Collections.Generic;
using ObjectPool.PoolExpansionStrategies;
using UnityEngine;

namespace ObjectPool
{
    public class PoolableObjectProvider : IPoolableObjectProvider, IDisposable
    {
        private readonly IPoolExpansionStrategy _defaultPoolExpansionStrategy;
        private readonly PoolContainersHolder _containersHolder;
        
        private readonly Dictionary<Type, Dictionary<IPoolable, IPoolContainer>> _poolContainers = new();
        private readonly Dictionary<IPoolable, IPoolable> _prototypes = new();
        
        public PoolableObjectProvider(PoolContainersHolder containersHolder, IPoolExpansionStrategy defaultPoolExpansionStrategy)
        {
            _containersHolder = containersHolder;
            _defaultPoolExpansionStrategy = defaultPoolExpansionStrategy;
        }

        public void RegisterPoolable<T>(T prefab, int startCount, IPoolExpansionStrategy poolExpansionStrategy = null)
                where T : MonoBehaviour, IPoolable
        {
            if (!_poolContainers.TryGetValue(typeof(T), out var containers))
            {
                containers = new Dictionary<IPoolable, IPoolContainer>();
                _poolContainers.Add(typeof(T), containers);
            }
            
            poolExpansionStrategy ??= _defaultPoolExpansionStrategy;

            var poolContainer = new PoolContainer<T>(prefab, _containersHolder.transform, poolExpansionStrategy, startCount);
            containers.Add(prefab, poolContainer);
        }

        public T GetFromPool<T>(T prefab) where T : MonoBehaviour, IPoolable
        {
            if (_poolContainers.TryGetValue(typeof(T), out var containers))
            {
                if (containers.TryGetValue(prefab, out IPoolContainer container))
                {
                    if (container is IPoolContainer<T> genericContainer)
                    {
                        return genericContainer.GetObject();
                    }

                    Debug.LogError($"Pool container for {typeof(T)} is not a generic pool container");
                    return null;
                }
            }
            else
            {
                containers = new Dictionary<IPoolable, IPoolContainer>();
                _poolContainers.Add(typeof(T), containers);
            }

            var poolContainer = new PoolContainer<T>(prefab, _containersHolder.transform, _defaultPoolExpansionStrategy);
            containers.Add(prefab, poolContainer);

            T res = poolContainer.GetObject();
            _prototypes.Add(res, prefab);
            return res;
        }
        
        public void ReturnAllInstancesToPool<T>(T poolable) where T : MonoBehaviour, IPoolable
        {
            if (!TryGetContainer(poolable, out IPoolContainer container))
            {
                Debug.LogError($"Pool container for {poolable} not found");
                return;
            }
 
            container.ReturnAll();
        }

        public void ReturnToPool<T>(T poolable) where T : MonoBehaviour, IPoolable
        {
            if (!TryGetContainer(poolable, out IPoolContainer container))
            {
                Debug.LogError($"Pool container for {poolable} not found");
                return;
            }

            if (container is not IPoolContainer<T> genericContainer)
            {
                Debug.LogError($"Pool container for {typeof(T)} is not a generic pool container");
                return;
            }
                
            genericContainer.ReturnObject(poolable);
        }
        
        private bool TryGetContainer<T>(T poolable, out IPoolContainer container) where T : MonoBehaviour, IPoolable
        {
            container = null;
            if (!_poolContainers.TryGetValue(typeof(T), out var containers))
            {
                Debug.LogError($"Pool container for {typeof(T)} not found");
                return false;
            }

            if (!_prototypes.TryGetValue(poolable, out IPoolable prefab))
            {
                Debug.LogError($"Prefab for poolable {poolable} not found");
                return false;
            }

            return containers.TryGetValue(prefab, out container);
        }

        void IDisposable.Dispose()
        {
            foreach (var containers in _poolContainers)
            {
                foreach (var container in containers.Value)
                {
                    container.Value.Dispose();
                }
            }
        }
    }
}