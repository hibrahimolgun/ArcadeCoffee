using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TableLockList", menuName = "ScriptableObjects/TableLockList", order = 1)]
public class TableLockList : ScriptableObject
{
    [SerializeField] public TableLock[] _tableLocks;
    
    
    [Serializable]
    public struct TableLock
    {
        public int tableNumber;
        public bool isLocked;
        public int price;
    }
}
