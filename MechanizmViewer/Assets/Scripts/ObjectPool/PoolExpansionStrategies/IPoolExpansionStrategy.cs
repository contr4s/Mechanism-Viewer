namespace ObjectPool.PoolExpansionStrategies
{
    public interface IPoolExpansionStrategy
    {
        int CalculateCountOfObjectsToCreate(int currentPoolSize);
    }
}