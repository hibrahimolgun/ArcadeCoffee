using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Upgrade Settings")]
    [SerializeField] private SORangedFloat _currentValue;
    [SerializeField] private float _upgradeValue;
    [SerializeField] private int _upgradeCost;
    [SerializeField] private SOint _totalCoins;
    [SerializeField] private float _costMultiplier;
    [SerializeField] private IntEvent _coinEvent;

    [Header("Upgrade Button Settings")]
    [SerializeField] private Image ButtonImage;
    [SerializeField] private TextMeshProUGUI _upgradeCostText;
    [SerializeField] private TextMeshProUGUI _currentValueText;

    private void Start()
    {
        _currentValueText.text = "% " + Mathf.RoundToInt((1-((_currentValue.value-0.5f)/0.5f))*100).ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonImage.transform.DOScale(1.2f, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonImage.transform.DOScale(1f, 0.2f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ButtonImage.transform.DOScale(1.1f, 0.2f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ButtonImage.transform.DOScale(1.2f, 0.2f);
        Upgrade();
    }

    private void Upgrade()
    {
        if (_totalCoins.value >= _upgradeCost)
        {
            _totalCoins.value -= _upgradeCost; // pay
            _coinEvent.InvokeAction(0);
            _currentValue.value += _upgradeValue; //upgraded
            _upgradeCost = Mathf.RoundToInt(_upgradeCost * _costMultiplier); // increase cost
            _upgradeCostText.text = "<sprite index=0> " + _upgradeCost.ToString(); // update text

            _currentValueText.text = "% " + Mathf.RoundToInt((1-((_currentValue.value-0.5f)/0.5f))*100).ToString(); // update text
            if (_currentValue.value <= 0.5f)
            {
                ButtonImage.DOFade(0.5f, 0f);
                this.enabled = false;
            }
        }
    }
}
