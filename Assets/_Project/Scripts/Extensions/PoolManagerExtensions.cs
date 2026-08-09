using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.Extensions
{
    public static class PoolManagerExtensions
    {
        /// <summary>
        ///     Shortened syntax of <see cref="PoolManager.Spawn" />.
        /// </summary>
        public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return PoolManager.Instance.Spawn(prefab, position, rotation);
        }

        /// <summary>
        ///     Shortened syntax of <see cref="PoolManager.Despawn" />
        /// </summary>
        public static void Despawn(this GameObject instance)
        {
            PoolManager.Instance.Despawn(instance);
        }
    }
}