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
        Vector2 translateVector = -direction * _levelOneparallaxSpeed * Time.deltaTime;
        _sumVector -= translateVector;

        ParallaxMovement(translateVector);

        if (_sumVector.x > unitWidth || _sumVector.x < -unitWidth)
        {
            int shift = (int)(_sumVector.x * pixelPerUnit);
            UpdateParallax(shift);
            _sumVector = Vector2.zero;
        }
    }

    private void ParallaxMovement(Vector2 translateVector)
    {
        foreach (SpriteRenderer sprite in _levelOneParallax)
        {
            sprite.transform.Translate(translateVector);
        }
    }

    private void UpdateParallax( int shiftDirection)
    {
        bool isNegativeShift = shiftDirection < 0;
        int index = isNegativeShift ? 2 : 0;
        SpriteRenderer sprite = _levelOneParallax[index];
        Vector3 directionVector = isNegativeShift ? Vector2.left : Vector2.right;
        sprite.transform.position += directionVector * unitWidth * 3;
        int parallaxIndexToRemove = isNegativeShift ? 2 : 0;
        _levelOneParallax.RemoveAt(parallaxIndexToRemove);
        if (isNegativeShift) 
        {
            _levelOneParallax.Insert(0, sprite);
        }
        else
        {
            _levelOneParallax.Add(sprite);
        }
    }
}
