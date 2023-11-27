using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Joystick _joystickStrength;
    [SerializeField] public Item _holdingItem; //Change to Item
    [SerializeField] private Transform _holdingPosition;
    [SerializeField] private ScriptableEvent _doubleTapEvent;

    private void Update()
    {
        MoveAndRotate();
    }

    private void MoveAndRotate()
    {
        transform.position += _joystickStrength.value * _speed * Time.deltaTime;
        if (_joystickStrength.value != Vector3.zero) transform.rotation = Quaternion.LookRotation(_joystickStrength.value);
    }

    public void HoldThis(Item item)
    {
        _holdingItem = item;
        _holdingItem.transform.SetParent(_holdingPosition);
        _holdingItem.transform.position = _holdingPosition.position;
    }

    private void DropItem()
    {
        _holdingItem.transform.SetParent(null);
        _holdingItem.transform.position = transform.position - new Vector3(0, -transform.position.y, 0);
        _holdingItem = null;
    }

    public void RidOfItem()
    {
        var item = _holdingItem;
        _holdingItem.transform.SetParent(null);
        _holdingItem = null;
        Destroy(item.gameObject);
    }

    private void Interact()
    {
        if (_holdingItem == null) return;
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
