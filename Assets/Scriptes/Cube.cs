using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(Renderer))]

public class Cube : MonoBehaviour
{
    private Renderer _color;
    private float _minOffsetOfPosition = -8f;
    private float _maxOffsetOfPosition = 8f;
    private float _maxTopCoordinateOfPosition = 15f;
    private float _minTimeOfLife = 2f;
    private float _maxTimeOfLife = 5f;
    private bool _isChanged;

    public event Action<Cube> LifeIsFinished;

    public float MaxTopCoordinateOfPosition => _maxTopCoordinateOfPosition;

    public void Awake()
    {
        _color = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Platform platform))
        {
            if (_isChanged == false)
            {
                ChangeSelfColor();
                _isChanged = true;
            }

            StartCoroutine(Live());
        }
    }

    private IEnumerator Live()
    {
        var wait = new WaitForSeconds(GetRandomTimeOfLife());

        yield return wait;

        LifeIsFinished?.Invoke(this);
    }

    private void ChangeSelfColor()
    {
        _color.material.color = new Color(GetConponentOfColor(), GetConponentOfColor(), GetConponentOfColor());
        _color.material.color = Color.green;
    }

    private float GetRandomTimeOfLife()
    {
        return UnityEngine.Random.Range(_minTimeOfLife, _maxTimeOfLife);
    }

    private float GetConponentOfColor()
    {
        return UnityEngine.Random.Range(0, 1f);
    }

    public void SetSelfStartColor()
    {
        _color.material.color = Color.red;
        _isChanged = false;
    }

    public float GetRandomOffset()
    {
        return UnityEngine.Random.Range(_minOffsetOfPosition, _maxOffsetOfPosition);
    }
}