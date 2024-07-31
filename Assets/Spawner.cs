using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubeObject;
    [SerializeField] private int _capacity;
    [SerializeField] private int _maxSize;

    private ObjectPool<GameObject> _objectPool;
    private Cube _cube;
    private Renderer _color;

    public ObjectPool<GameObject> ObjectPool => _objectPool;

    private void Awake()
    {
        _cube = _cubeObject.GetComponent<Cube>();
        _objectPool = new ObjectPool<GameObject>(
        createFunc: () => Instantiate(_cubeObject),
        actionOnGet: (cubeObject) => PrepareObjectToInstantiation(cubeObject),
        actionOnRelease: (cubeObject) => DeactivateObject(cubeObject),
        actionOnDestroy: (cubeObject) => Destroy(cubeObject),
        collectionCheck: true,
        defaultCapacity: _capacity,
        maxSize: _maxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(Getcube), 0, 1);
    }

    private void PrepareObjectToInstantiation(GameObject obj)
    {
        obj.transform.position = _cube.GetCoordinateOfAppearance();
        obj.GetComponent<Renderer>().material.color= Color.red;
        obj.SetActive(true);
    }

    private void Getcube()
    {
        _objectPool.Get();
    }

    private void DeactivateObject(GameObject obj)
    {     
        obj.SetActive(false);
    }
}