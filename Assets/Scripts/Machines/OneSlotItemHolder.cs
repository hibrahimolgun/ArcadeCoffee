using UnityEngine;

[SelectionBase]
public class OneSlotItemHolder : MonoBehaviour
{
    [SerializeField] private Item _itemPrefab;
    [SerializeField] private Item ItemSlot;
    [SerializeField] private Transform SlotPosition;
    [SerializeField] private ItemList _itemList;
    [SerializeField] private int _itemID; //initial item on the holder
    [SerializeField] private bool _spawnWithItem;

    private void Start()
    {
        if (_spawnWithItem)
        {
            ItemSlot = Instantiate(_itemPrefab, SlotPosition.position, Quaternion.identity);
            ItemSlot.transform.SetParent(transform);
            ItemSlot.transform.position = SlotPosition.position;
            ItemSlot.InitItem(_itemID);
            ItemSlot.transform.localEulerAngles = ItemSlot.ReturnRotation();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            if (ItemSlot == null) // If item slot is empty, place item in slot  
            {
                if (player._holdingItem == null) return;
                ItemSlot = player._holdingItem;
                ItemSlot.transform.SetParent(transform);
                ItemSlot.transform.position = SlotPosition.position;
                player._holdingItem = null;
            }
            else // If ItemSlot is occupied, take it
            {
                player.HoldThis(ItemSlot);
                ItemSlot = null;
            }
        }
    }
}
