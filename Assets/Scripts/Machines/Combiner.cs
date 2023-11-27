using Unity.VisualScripting;
using UnityEngine;

[SelectionBase]
public class Combiner : MonoBehaviour
{
    [SerializeField] private ItemList _itemList;
    [SerializeField] private RecipeList _recipeList;

    [SerializeField] private Item itemPrefab;
    [SerializeField] private Item _input1;
    [SerializeField] private Item _input2;
    private Item _outputItem;
    [SerializeField] private Transform _outputPosition;

    private void Start()
    {
        _input1 = null;
        _input2 = null;
        _outputItem = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() == null) return;
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player._holdingItem == null)
        {
            TakeThis(player);
            return;
        } 
        if (_input1 == null)
        {
            //if (player._holdingItem == null) return;
            _input1 = player._holdingItem;
            _input1.transform.SetParent(transform);
            _input1.transform.position = _outputPosition.position;
            player._holdingItem = null;
        }
        else if (_input2 == null)
        {
            //if (player._holdingItem == null) return;
            _input2 = player._holdingItem;
            TryCombine();
            if (_outputItem != null)
            {
                player.RidOfItem();
            }
            else Debug.Log("No recipe");    
        }
    }

    private void TakeThis(PlayerController player)
    {
        Debug.Log("TakeThis");
        if (_outputItem == null)
        {
            Debug.Log("No output item");
            if (_input1 != null)
            {
                Debug.Log("Input1 present");
                player.HoldThis(_input1);
                _input1 = null;
                Debug.Log("Input1 taken");
            }
            else if (_input2 != null)
            {
                player.HoldThis(_input2);
                _input2 = null;
            }
        }
        else
        {
            player.HoldThis(_outputItem);
            _outputItem = null;
        }
    }

    private void TryCombine()
    {
        foreach (var recipe in _recipeList.recipes)
        {
            if ((recipe.input1ID == _input1._itemID && recipe.input2ID == _input2._itemID) || (recipe.input1ID == _input2._itemID && recipe.input2ID == _input1._itemID))
            {
                Combine(recipe.outputID);
            }
        }
    }

    private void Combine(int outputID)
    {
        _outputItem = _input1;
        _outputItem.InitItem(outputID);
        _input1 = null;
        _input2 = null;
    }






}
