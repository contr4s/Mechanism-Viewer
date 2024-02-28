using System.Collections.Generic;
using System.Linq;
using ObjectPool.PoolExpansionStrategies;
using UnityEngine;

namespace ObjectPool
{
    public class PoolContainer<T> : IPoolContainer<T> where T : MonoBehaviour, IPoolable
    {
        private readonly T _prefab;
        private readonly HashSet<T> _activeObjects = new HashSet<T>();
        private readonly HashSet<T> _passiveObjects = new HashSet<T>();
        private readonly GameObject _container;
        private readonly IPoolExpansionStrategy _poolExpansionStrategy;

        public PoolContainer(T prefab,
                             Transform parent,
                             IPoolExpansionStrategy poolExpansionStrategy,
                             int initialCount = 1)
        {
            _prefab = prefab;
            _container = new GameObject($"{prefab.name} Container");
            _container.transform.SetParent(parent, false);
            _poolExpansionStrategy = poolExpansionStrategy;
            AddObjects(initialCount);
        }

        public T GetObject()
        {
            if (!_passiveObjects.Any())
            {
                AddObjects(_poolExpansionStrategy.CalculateCountOfObjectsToCreate(_activeObjects.Count));
            }

            T passiveObject = _passiveObjects.First();
            _passiveObjects.Remove(passiveObject);
            _activeObjects.Add(passiveObject);
            passiveObject.gameObject.SetActive(true);

            return passiveObject;
        }

        public void ReturnAll()
        {
            while (_activeObjects.Any())
            {
                ReturnObject(_activeObjects.First());
            }
        }

        public void ReturnObject(T obj)
        {
            if (!_activeObjects.Contains(obj))
            {
                Debug.LogWarning($"Object {obj.name} is not in active objects");
                obj.gameObject.SetActive(false);

                return;
            }

            _activeObjects.Remove(obj);
            _passiveObjects.Add(obj);
            
            obj.Reset();
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_container.transform, false);
        }

        private void AddObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T newObject = Object.Instantiate(_prefab, _container.transform);
                newObject.gameObject.SetActive(false);
                _passiveObjects.Add(newObject);
            }
        }

        public void Dispose()
        {
            ReturnAll();
            _passiveObjects.Clear();
            Object.Destroy(_container);
        }
    }
}
