using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(ObjectPool<>))]
[RequireComponent(typeof(Cube))]
[RequireComponent(typeof(Renderer))]

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefabObject;
    [SerializeField] private int _capacity;
    [SerializeField] private int _maxSize;
    [SerializeField] private MainPlatform _mainPlatform;

    private ObjectPool<Cube> _objectPool;
    private float _repeatRate = 1f;
    private bool _isSpawn = true;

    private void Awake()
    {
        _objectPool = new ObjectPool<Cube>(
        createFunc: () => Instantiate(_prefabObject),
        actionOnGet: (obj) => PrepareObjectToInstantiation(obj),
        actionOnRelease: (obj) => DeactivateObject(obj),
        actionOnDestroy: (obj) => Destroy(obj),
        collectionCheck: true,
        defaultCapacity: _capacity,
        maxSize: _maxSize);
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (_isSpawn)
        {
            SpawnCube();

            var wait = new WaitForSeconds(_repeatRate);

            yield return wait;
        }
    }

    private void PrepareObjectToInstantiation(Cube cube)
    {
        SetCoordinateOfAppearance(cube);
        cube.SetSelfStartColor();
        cube.gameObject.SetActive(true);
        cube.LifeIsFinished += PutToPool;
    }

    private void SetCoordinateOfAppearance(Cube cube)
    {
        cube.transform.position = new Vector3(_mainPlatform.transform.position.x + 
            cube.GetRandomOffset(), _mainPlatform.transform.position.y +
            cube.MaxTopCoordinateOfPosition, _mainPlatform.transform.position.z + 
            cube.GetRandomOffset());
    }

    private void SpawnCube()
    {
        _objectPool.Get();
    }

    private void DeactivateObject(Cube cube)
    {
        cube.LifeIsFinished -= PutToPool;
        cube.gameObject.SetActive(false);
    }

    private void PutToPool(Cube obj)
    {
        _objectPool.Release(obj);
    }
}