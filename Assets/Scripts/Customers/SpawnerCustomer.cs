
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerCustomer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _camera;
    [SerializeField] private CustomerBehaviour1 _customerPrefab;
    [SerializeField] private PathScriptList _pathList;
    [SerializeField] private TableLockList _tableLockList;
    [SerializeField] int[] tableNumbers;
    [SerializeField] IntEvent _unlockEvent;
    private bool[] _occupied;

    private ObjectPool<CustomerBehaviour1> _customerPool;

    [Header("Spawn Settings")]
    private float _spawnRate;
    //[SerializeField] private float _spawnRatemin = 3f;
    //[SerializeField] private float _spawnRatemax = 20f;
    [SerializeField] private SORangedFloat _spawnRateRange;
    [SerializeField] private IntEvent _freeTable;

    private void Awake()
    {
        _customerPool = new ObjectPool<CustomerBehaviour1>(_customerPrefab, 5, transform);
    }

    private void Start()
    {
        AvailableTables();
    }

    private void AvailableTables()
    {
        for (int i = 0; i < _tableLockList._tableLocks.Length; i++)
        {
            if (_tableLockList._tableLocks[i].isLocked == false)
            {
                tableNumbers = tableNumbers.Append(_tableLockList._tableLocks[i].tableNumber).ToArray();
            }
        }
        _occupied = new bool[tableNumbers.Length];
    }

    private void Update()
    {
        _spawnRate -= Time.deltaTime;
        if (_spawnRate <= 0)
        {
            SpawnCustomer();
            _spawnRate = Random.Range(_spawnRateRange.value * _spawnRateRange.min, _spawnRateRange.value * _spawnRateRange.max);
        }
    }

    private void SpawnCustomer()
    {
        int randomPath = Random.Range(0, tableNumbers.Length);
        if (_occupied[randomPath] == false)
        {
            CustomerBehaviour1 customer = _customerPool.Get();
            customer.InitiateCustomer(tableNumbers[randomPath], randomPath);
            customer.GetCamera(_camera);
            customer.SetSpawner(this);
            customer.gameObject.SetActive(true);
            _occupied[randomPath] = true;
        }
    }

    public void ReturnToPool(CustomerBehaviour1 customer)
    {
        _customerPool.Return(customer);
    }

    private void OnEnable()
    {
        _freeTable.Subscribe(FreeTable);
        _unlockEvent.Subscribe(AddTable);
    }

    private void OnDisable()
    {
        _freeTable.Unsubscribe(FreeTable);
        _unlockEvent.Unsubscribe(AddTable);
    }

    private void AddTable(int table)
    {
        tableNumbers = tableNumbers.Append(table).ToArray();
        _occupied = _occupied.Append(false).ToArray();
    }

    private void FreeTable(int table)
    {
        _occupied[table] = false;
    }
}
