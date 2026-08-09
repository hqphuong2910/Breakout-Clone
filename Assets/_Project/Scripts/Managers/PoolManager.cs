using System;
using System.Collections.Generic;
using _Project.Scripts.Patterns;
using _Project.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace _Project.Scripts.Managers
{
    public class PoolManager : Singleton<PoolManager>
    {
        [Header("Pre-warm Pools")] [SerializeField]
        private List<PoolConfig> initialPools;

        private readonly Dictionary<int, ObjectPool<GameObject>> _pools = new();
        private readonly Dictionary<GameObject, ObjectPool<GameObject>> _spawnedObjects = new();

        /// <summary>
        ///     Create new pool if not exists.
        /// </summary>
        public void CreatePool(GameObject prefab, int defaultCapacity = 20, int maxSize = 100)
        {
            var poolKey = prefab.GetInstanceID();
            if (_pools.ContainsKey(poolKey)) return;
            var pool = new ObjectPool<GameObject>(
                () => Instantiate(prefab, transform),
                obj => obj.SetActive(true),
                obj => obj.SetActive(false),
                Destroy,
                true,
                defaultCapacity,
                maxSize
            );

            _pools.Add(poolKey, pool);
            AppLogger.Log(name, $"Created new pool for {prefab.name}");
        }

        /// <summary>
        ///     Get an object from pool.
        /// </summary>
        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var poolKey = prefab.GetInstanceID();

            if (!_pools.ContainsKey(poolKey)) CreatePool(prefab);

            var pool = _pools[poolKey];
            var instance = pool.Get();
            instance.transform.SetPositionAndRotation(position, rotation);

            _spawnedObjects[instance] = pool;

            return instance;
        }

        /// <summary>
        ///     Return an object to its pool.
        /// </summary>
        public void Despawn(GameObject instance)
        {
            if (_spawnedObjects.TryGetValue(instance, out var pool))
            {
                pool.Release(instance);
                _spawnedObjects.Remove(instance);
            }
            else
            {
                AppLogger.LogWarning(name, $"This object doesn't belong to {name}. Destroying...");
                Destroy(instance);
            }
        }

        [Serializable]
        public class PoolConfig
        {
            public GameObject prefab;
            public int defaultCapacity = 20;
            public int maxSize = 100;
        }
    }
}