using System;
using Core.Pools;
using UnityEngine;

namespace Core.Vfx
{
    public class VfxReturnToPoolCallback : MonoBehaviour
    {
        private void OnParticleSystemStopped()
        {
            CommonGameObjectPool.Instance.Despawn(gameObject);
        }
    }
}
