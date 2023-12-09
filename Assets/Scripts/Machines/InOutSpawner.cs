using System.Collections;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

[SelectionBase]
public class InOutSpawner : MonoBehaviour
{
    [Header("Items")]
    //[SerializeField] private Item itemPrefab; //item prefab
    [SerializeField] private ItemPool _itemPool;
    [SerializeField] private int _inputItemID;
    [SerializeField] private int _outputItemID;
    private Item _outputItem;

    [Header("Spawn Settings")]
    [SerializeField] private SORangedFloat _timerRange;
    [SerializeField] private Transform _outputPosition; //output position

    private bool _isSpawning; //is spawning
    private bool _checkInputItem;
    private bool _outputReady;

    [Header("Animation")]
    [SerializeField] private AnimationTrial _animationScript;
    [SerializeField] private CountdownIndicator _countdownIndicator;
    

    private void Start()
    {
        if (_countdownIndicator != null) _countdownIndicator.Reset(); //reset timer
        if (_inputItemID >= 0) _checkInputItem = true; //if negative no input
        else _checkInputItem = false; //else check input item is false
        //Debug.Log("Check Input Item: " + _checkInputItem); //log check input item
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
                if (_countdownIndicator != null) _countdownIndicator.Reset(); //reset timer
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
                //Debug.Log("No item"); //if player holding item is null, return
                if (_animationScript != null) _animationScript.IncorrectAnimation();
                return;
            }  
            if (player._holdingItem._itemID == _inputItemID) //if player holding item is input item
            {
                player.RidOfItem(); //player holding item is null
                StartCoroutine(StartSpawning()); //start spawning
                if (_animationScript != null) _animationScript.CorrectAnimation();
            }
            else if (_animationScript != null) _animationScript.IncorrectAnimation();
        }
        else //else
        {
            StartCoroutine(StartSpawning()); //start spawning
            if (_animationScript != null) _animationScript.CorrectAnimation();
        }
    }

    private IEnumerator StartSpawning()
    {
        if (_isSpawning == true) yield break; //if is spawning is true, break
        _isSpawning = true; //start spawn
        if (_countdownIndicator != null) _countdownIndicator.StartTimer(_timerRange.value * _timerRange.max); //start timer
        yield return new WaitForSeconds(_timerRange.value * _timerRange.max); //wait for timer
        _outputItem = _itemPool.GetItem();
        _outputItem.gameObject.SetActive(true);
        _outputItem.InitItem(_outputItemID); //init output item
        _outputItem.transform.SetParent(_outputPosition); //set parent
        _outputItem.transform.localPosition = Vector3.zero; //set local position
        _outputReady = true; //output ready
        _isSpawning = false; //stop spawn
    }
}
