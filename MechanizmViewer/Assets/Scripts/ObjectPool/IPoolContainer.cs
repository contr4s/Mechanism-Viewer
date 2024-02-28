using System;

namespace ObjectPool
{
    public interface IPoolContainer : IDisposable
    {
        void ReturnAll();
    }

    public interface IPoolContainer<T> : IPoolContainer
    {
        T GetObject();
        void ReturnObject(T obj);
    }
}