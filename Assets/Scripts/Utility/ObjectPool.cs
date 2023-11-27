using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private const int ExpandStep = 10;
    
    private readonly Queue<T> _items;

    private readonly T _prefab;
    private readonly Transform _parent;

    public ObjectPool(T prefab, int count, Transform parent)
    {
        _items = new Queue<T>(count);
        this._prefab = prefab;
        this._parent = parent;
        InitializePool(prefab, count, parent);
    }

    public T Get()
    {
        if (_items.Count == 0)
        {
            int c = 0;
            while (c < ExpandStep) 
            {
                InstantiateInstance(this._prefab, this._parent);
                c++;
            }        
        }

        return _items.Dequeue();
    }

    public void Return(T t)
    {
        _items.Enqueue(t);
    }
    
    private void InitializePool(T prefab, int count, Transform parent)
    {
        for (int i = 0; i < count; i++)
        {
            InstantiateInstance(prefab, parent);
        }
    }
    
    private void InstantiateInstance(T prefab, Transform parent)
    {
        T instance = Object.Instantiate(prefab, parent, true);
        _items.Enqueue(instance);
        instance.gameObject.SetActive(false);
    }
}
