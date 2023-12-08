using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "ScriptableObjects/ItemList", order = 1)]
public class ItemList : ScriptableObject
{
    public List<ItemSpecs> items;

    [Serializable]
    public struct ItemSpecs
    {
        public int id;
        public string name;
        public Sprite sprite;
        public Mesh mesh;
        public Material material;
        public float size;
        public int price;
        public float timeToFinish;
        public Vector3 rotation;
    }

    public ItemSpecs GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }
}
