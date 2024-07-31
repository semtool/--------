using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Spawner _spawner;
    private Renderer _color;
    private float _minTimeOfLife = 2f;
    private float _maxTimeOfLife = 5f;
    private float _minCoordinateOfPosition = -5f;
    private float _maxCoordinateOfPosition = -10f;
    private float _maxTopCoordinateOfPosition = 15f;

    public void Awake()
    {
        _color = gameObject.GetComponent<Renderer>();
        _spawner = GameObject.FindGameObjectWithTag("Spawn").GetComponent<Spawner>();
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
        return Random.Range(_minCoordinateOfPosition, _maxCoordinateOfPosition);
    }

    private IEnumerator Live()
    {
        var wait = new WaitForSeconds(GetRandomTimeOfLife());

        yield return wait;

        _spawner.ObjectPool.Release(gameObject);
    }

    private float GetRandomTimeOfLife()
    {
        return Random.Range(_minTimeOfLife, _maxTimeOfLife);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ChangeSelfColor();

        StartCoroutine(Live());
    }
}