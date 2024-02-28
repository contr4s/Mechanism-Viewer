using UnityEngine;

namespace ObjectPool.PoolExpansionStrategies
{
    public class AggressivePoolExpansion : IPoolExpansionStrategy
    {
        private readonly int _poolSizeDivisor;
        
        public AggressivePoolExpansion(int poolSizeDivisor = 1)
        {
            _poolSizeDivisor = poolSizeDivisor;
        }

        public virtual int CalculateCountOfObjectsToCreate(int currentPoolSize) =>
                Mathf.Max(1, currentPoolSize / _poolSizeDivisor);
    }
}