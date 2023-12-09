using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[SelectionBase]
public class SimpleItemGiver : MonoBehaviour
{
    [SerializeField] private ItemPool _itemPool;
    [SerializeField] private int outputItemID;
    private Item _outputItem;

    [SerializeField] private AnimationTrial _animationScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            if (player._holdingItem == null)
            {
                _outputItem = _itemPool.GetItem();
                _outputItem.gameObject.SetActive(true);
                _outputItem.InitItem(outputItemID);
                player.HoldThis(_outputItem);
                if (_animationScript != null) _animationScript.CorrectAnimation(); //animation
                _outputItem = null;
                return;
            }
        }
    }
}
