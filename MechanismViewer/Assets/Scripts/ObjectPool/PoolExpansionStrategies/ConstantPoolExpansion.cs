namespace ObjectPool.PoolExpansionStrategies
{
    public class ConstantPoolExpansion : IPoolExpansionStrategy
    {
        private readonly int _сountOfObjectsToCreate;
        
        public ConstantPoolExpansion(int сountOfObjectsToCreate)
        {
            _сountOfObjectsToCreate = сountOfObjectsToCreate;
        }

        public int CalculateCountOfObjectsToCreate(int _) => _сountOfObjectsToCreate;
    }
}