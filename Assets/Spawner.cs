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

    private ObjectPool<Cube> _objectPool;
    private Cube _cube;
    private Renderer _color;
    private float _startTimeOfInstantiation = 0;
    private float _repeatRate = 1f;
   
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
        InvokeRepeating(nameof(SpawnCube), _startTimeOfInstantiation, _repeatRate);
    }

    private void PrepareObjectToInstantiation(Cube obj)
    {
        obj.transform.position = _prefabObject.GetCoordinateOfAppearance();
        obj.GetComponent<Renderer>().material.color= Color.red;
        obj.gameObject.SetActive(true);
        obj.LifeIsFinished += PutToPool;
    }

    private void SpawnCube()
    {
        _objectPool.Get();
    }

    private void DeactivateObject(Cube obj)
    {
        obj.LifeIsFinished -= PutToPool;
        obj.gameObject.SetActive(false);
    }

    private void PutToPool(Cube obj)
    {
        _objectPool.Release(obj);
    }
}