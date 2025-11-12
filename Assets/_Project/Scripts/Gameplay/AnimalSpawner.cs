using System.Collections.Generic;
using _Project.Scripts.Core;
using _Project.Scripts.Core.Signals;
using DefaultNamespace.Configs;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class AnimalSpawner : MonoBehaviour
    {
        [Header("Can spawn debug 20 and 100 animals via context menu")]
        
        [SerializeField] private List<AnimalConfigSO> _animalConfigs;
        [SerializeField] private float _spawnInterval = 2f;
        [SerializeField] private Vector2 _spawnAreaSize = new(10f, 10f);
        [SerializeField] private LayerMask _obstacleLayer;
        [SerializeField] private int _defaultPoolSize = 10;
        [SerializeField] private int _maxPoolSize = 50;
        [SerializeField] private AnimalSignals _animalSignals;
        
        private float _spawnTimer;
        private float _nextSpawnTime;
        private Vector3 _lastCheckedPosition;
        private bool _isPositionValid;
        private const float SPAWN_CHECK_RADIUS = 1f;

        private Dictionary<AnimalConfigSO, ObjectPool<Animal>> _animalPools;
        
        private void Awake()
        {
            InitializePools();
            RestartTimer();
        }

        private void Update()
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer < _nextSpawnTime)
                return;

            SpawnRandomAnimal();
            RestartTimer();
        }

        private void InitializePools()
        {
            _animalPools = new Dictionary<AnimalConfigSO, ObjectPool<Animal>>();

            foreach (var cfg in _animalConfigs)
            {
                var pool = new ObjectPool<Animal>(
                    createFunc: () => CreateAnimalForPool(cfg),
                    actionOnGet: OnGetFromPool,
                    actionOnRelease: OnReleaseFromPool,
                    actionOnDestroy: animal => Destroy(animal.gameObject),
                    collectionCheck: true,
                    defaultCapacity: _defaultPoolSize,
                    maxSize: _maxPoolSize
                );
                
                _animalPools[cfg] = pool;
            }
        }

        private Animal CreateAnimalForPool(AnimalConfigSO cfg)
        {
            var animal = AnimalFactory.CreateAnimal(cfg, Vector3.zero, _animalSignals);
            animal.transform.SetParent(transform);
            return animal;
        }

        private void OnGetFromPool(Animal animal) =>
            animal.gameObject.SetActive(true);

        private void OnReleaseFromPool(Animal animal)
        {
            animal.gameObject.SetActive(false);
        }

        private void RestartTimer()
        {
            //TODO: Can implement some randomization later
            _spawnTimer = 0;

            _nextSpawnTime = _spawnInterval;
        }

        [ContextMenu("SPAWN RANDOM ANIMAL")]
        private void SpawnRandomAnimal()
        {
            var configSO = _animalConfigs[Random.Range(0, _animalConfigs.Count)];

            Vector3 spawnPosition;

            while (true)
            {
                spawnPosition = new Vector3(
                    Random.Range(-_spawnAreaSize.x / 2, _spawnAreaSize.x / 2),
                    0,
                    Random.Range(-_spawnAreaSize.y / 2, _spawnAreaSize.y / 2)
                );

                _lastCheckedPosition = spawnPosition;

                _isPositionValid = !Physics.CheckSphere(spawnPosition, SPAWN_CHECK_RADIUS, _obstacleLayer);

                if (_isPositionValid)
                    break;
            }

            if (_animalPools.TryGetValue(configSO, out var pool))
            {
                var animal = pool.Get();
                animal.transform.position = spawnPosition;
                animal.SetSourcePool(pool);
            }
            else
            {
                Debug.LogError($"No pull found for animal config: {configSO.name}");
            }
        }


        //--------------------------------DEBUG
        [ContextMenu("SPAWN 20 RANDOM ANIMAL")]
        private void Spawn20Animals()
        {
            for (int i = 0; i < 20; i++)
            {
                SpawnRandomAnimal();
            }
        }

        [ContextMenu("SPAWN 100 RANDOM ANIMAL")]
        private void Spawn100Animals()
        {
            for (int i = 0; i < 100; i++)
            {
                SpawnRandomAnimal();
            }
        }

        private void OnDrawGizmos()
        {
            // Spawn area
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(_spawnAreaSize.x, 0, _spawnAreaSize.y));

            // Spawn position check
            Gizmos.color = _isPositionValid ? Color.green : Color.red;
            Gizmos.DrawWireSphere(_lastCheckedPosition, SPAWN_CHECK_RADIUS);
        }
    }
}