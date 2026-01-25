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
        [SerializeField] private List<GameObject> _enemyPrefabs;
        [SerializeField] private int _pooledEnemiesCount = 50;
        private List<GameObject> _enemyPool;

        /// <summary>
        /// Instantiate enemies to the enemy pool.
        /// </summary>
        public void InitializeEnemyPool()
        {
            _enemyPool = new List<GameObject>();
            while (_enemyPool.Count < _pooledEnemiesCount)
            {
                foreach (GameObject prefab in _enemyPrefabs)
                {
                    GameObject enemy = Instantiate(prefab, transform.position, Quaternion.identity);
                    enemy.SetActive(false);
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
            GameObject enemy = _enemyPool.Find(x => !x.activeInHierarchy);
            enemy.SetActive(true);
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
                yield return new WaitForSecondsRealtime(3);
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
            return new Vector2(playerPosition.x + sign * 20f, 0f);
        }
    }
}
