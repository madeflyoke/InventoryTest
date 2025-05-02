using UnityEngine;

namespace Core.Pools
{
    public class PoolableGameObject : MonoBehaviour
    {
        public GameObject Id { get; private set; }

        public void SetId(GameObject id)
        {
            Id = id;
        }
    }
}
