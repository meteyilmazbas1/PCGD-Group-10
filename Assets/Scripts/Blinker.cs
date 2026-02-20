using System.Collections;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Coroutine _blinkRoutine;
    private void OnEnable()
    {
        if (_blinkRoutine != null)
        {
            StopCoroutine(_blinkRoutine);
            _blinkRoutine = null;
        }
        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = Color.white;
        }

    }
    public void SetSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
    }
    public void BlinkDamage()
    {
        if (_blinkRoutine != null) return;
        _blinkRoutine = StartCoroutine(BlinkerDamageCoroutine());
    }
    public void BlinkDeath()
    {
        if (_blinkRoutine != null)
        {
            StopCoroutine(_blinkRoutine);
        }
        _blinkRoutine = StartCoroutine(BlinkerDeathCoroutine());
    }
    private  IEnumerator BlinkerDamageCoroutine()
    {
        for(int i = 0; i < 100; i++)
        {
            float r = Random.Range(0f,1f);
            float g = Random.Range(0f,1f);
            float b = Random.Range(0f,1f);
            Color color = new Color(r, g, b);
            _spriteRenderer.color = color;
            yield return null;
        }
        _spriteRenderer.color = Color.white;
        _blinkRoutine = null;
    }
    private IEnumerator BlinkerDeathCoroutine()
    {
        Color color1 = Color.white;
        Color color2 = Color.white;
        color2.a = 0.2f;
        for (int i = 0; i < 10; i++)
        { 
            yield return new WaitForSecondsRealtime(0.3f);
            
            _spriteRenderer.color = i%2==0? color1: color2;
        }
        color2.a = 0f;
        _spriteRenderer.color = color2;
    }
}
