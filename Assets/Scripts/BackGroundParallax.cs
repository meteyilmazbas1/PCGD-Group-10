using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UrbanNinja;

public class BackGroundParallax : MonoBehaviour
{
    [Header("Place 3 SpriteRenderers of same pixel size (left, center, right)")]
    [SerializeField] private List<SpriteRenderer> _levelOneParallax;
    [Header("The pixel width of parallax sprites.")]
    [SerializeField] private int _pixelWidth;
    [Header("Scrollling speed for parallax 1")]
    [SerializeField] private float _levelOneparallaxSpeed;

    private Vector2 _lastPos;
    private Vector2 _sumVector;
    private int pixelPerUnit;
    private int unitWidth;
    private void Awake()
    {
        pixelPerUnit = (int)_levelOneParallax[0].sprite.pixelsPerUnit;
        unitWidth = _pixelWidth / pixelPerUnit;
    }
    private void FixedUpdate()
    {
        Vector2 campos = (Vector2)Camera.main.transform.position;
        Vector2 direction = campos - _lastPos;
        _lastPos = campos;
        _sumVector += direction;
        direction.y = 0f;
        direction = direction.normalized;
        

        foreach (SpriteRenderer sprite in _levelOneParallax)
        {
            sprite.transform.Translate(direction * _levelOneparallaxSpeed * Time.deltaTime);
        }
        
        if (_sumVector.x > unitWidth || _sumVector.x < -unitWidth)
        {
            int shift = (int)(_sumVector.x * pixelPerUnit);
            if (shift < 0)
            {

                SpriteRenderer sprite = _levelOneParallax[2];
                sprite.transform.position += Vector3.left * unitWidth * 3;
                _levelOneParallax.RemoveAt(2);
                _levelOneParallax.Insert(0, sprite);
            }
            else
            {
                SpriteRenderer sprite = _levelOneParallax[0];
                sprite.transform.position += Vector3.right * unitWidth * 3;
                _levelOneParallax.RemoveAt(0);
                _levelOneParallax.Add(sprite);
            }
            _sumVector = Vector2.zero;
        }
    }
}
