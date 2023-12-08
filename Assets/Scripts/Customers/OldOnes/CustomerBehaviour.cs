using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations;
using Random = UnityEngine.Random;

[SelectionBase]
public class CustomerBehaviour : MonoBehaviour
{
    //TODO SEPERATE MOVEMENT
    [Header("Order")]
    [SerializeField] private ItemList menu;
    [SerializeField] private Collider _collider;
    private int _orderID;
    private bool _orderComplete;
    private int _orderPrice;
    private bool _payReduce;
    private int _minPay = 10;
    private bool _orderTaken = false;
    

    [Header("Movement")]
    [SerializeField] private Vector3 _startPoint;
    [SerializeField] private Vector3 _endPoint;
    [SerializeField] private Vector3[] _path;
    [SerializeField] private float _speed = 1f;
    private bool _reachedEnd = false;
    public int _occupiedTable=-1; // The table the customer is sitting at
    [SerializeField] private IntEvent _customerLeft;
    
    //Put To Table
    [SerializeField] private PathScriptList _tableTopPoints;
    private Item _itemToTable;

    [Header("Correct Order Animation")]
    [SerializeField] private float _jumpHeight = 0.5f;
    [SerializeField] private float _jumpDuration = 1f;

    [Header("LookAt")]
    [SerializeField] private SOVector3 _playersTransform;
    [SerializeField] private Camera _camera;
    [SerializeField] private SpriteHeader _customerHeader;

    [Header("Customer Shape&Color")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Shader _shader;
    [SerializeField] private Shader _shaderToon;

    private void Update()
    {
        CheckifReached();
        if (transform.position == _endPoint) 
        {
            transform.rotation = Quaternion.LookRotation(_playersTransform.value - transform.position);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }

    private void CheckifReached()
    {
        if (_reachedEnd == true) return;
        if (transform.position == _path[^2])
        {
            transform.DOJump(_endPoint, 1f, 1, 1f).SetEase(Ease.InBack);
            Debug.Log("Customer reached the end of the path");
            _collider.enabled = true; //enable collider
            _reachedEnd = true; //set reached end to true
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
    //TODO SEPERATE TO ACTION TO TABLE SCRIPT MAYBE?
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
        StartCoroutine(Leave());
        if (_customerHeader != null) _customerHeader.GainCoin(_orderPrice, menu.GetItem(_orderID).price);
        transform.DOJump(transform.position, _jumpHeight, 1, _jumpDuration).SetEase(Ease.InOutBounce);
    }

    private void GotoPoint(Vector3[] pathPoints)
    {
        var sequence = DOTween.Sequence();
        var point0 = _startPoint;
        for (int i = 0; i < pathPoints.Length-1; i++)
        {
            sequence.Append(transform.DOMove(pathPoints[i], (pathPoints[i] - point0).magnitude/_speed).SetEase(Ease.Linear));
            sequence.Join(transform.DOLookAt(pathPoints[i], 0.5f).SetEase(Ease.Linear));
            point0 = pathPoints[i];
        }
    }

    public void InitiateCustomer(Vector3 startPoint, Vector3 endPoint, Vector3[] path)
    {
        _collider.enabled = false;
        _reachedEnd = false;
        _orderComplete = false;
        _startPoint = startPoint;
        _endPoint = endPoint;
        _path = path;
        _orderComplete = false;
        transform.position = _startPoint;
        GotoPoint(_path);
        InitializeColor();
    }

    private void InitializeColor()
    {
        _renderer.materials[0].shader = _shader;
        var hue = Random.Range(0f, 1f);
        _renderer.materials[0].color = Color.HSVToRGB(hue, 0.5f, 1f);
        _renderer.materials[0].shader = _shaderToon;
    }

    private IEnumerator Leave()
    {
        yield return new WaitForSeconds(menu.GetItem(_orderID).timeToFinish); // Wait for the time it takes to finish the order
        GoBack(_path); // Go back to the start
    }

    private void GoBack(Vector3[] pathPoints)
    {
        pathPoints = pathPoints.SkipLast(1).ToArray();
        pathPoints = pathPoints.Reverse().ToArray();
        pathPoints = pathPoints.Append(_startPoint).ToArray();
        
        var sequence = DOTween.Sequence();
        var point0 = _endPoint;
        for (int i = 0; i < pathPoints.Length; i++)
        {
            sequence.Append(transform.DOMove(pathPoints[i], (pathPoints[i] - point0).magnitude/_speed).SetEase(Ease.Linear));
            sequence.Join(transform.DOLookAt(pathPoints[i], 0.5f).SetEase(Ease.Linear));
            point0 = pathPoints[i];
        }
        sequence.Append(transform.DOMove(_startPoint, (_startPoint - point0).magnitude/_speed).SetEase(Ease.Linear));
        sequence.AppendCallback(() => DisableCallback());
    }

    private void DisableCallback()
    {
        _customerLeft.InvokeAction(_occupiedTable);
        gameObject.SetActive(false);
    }

    private IEnumerator ReducePay()
    {
        while (_payReduce == true)
        {
            yield return new WaitForSeconds(1f);
            _orderPrice -= 1;
            if (_orderPrice <= 0)
            {
                _payReduce = false;
                _orderPrice = _minPay;
            }
        }
    }

    public void GetCamera(Camera camera) 
    {
        _camera = camera;
    }
}
