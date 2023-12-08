using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CoinGain : MonoBehaviour
{
    [Header("Coin Gain")]
    [SerializeField] private TextMeshProUGUI _coinGainText;
    [SerializeField] private IntEvent _coinGainEvent;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private SOint _totalCoin;

    [Header("Animation")]
    [SerializeField] private GameObject _coinGainObject;
    [SerializeField] private RectTransform _coinGainTransform;
    [SerializeField] private Image _coinGainImage;
    [SerializeField] private SelectEase _selectCoinEase;
    [SerializeField] private float _duration;
    [SerializeField] private float _fadeTo;
    [SerializeField] private float _dropLength;
    [SerializeField] private float _scaleTo;
    private float _initialY;
    
    [Header("Coin Pop Animation")]
    [SerializeField] private RectTransform _coinPopTransform;
    [SerializeField] private SelectEase _selectPopEase;
    [SerializeField] private float _popDuration;
    [SerializeField] private float _popScaleTo;


    private void OnEnable()
    {
        _coinGainEvent.Subscribe(OnCoinGain);
    }

    private void OnDisable()
    {
        _coinGainEvent.Unsubscribe(OnCoinGain);
    }

    private void Awake()
    {
        _coinGainObject.SetActive(false);
        _coinText.text = _totalCoin.value.ToString();
    }

    private void Start()
    {
        _initialY = _coinGainTransform.anchoredPosition.y;
    }

    private void OnCoinGain(int coinGain)
    {
        if (coinGain > 0) PlayCoinGainAnimation(coinGain);
        _totalCoin.value += coinGain;
        _coinText.text = _totalCoin.value.ToString();
    }

    private void PlayCoinGainAnimation(int coinGain)
    {
        _coinGainText.text = $"+{coinGain}";
        _coinGainObject.SetActive(true);
        var sequence = DOTween.Sequence();
        sequence.Append(_coinGainTransform.DOAnchorPosY(_initialY - _dropLength, _duration).SetEase((Ease) _selectCoinEase));
        sequence.Join(_coinGainImage.DOFade(_fadeTo, _duration).SetEase((Ease) _selectCoinEase));
        sequence.Join(_coinGainText.DOFade(_fadeTo, _duration).SetEase((Ease) _selectCoinEase));
        sequence.Join(_coinGainTransform.DOScale(_scaleTo, _duration).SetEase((Ease) _selectCoinEase));
        sequence.AppendCallback(() => AnimationComplete());
    }

    private void AnimationComplete()
    {
        PlayPop();
        _coinGainObject.SetActive(false);
        _coinGainTransform.anchoredPosition = new Vector2(_coinGainTransform.anchoredPosition.x, _initialY);
        _coinGainImage.color = new Color(_coinGainImage.color.r, _coinGainImage.color.g, _coinGainImage.color.b, 1);
        _coinGainText.color = new Color(_coinGainText.color.r, _coinGainText.color.g, _coinGainText.color.b, 1);
        _coinGainTransform.localScale = new Vector3(1, 1, 1);
    }

    private void PlayPop()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_coinPopTransform.DOScale(_popScaleTo, _popDuration).SetEase((Ease) _selectPopEase));
        sequence.Append(_coinPopTransform.DOScale(1, _popDuration).SetEase((Ease) _selectPopEase));
    }

    private enum SelectEase
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        BounceIn,
        BounceOut,
        BounceInOut,
        BackIn,
        BackOut,
        BackInOut,
        ElasticIn,
        ElasticOut,
        ElasticInOut
    }
}
