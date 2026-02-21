using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace UrbanNinja
{
    public class EnemyDirectionSign : MonoBehaviour
    {
        [SerializeField] Sprite _sign;
        private SpriteRenderer _renderer;
        private Blinker _blinker;
        private float _widthHalf;
        private void Awake()
        {
            _widthHalf = (1 / Camera.main.aspect * Camera.main.orthographicSize / 2);
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.sprite = _sign;
            _renderer.enabled = false;
            _blinker = GetComponent<Blinker>();
            _blinker.SetSpriteRenderer(_renderer);
        }
        private void OnEnable()
        {
            EnemySpawner.OnEnemySpawn += OnEnemySpawn;
        }
        private void OnDisable()
        {
            EnemySpawner.OnEnemySpawn -= OnEnemySpawn;
        }
        private void OnEnemySpawn(Vector3 position)
        {
            _renderer.enabled = true;
            _blinker.BlinkDeath();
            bool outaSight = position.x > Camera.main.transform.position.x + _widthHalf 
                || position.x < Camera.main.transform.position.x - _widthHalf;
            if (outaSight)
            {
                int sign = (position-transform.position).x < 0 ? -1 : 1;
                transform.localScale = new Vector3(sign, 1f, 1f);
            }
        }
    }
}
