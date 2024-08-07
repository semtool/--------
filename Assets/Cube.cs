using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(Renderer))]

public class Cube : MonoBehaviour
{
    private static float _minCoordinateOfPosition = -5f;
    private static float _maxCoordinateOfPosition = 5f;
    private static float _maxTopCoordinateOfPosition = 15;

    private Renderer _color;
    private float _minTimeOfLife = 2f;
    private float _maxTimeOfLife = 5f;

    public event Action<Cube> LifeIsFinished;

    public void Awake()
    {
        _color = gameObject.GetComponent<Renderer>();
    }

    public Vector3 GetCoordinateOfAppearance()
    {
        return new Vector3(GetRandomPosition(), _maxTopCoordinateOfPosition, GetRandomPosition());
    }

    private void ChangeSelfColor()
    {
        _color.material.color = Color.black;
    }

    private float GetRandomPosition()
    {
        return UnityEngine.Random.Range(_minCoordinateOfPosition, _maxCoordinateOfPosition);
    }

    private IEnumerator Live()
    {
        var wait = new WaitForSeconds(GetRandomTimeOfLife());

        yield return wait;

        LifeIsFinished?.Invoke(this);
    }

    private float GetRandomTimeOfLife()
    {
        return UnityEngine.Random.Range(_minTimeOfLife, _maxTimeOfLife);
    }

    private void OnCollisionEnter(Collision collision)
    {
       var cube = collision.gameObject.GetComponent<Cube>();

        if (cube == null)
        {
            ChangeSelfColor();

            StartCoroutine(Live());
        }
    }
}