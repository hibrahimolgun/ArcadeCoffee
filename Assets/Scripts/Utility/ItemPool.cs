using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private Item itemPrefab;
    [SerializeField] private int poolSize = 15;
    [SerializeField] public ObjectPool<Item> pool;

    private void Awake()
    {
        pool = new ObjectPool<Item>(itemPrefab, poolSize, transform);
    }

    public Item GetItem()
    {
        var item = pool.Get();
        item.SetPool(this);
        return item;
    }
}
