using UnityEngine;

namespace ObjectPool
{
    public class PoolContainersHolder : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}