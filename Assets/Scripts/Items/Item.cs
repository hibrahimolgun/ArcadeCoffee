using UnityEngine;

public class Item : MonoBehaviour
{
    public int _itemID;
    public string _itemName;
    public Sprite _itemSprite;
    public Mesh _itemMesh;
    public Material _itemMaterial;
    public ItemList itemList;

    [Header("Components")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private MeshFilter _meshFilter;

    public void InitItem(int itemID)
    {
        _itemID = itemID;
        var myItem = itemList.items[itemID];
        _itemName = myItem.name;
        _itemSprite = myItem.sprite;
        _itemMesh = myItem.mesh;
        _itemMaterial = myItem.material;

        _renderer.material = _itemMaterial;
        _meshFilter.mesh = _itemMesh;
    }
}
