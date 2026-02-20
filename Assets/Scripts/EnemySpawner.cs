using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrbanNinja
{
    
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<EnemyController> _enemyPrefabs;
        [SerializeField] private int _pooledEnemiesCount = 50;
        [SerializeField] private float _maxSpawDelay = 3f;
        [SerializeField] private float _spawnTimeDecrement = 0.25f;

        private float _minSpawnRate = 0.5f;
        private List<EnemyController> _enemyPool;
        private int _spawnedCount;
        private EnemyTier _tier;

        
        public void InitializeEnemyPool()
        {
            if (_enemyPrefabs == null || _enemyPrefabs.Count == 0)
            {
                //Debug.LogError("EnemySpawner: _enemyPrefabs list is empty! Please assign enemy prefabs in Inspector.");
                return;
            }

            bool hasValidPrefab = false;
            foreach (EnemyController prefab in _enemyPrefabs)
            {
                if (prefab != null)
                {
                    hasValidPrefab = true;
                    break;
                }
            }

            if (!hasValidPrefab)
            {
                //Debug.LogError("EnemySpawner: All prefabs in _enemyPrefabs list are null! Cannot initialize pool.");
                return;
            }

            _enemyPool = new List<EnemyController>();
            int maxIterations = _pooledEnemiesCount * 10;
            int iterations = 0;
            
            while (_enemyPool.Count < _pooledEnemiesCount && iterations < maxIterations)
            {
                iterations++;
                foreach (EnemyController prefab in _enemyPrefabs)
                {
                    if (prefab == null) continue;

                    EnemyController enemy = Instantiate(prefab, transform.position, Quaternion.identity);
                    enemy.gameObject.SetActive(false);
                    _enemyPool.Add(enemy);
                    if (_enemyPool.Count >= _pooledEnemiesCount) break;
                }
            }

            if (iterations >= maxIterations)
            {
                //Debug.LogError($"EnemySpawner: Reached max iterations ({maxIterations}) while initializing pool. Only created {_enemyPool.Count} enemies.");
            }
            else
            {
                //Debug.Log($"EnemySpawner: Initialized enemy pool with {_enemyPool.Count} enemies.");
            }
        }
        
        private void SpawnEnemy(Vector2 position)
        {
            if (_enemyPool == null || _enemyPool.Count == 0)
            {
                //Debug.LogWarning("EnemySpawner: Enemy pool is empty! Cannot spawn enemy.");
                return;
            }

            EnemyController enemy = _enemyPool.Find(x => !x.gameObject.activeInHierarchy);
            if (enemy == null)
            {
                //Debug.LogWarning("EnemySpawner: No inactive enemies found in pool!");
                return;
            }

            EnemyTier tier = enemy.GetTier();
            if(_tier == null)
            {
                _tier = tier;
            }
            else if (tier != null && _tier.TierLevel < tier.TierLevel)
            {
                _tier = tier;
            }
            enemy.gameObject.SetActive(true);
            enemy.transform.position = position;
            enemy.RandomizeWeaponWield();
        }
        private void Start()
        {
            InitializeEnemyPool();
            StartCoroutine(SpawnEnemies());
        }
       
        private IEnumerator SpawnEnemies()
        {
            yield return new WaitForSeconds(1f); // Initial delay for player to spawn
            //Debug.Log("EnemySpawner: SpawnEnemies coroutine started.");
            while (true)
            {
                float timeToWait = GetTimeBetweenSpawns();
                //Debug.Log($"EnemySpawner: Waiting {timeToWait} seconds before next spawn...");
                yield return new WaitForSeconds(timeToWait);
                //Debug.Log("EnemySpawner: Attempting to spawn enemy at position...");
                SpawnEnemy(OffsetPosition());
            }
        }
       
        private Vector2 OffsetPosition()
        {
            PlayerController player = GameManager.GetPlayerController();
            if (player == null)
            {
                //Debug.LogWarning("EnemySpawner: Player not found! Using default spawn position.");
                return new Vector2(10f, 0f);
            }

            Vector2 playerPosition = player.transform.position;
            int sign = Random.Range(-10, 11) < 0 ? -1 : 1;
            return new Vector2(playerPosition.x + sign * 10f, 0f);
        }
        private float GetTimeBetweenSpawns()
        {
            if (_tier == null) return _maxSpawDelay;
            float rate = _maxSpawDelay - _tier.TierLevel * _spawnTimeDecrement;
            return rate < _minSpawnRate ? _minSpawnRate : rate;
        }
    }
}
