using UnityEngine;
using DG.Tweening;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Joystick _joystickStrength;
    [SerializeField] public Item _holdingItem; //Change to Item
    [SerializeField] private Transform _holdingPosition;
    [SerializeField] private ScriptableEvent _doubleTapEvent;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SOVector3 _playerPosition;

    private void Update()
    {
        MoveAndRotate();
    }

    private void MoveAndRotate()
    {
        _rigidbody.position += _joystickStrength.value * _speed * Time.deltaTime;
        if (_joystickStrength.value != Vector3.zero) _rigidbody.MoveRotation(Quaternion.LookRotation(_joystickStrength.value));
        _playerPosition.value = transform.position;
    }

    public void HoldThis(Item item)
    {
        _holdingItem = item;
        _holdingItem._collider.enabled = false;
        _holdingItem.transform.SetParent(_holdingPosition);
        _holdingItem.transform.position = _holdingPosition.transform.position;
        _holdingItem.transform.localEulerAngles = _holdingItem.ReturnRotation();
    }

    private void DropItem()
    {
        _holdingItem.transform.SetParent(null);
        var finalPosition = transform.position - new Vector3(0, -transform.position.y, 0);

        if (_holdingItem._itemID == 100) _holdingItem.transform.position = finalPosition; //leave the broom on the floor
        else _holdingItem.transform.DOJump(finalPosition, 2f, 1, 1f); //throw the item

        _holdingItem.transform.eulerAngles = Vector3.zero;
        _holdingItem.ItemDropped();
        _holdingItem = null;
    }

    public void RidOfItem()
    {
        var item = _holdingItem;
        _holdingItem.transform.SetParent(null);
        _holdingItem = null;
        item.ItemDestroy(0f);
    }

    private void Interact()
    {
        if (_holdingItem == null) return;
        //if (_holdingItem._itemID == 100) Debug.Log("Cannot Drop"); //100 is the ID for the broom
        else DropItem();
    }

    private void OnEnable()
    {
        _doubleTapEvent.Subscribe(Interact);
    }

    private void OnDisable()
    {
        _doubleTapEvent.Unsubscribe(Interact);
    }

}
