using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class CustomerBehaviour1 : MonoBehaviour
{
    [Header("Order")]
    [SerializeField] private ItemList menu;
    [SerializeField] private Collider _collider;
    private int _orderID;
    private bool _orderComplete;
    private int _orderPrice;
    private bool _payReduce;
    private int _minPay = 10;
    private bool _orderTaken = false;
    private float _maxWaitTime = 30f;
    private int _occupiedTableIndex;

    [Header("Customer Movement")]
    [SerializeField] private CustomerMovement _movement;
    private bool _reachedTable;

    [Header("Put to Table")]
    [SerializeField] private PathScriptList _tableTopPoints;
    private Item _itemToTable;
    private int _occupiedTable = -1;

    [Header("Correct Order Animation")]
    [SerializeField] private float _jumpHeight = 0.5f;
    [SerializeField] private float _jumpDuration = 1f;

    [Header("LookAt")]
    [SerializeField] private SOVector3 _playersTransform;
    [SerializeField] private Camera _camera;

    [Header("Visualization")]
    [SerializeField] private SpriteHeader _customerHeader;

    [Header("Customer Shape&Color")] //TODO ALSO CHANGE MESH
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Shader _shader;
    [SerializeField] private Shader _shaderToon;

    [Header("Events")]
    [SerializeField] private IntEvent _customerLeft;

    [Header("Spawner Pool")]
    [SerializeField] private SpawnerCustomer _spawnerCustomer;

    private void Update()
    {
        if (_reachedTable == true)
        {
            transform.rotation = Quaternion.LookRotation(_playersTransform.value - transform.position);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }

    public void InitiateCustomer(int pathIndex, int occupiedTableIndex)
    {
        _occupiedTableIndex = occupiedTableIndex;
        _collider.enabled = false;
        _orderComplete = false;
        _reachedTable = false;
        _orderTaken = false;
        _orderID = -1;
        RandomAppearance();
        _occupiedTable = pathIndex;
        _movement.GoToTable(pathIndex);
    }

    private void RandomAppearance()
    {
        _renderer.materials[0].shader = _shader;
        var hue = Random.Range(0f, 1f);
        _renderer.materials[0].color = Color.HSVToRGB(hue, 0.5f, 1f);
        _renderer.materials[0].shader = _shaderToon;
    }

    public void ReadyToOrder()
    {
        _collider.enabled = true;
        _reachedTable = true;
        StartCoroutine(LeaveWithNoOrder(_maxWaitTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            if (_orderTaken == false) TakeOrder(player);
            if (player._holdingItem == null) return; // If player is not holding an item, return
            if (_orderComplete == true) return; // If order is already complete, return
            if (player._holdingItem._itemID == _orderID)
            {
                PutItemToTable(player);
                Debug.Log("Correct item! Order complete!");
                OrderCompletedRoutine();
            }
            else
            {
                Debug.Log("Wrong item!");
            }
        }
    }

    private void TakeOrder(PlayerController player)
    {
        if (player._holdingItem == null)
        {
            RandomOrder();
            _orderTaken = true;
        }
        
    }

    private void RandomOrder()
    {
        var randomItem = Random.Range(0, menu.items.Count); //random item
        _orderID = menu.items[randomItem].id; //init id
        _orderPrice = menu.GetItem(_orderID).price; //init price
        _payReduce = true; //start reducing pay
        StartCoroutine(ReducePay()); //start reducing pay
        if (_customerHeader != null) _customerHeader.InitHeader(menu.GetItem(_orderID).sprite, _camera); //init header
        Debug.Log($"Customer wants {menu.items[randomItem].name}");
    }

    private IEnumerator ReducePay()
    {
        while (_payReduce == true)
        {
            yield return new WaitForSeconds(1f);
            _orderPrice -= 1;
            if (_orderPrice <= _minPay)
            {
                _payReduce = false;
            }
        }
    }

    private void PutItemToTable(PlayerController player)
    {
        _itemToTable = player._holdingItem;
        _itemToTable.transform.position = _tableTopPoints._tablePoints[_occupiedTable];
        _itemToTable.ItemDestroy(menu.GetItem(_orderID).timeToFinish);
        _itemToTable.transform.SetParent(null);
        _itemToTable = null;
        player._holdingItem = null;
    }

    private void OrderCompletedRoutine()
    {
        _orderComplete = true;
        _collider.enabled = false;
        _payReduce = false;
        if (_customerHeader != null) _customerHeader.GainCoin(_orderPrice, menu.GetItem(_orderID).price);
        transform.DOJump(transform.position, _jumpHeight, 1, _jumpDuration).SetEase(Ease.InOutBounce);

        _reachedTable = false;
        _movement.Leave(menu.GetItem(_orderID).timeToFinish);
    }

    public void DisableCallback()
    {
        _customerLeft.InvokeAction(_occupiedTableIndex);
        gameObject.SetActive(false);
        _spawnerCustomer.ReturnToPool(this);
    }

    public void GetCamera(Camera camera)
    {
        _camera = camera;
    }

    public void SetSpawner(SpawnerCustomer spawnerCustomer)
    {
        _spawnerCustomer = spawnerCustomer;
    }

    private IEnumerator LeaveWithNoOrder(float maxWaitTime)
    {
        yield return new WaitForSeconds(maxWaitTime);
        if (_orderTaken == false) _movement.Leave(0f);        
    }
}
