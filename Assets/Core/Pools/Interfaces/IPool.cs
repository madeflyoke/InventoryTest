using UnityEngine;

namespace Core.Pools
{
    public interface IPool<T> where T: MonoBehaviour
    {
        public T Spawn(Transform parent, bool worldPositionStays = false);
        public void Despawn(T item);
    }
}
