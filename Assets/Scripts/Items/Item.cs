using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class Item : MonoBehaviour
{
    public int _itemID;
    public string _itemName;
    public Sprite _itemSprite;
    public Mesh _itemMesh;
    public Material _itemMaterial;
    public ItemList itemList;
    public float _itemSize;
    
    public ItemPool _itemPool;
    
    [Header("Components")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] public Collider _collider;

    private void Start()
    {
        if (_itemID != 100) _collider.enabled = false;
    }

    public void InitItem(int itemID)
    {
        _itemID = itemID;
        var myItem = itemList.GetItem(itemID);
        _itemName = myItem.name;
        _itemSprite = myItem.sprite;
        _itemMesh = myItem.mesh;
        _itemMaterial = myItem.material;
        _itemSize = myItem.size;

        _renderer.material = _itemMaterial;
        _meshFilter.mesh = _itemMesh;
        transform.localScale = Vector3.one * _itemSize;
        transform.localPosition = Vector3.zero;
    }

    public void ItemDropped()
    {
        DOVirtual.DelayedCall(3f, () => _collider.enabled = true);
    }

    public void ItemDestroy(float destroyAfterSeconds)
    {
        DOVirtual.DelayedCall(destroyAfterSeconds, () => 
        {
            gameObject.SetActive(false);
            transform.SetParent(_itemPool.transform);
            _itemPool.pool.Return(this);
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            if (player._holdingItem == null) 
            {
                if (_itemID == 100)
                {
                    player.HoldThis(this);
                }
                else return;
            }
            else if (player._holdingItem._itemID == 100)
            {
                _collider.enabled=false;
                ItemDestroy(0);
                Debug.Log("destroyed");
            } 
            else Debug.Log("use Broom");
        }
    }

    public Vector3 ReturnRotation()
    {
        return itemList.GetItem(_itemID).rotation;
    }

    public void SetPool(ItemPool itemPool)
    {
        _itemPool = itemPool;
    }
}
