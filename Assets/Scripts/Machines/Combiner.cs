using Unity.VisualScripting;
using UnityEngine;

[SelectionBase]
public class Combiner : MonoBehaviour
{
    [SerializeField] private ItemList _itemList;
    [SerializeField] private RecipeList _recipeList;

    [SerializeField] private Item itemPrefab;
    [SerializeField] private Item _input1;
    //[SerializeField] private Item _input2;
    [SerializeField] private Transform _outputPosition;
    private bool _isCombined;

    [SerializeField] private AnimationTrial _animationScript;

    private void Start()
    {
        _input1 = null;
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
            _input1 = player._holdingItem;
            _input1.transform.SetParent(transform);
            _input1.transform.position = _outputPosition.position;
            player._holdingItem = null;
            if (_animationScript != null) _animationScript.CorrectAnimation();
            return;
        }
        if (_input1 != null)
        {
            var playerItem = player._holdingItem;
            TryCombine(_input1, playerItem, player);
        }
    }

    private void TakeThis(PlayerController player)
    {
        if (_input1 != null)
        {
            player.HoldThis(_input1);
            _input1 = null;
            //_isCombined = false;
        }
    }

    private void TryCombine(Item item1, Item item2, PlayerController player)
    {
        foreach (var recipe in _recipeList.recipes)
        {
            if ((recipe.input1ID == item1._itemID && recipe.input2ID == item2._itemID) || (recipe.input1ID == item2._itemID && recipe.input2ID == item1._itemID))
            {
                Combine(recipe.outputID, player);
            }
        }
        if (_animationScript != null && _isCombined == false) _animationScript.IncorrectAnimation(); //animation
    }

    private void Combine(int outputID, PlayerController player)
    {
        _input1.InitItem(outputID);
        _input1.transform.position = _outputPosition.position;
        _input1.transform.SetParent(transform);
        if (_animationScript != null) _animationScript.CorrectAnimation();
        player.RidOfItem();
        TakeThis(player);
        //_input1 = null;
    }






}
