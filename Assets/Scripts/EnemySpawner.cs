using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrbanNinja
{
    /// <summary>
    /// This class will handle enemy object pooling and
    /// continuous enemy spawning.
    /// </summary>
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

        /// <summary>
        /// Instantiate enemies to the enemy pool.
        /// </summary>
        public void InitializeEnemyPool()
        {
            _enemyPool = new List<EnemyController>();
            while (_enemyPool.Count < _pooledEnemiesCount)
            {
                foreach (EnemyController prefab in _enemyPrefabs)
                {
                    EnemyController enemy = Instantiate(prefab, transform.position, Quaternion.identity);
                    enemy.gameObject.SetActive(false);
                    _enemyPool.Add(enemy);
                    if (_enemyPool.Count >= _pooledEnemiesCount)
                    {
                        break;
                    }
                }
            }

        }
        /// <summary>
        /// Select the next inactive enemy from the pool and enable it.
        /// </summary>
        /// <param name="position"></param>
        private void SpawnEnemy(Vector2 position)
        {
            EnemyController enemy = _enemyPool.Find(x => !x.gameObject.activeInHierarchy);
            EnemyTier tier = enemy.GetTier();
            if(_tier == null)
            {
                _tier = tier;
            }
            else if (_tier.TierLevel < tier.TierLevel)
            {
                _tier = tier;
            }
            enemy.gameObject.SetActive(true);
            enemy.transform.position = position;
        }
        private void Start()
        {
            InitializeEnemyPool();
            StartCoroutine(SpawnEnemies());
        }
        /// <summary>
        /// Coroutine to continously spawn enemies.
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpawnEnemies()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(GetTimeBetweenSpawns());
                SpawnEnemy(OffsetPosition());
            }
        }
        /// <summary>
        /// Calculate an offset vector to spawn enemies off screen
        /// and randomly from left and right.
        /// </summary>
        /// <returns></returns>
        private Vector2 OffsetPosition()
        {

            Vector2 playerPosition = GameManager.GetPlayerController().transform.position;
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
