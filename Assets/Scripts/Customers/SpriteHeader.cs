using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SpriteHeader : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private SpriteRenderer _bubbleRenderer;
    [SerializeField] private SpriteRenderer _orderSpriteRenderer;
    [SerializeField] private SpriteRenderer _emojiSpriteRenderer;
    [SerializeField] private Sprite[] _happySprites;
    [SerializeField] private Sprite[] _sadSprites; //0 = sad, 1 = happy
    [SerializeField] private IntEvent _coinEvent;
    [SerializeField] private float _bubbleSize = 0.05f;
    [SerializeField] private float _fadeTime = 2f;
    [SerializeField] private float _popTime = 0.1f;
    private bool init = false;

    private void Start()
    {
        _bubbleRenderer.enabled = false;
        _orderSpriteRenderer.enabled = false;
        _emojiSpriteRenderer.enabled = false;
    }

    private void Update()
    {
        if (init == false) return;
        _bubbleRenderer.transform.forward = _camera.transform.forward;
        _orderSpriteRenderer.transform.forward = _camera.transform.forward;
    }

    public void InitHeader(Sprite sprite, Camera camera) //call when customer orders with sprite of item
    {
        _camera = camera;
        init = true;
        _orderSpriteRenderer.sprite = sprite;
        _bubbleRenderer.enabled = true;
        _orderSpriteRenderer.enabled = true;
        _emojiSpriteRenderer.enabled = false;
    }

    public void GainCoin(int amount, int maxPay)
    {
        _orderSpriteRenderer.enabled = false;
        _emojiSpriteRenderer.sprite = (amount > maxPay/2) ? _happySprites[Random.Range(0,_happySprites.Length)] : _sadSprites[Random.Range(0, _sadSprites.Length)]; //if amount is greater than half of maxPay, use happy emoji
        _emojiSpriteRenderer.enabled = true;
        init = false;
        _coinEvent.InvokeAction(amount);
        FadeOut();
    }

    private void FadeOut()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_emojiSpriteRenderer.DOFade(0.5f, _fadeTime).SetEase(Ease.InExpo));
        sequence.Join(_bubbleRenderer.transform.DOScale(0f, _popTime).SetDelay(_fadeTime-_popTime).SetEase(Ease.InBack)).OnComplete(() =>
        {
            _bubbleRenderer.enabled = false;
            _emojiSpriteRenderer.enabled = false;
            _emojiSpriteRenderer.color = new Color(_emojiSpriteRenderer.color.r, _emojiSpriteRenderer.color.g, _emojiSpriteRenderer.color.b, 1f);
            _bubbleRenderer.transform.localScale = Vector3.one * _bubbleSize;
        });
    }



    
}
