using UnityEngine;

namespace ObjectPool.PoolExpansionStrategies
{
    public class LimitedPoolExpansion : AggressivePoolExpansion
    {
        private readonly int _maxCountOfObjectsToCreate;
        
        public LimitedPoolExpansion(int maxCountOfObjectsToCreate, int poolSizeDivisor = 1) : base(poolSizeDivisor)
        {
            _maxCountOfObjectsToCreate = maxCountOfObjectsToCreate;
        }

        public override int CalculateCountOfObjectsToCreate(int currentPoolSize)
        {
            return Mathf.Min(base.CalculateCountOfObjectsToCreate(currentPoolSize), _maxCountOfObjectsToCreate);
        }
    }
}