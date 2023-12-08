using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[SelectionBase]
public class SimpleItemGiver : MonoBehaviour
{
    [SerializeField] private Item _itemPrefab;
    [SerializeField] private int outputItemID;
    private Item _outputItem;

    [SerializeField] private AnimationTrial _animationScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() == null) return;
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player._holdingItem == null)
        {
            _outputItem = Instantiate(_itemPrefab, transform.position, Quaternion.identity);
            _outputItem.InitItem(outputItemID);
            player.HoldThis(_outputItem);
            if (_animationScript != null) _animationScript.CorrectAnimation(); //animation
            _outputItem = null;
            return;
        }
    }
}
