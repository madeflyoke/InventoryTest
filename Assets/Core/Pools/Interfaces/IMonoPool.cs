using UnityEngine;

namespace Core.Pools.Interfaces
{
    public interface IMonoPool
    {
        public T Spawn<T>(Transform parent, bool worldPositionStays = false) where T : MonoBehaviour;
        public void Despawn<T>(T item) where T : MonoBehaviour; 
    }
}
