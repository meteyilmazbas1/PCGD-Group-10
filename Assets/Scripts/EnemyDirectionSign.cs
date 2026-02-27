
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
            _widthHalf = ( Camera.main.aspect * Camera.main.orthographicSize);
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
            bool outaSight = position.x > Camera.main.transform.position.x + _widthHalf 
                || position.x < Camera.main.transform.position.x - _widthHalf;
            if (outaSight)
            {
                Vector3 pos = position - transform.position;
                transform.position = pos.x<0?new Vector3(Camera.main.transform.position.x - _widthHalf + 1f, 
                    transform.position.y, 0f): new Vector3(Camera.main.transform.position.x + _widthHalf - 1f,
                    transform.position.y, 0f);
                int sign = (position-transform.position).x < 0 ? -1 : 1;
                transform.localScale = new Vector3(sign, 1f, 1f);
                _renderer.enabled = true;
                _blinker.SetBlinkEndCallback(() => _renderer.enabled = false);
                _blinker.BlinkDeath();
            }
        }
    }
}
