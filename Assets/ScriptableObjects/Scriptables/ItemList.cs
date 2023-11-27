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
    }
}
