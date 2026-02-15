using UnityEngine;

namespace UrbanNinja
{
    /// <summary>
    /// Screen shake effect that works with Cinemachine.
    /// DefaultExecutionOrder(9999) ensures this runs AFTER
    /// Cinemachine has finished positioning the camera in LateUpdate.
    /// Attach this to the Main Camera.
    /// </summary>
    [DefaultExecutionOrder(9999)]
    public class ScreenShake : MonoBehaviour
    {
        #region Singleton
        private static ScreenShake _instance;
        public static ScreenShake Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ScreenShake>();
                }
                return _instance;
            }
        }
        #endregion

        private float _shakeDuration;
        private float _shakeTimeRemaining;
        private float _initialMagnitude;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(this);
                return;
            }
        }

        /// <summary>
        /// Runs AFTER Cinemachine (execution order 9999).
        /// Adds shake offset on top of Cinemachine's position.
        /// </summary>
        private void LateUpdate()
        {
            if (_shakeTimeRemaining > 0f)
            {
                float progress = _shakeTimeRemaining / _shakeDuration;
                float currentMagnitude = _initialMagnitude * progress;

                float offsetX = Random.Range(-1f, 1f) * currentMagnitude;
                float offsetY = Random.Range(-1f, 1f) * currentMagnitude;

                transform.position += new Vector3(offsetX, offsetY, 0f);

                _shakeTimeRemaining -= Time.deltaTime;
            }
        }

        /// <summary>
        /// Trigger a screen shake with custom duration and magnitude.
        /// </summary>
        public void Shake(float duration, float magnitude)
        {
            // Only override if new shake is stronger
            if (_shakeTimeRemaining > 0f && magnitude < _initialMagnitude) return;
            
            _shakeDuration = duration;
            _initialMagnitude = magnitude;
            _shakeTimeRemaining = duration;
        }

        /// <summary>
        /// Light shake - for player hitting enemies.
        /// </summary>
        public void ShakeLight()
        {
            Shake(0.08f, 0.06f);
        }

        /// <summary>
        /// Medium shake - for player taking damage.
        /// </summary>
        public void ShakeMedium()
        {
            Shake(0.15f, 0.12f);
        }

        /// <summary>
        /// Heavy shake - for high combos or big hits.
        /// </summary>
        public void ShakeHeavy()
        {
            Shake(0.2f, 0.2f);
        }
    }
}
