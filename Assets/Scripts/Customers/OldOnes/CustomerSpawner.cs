using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CustomerBehaviour _customerPrefab;
    [SerializeField] private PathScriptList _pathList;
    private Vector3[] _path;
    private bool[] _occupied;

    private ObjectPool<CustomerBehaviour> _customerPool;

    [Header("Spawn Settings")]
    private float _spawnRate;
    [SerializeField] private float _spawnRatemin = 3f;
    [SerializeField] private float _spawnRatemax = 20f;
    [SerializeField] private IntEvent _freeTable;

    private void Awake()
    {
        _customerPool = new ObjectPool<CustomerBehaviour>(_customerPrefab, 10, transform);
    }

    private void Start()
    {
        _occupied = new bool[_pathList.pathScripts.Length];
    }

    private void Update()
    {
        _spawnRate -= Time.deltaTime;
        if (_spawnRate <= 0)
        {
            SpawnCustomer();
            _spawnRate = Random.Range(_spawnRatemin, _spawnRatemax);
        }
    }

    private void SpawnCustomer()
    {
        int randomPath = Random.Range(0, _pathList.pathScripts.Length);
        if (_occupied[randomPath] == false)
        {
            _path = _pathList.pathScripts[randomPath].GetPath();
            CustomerBehaviour customer = _customerPool.Get();
            customer.transform.position = _pathList._startPoint;
            customer.InitiateCustomer(_pathList._startPoint, _path[^1], _path);
            customer.GetCamera(_camera);
            customer.gameObject.SetActive(true);
            _occupied[randomPath] = true;
            customer._occupiedTable = randomPath;
        }
    }

    private void OnEnable()
    {
        _freeTable.Subscribe(FreeTable);
    }

    private void OnDisable()
    {
        _freeTable.Unsubscribe(FreeTable);
    }

    private void FreeTable(int table)
    {
        _occupied[table] = false;
    }
}
