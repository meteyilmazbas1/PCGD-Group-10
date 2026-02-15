using UnityEngine;

namespace UrbanNinja
{
    public class LevelBounds : MonoBehaviour
    {
        public static LevelBounds Instance;
        private BoxCollider2D _collider;
        private float _left;
        private float _right;
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            _collider = GetComponent<BoxCollider2D>();
            Bounds b = _collider.bounds;
            _left = b.center.x - b.extents.x;
            _right = b.center.x + b.extents.x;
        }
        /// <summary>
        /// Check if a given vector 3 position
        /// is within the level bounds.
        /// </summary>
        /// <param name="position">position to check.</param>
        /// <returns>True if it is in, otherwise false.</returns>
        public bool IsInsideLevelBounds(Vector3 position)
        {
            if(position.x < _left)
            {
                return false;
            }
            else if (position.x > _right)
            {
                return false;
            }
            return true;
        }
    }

}