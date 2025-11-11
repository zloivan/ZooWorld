using System.Collections.Generic;
using _Project.Scripts.Core;
using DefaultNamespace.Configs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class AnimalSpawner : MonoBehaviour
    {
        [SerializeField] private List<AnimalConfigSO> _animalConfigs;
        [SerializeField] private float _spawnInterval = 2f;
        [SerializeField] private Vector2 _spawnAreaSize = new(10f, 10f);
        [SerializeField] private LayerMask _obstacleLayer;

        private float _spawnTimer;
        private float _nextSpawnTime;
        private Vector3 _lastCheckedPosition;
        private bool _isPositionValid;
        private const float CHECK_RADIUS = 1f;

        private void Awake() =>
            RestartTimer();

        private void Update()
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer < _nextSpawnTime)
                return;

            SpawnRandomAnimal();
            RestartTimer();
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

                _isPositionValid = !Physics.CheckSphere(spawnPosition, CHECK_RADIUS, _obstacleLayer);

                if (_isPositionValid)
                    break;
            }

            var animalGo = AnimalFactory.CreateAnimal(configSO, spawnPosition);
            animalGo.transform.SetParent(transform);
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
            Gizmos.DrawWireSphere(_lastCheckedPosition, CHECK_RADIUS);
        }
    }
}