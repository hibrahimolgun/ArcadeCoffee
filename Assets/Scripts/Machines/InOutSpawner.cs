using System.Collections;
using UnityEditor;
using UnityEngine;

[SelectionBase]
public class InOutSpawner : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private Item itemPrefab; //item prefab
    [SerializeField] private int _inputItemID;
    [SerializeField] private int _outputItemID;
    [SerializeField] private ItemList itemList; //item list
    private Item _outputItem;

    [Header("Spawn Settings")]
    [SerializeField] private float _timer; //timer for spawning
    [SerializeField] private Transform _outputPosition; //output position

    private bool _isSpawning; //is spawning
    private bool _checkInputItem;
    private bool _outputReady;

    private void Start()
    {
        if (_inputItemID >= 0) _checkInputItem = true; //if negative no input
        else _checkInputItem = false; //else check input item is false
        Debug.Log("Check Input Item: " + _checkInputItem); //log check input item
    }

    //trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<PlayerController>(out var player)) return;
        if (_outputReady == false) Spawn(player);
        else
        {
            if (player._holdingItem == null)
            {
                player.HoldThis(_outputItem); //hold output item
                _outputItem = null;
                _outputReady = false;
            }
        }
    }

    private void Spawn(PlayerController player)
    {
        if (_checkInputItem == true) //if check input item is true
        {
            if(player._holdingItem == null)
            {
                Debug.Log("No item"); //if player holding item is null, return
                return;
            }  
            if (player._holdingItem._itemID == _inputItemID) //if player holding item is input item
            {
                player.RidOfItem(); //player holding item is null
                StartCoroutine(StartSpawning()); //start spawning
            }
            else Debug.Log("Wrong Item");
        }
        else //else
        {
            StartCoroutine(StartSpawning()); //start spawning
        }
    }

    private IEnumerator StartSpawning()
    {
        if (_isSpawning == true) yield break; //if is spawning is true, break
        _isSpawning = true; //start spawn
        yield return new WaitForSeconds(_timer); //wait for timer
        _outputItem = itemPrefab; //init item prefab
        _outputItem.InitItem(_outputItemID); //init output item
        _outputItem = Instantiate(_outputItem, _outputPosition.position, Quaternion.identity); //spawn output item
        _outputReady = true; //output ready
        _isSpawning = false; //stop spawn
    }
}
