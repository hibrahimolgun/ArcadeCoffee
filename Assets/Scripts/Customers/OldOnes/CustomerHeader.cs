using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CustomerHeader : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private IntEvent _coinEvent;
    private bool _init = false;

    private void Start()
    {
        _init = false;
        _canvas.enabled = false;
        _image.enabled = false;
        _text.enabled = false;
    }

    private void Update()
    {
        if (_init == true) _canvas.transform.forward = _camera.transform.forward;
    }

    public void InitHeader(Sprite sprite, Camera camera) //call when customer orders with sprite of item
    {
        _camera = camera;
        _init = true;
        _image.sprite = sprite;
        _canvas.enabled = true;
        _image.enabled = true;
    }

    public void GainCoin(int amount) //call when customer pays
    {
        _image.enabled = false;
        _text.text = "+" + amount;
        _text.enabled = true;
        _init = false;
        TextAnimation();
        _coinEvent.InvokeAction(amount);
    }

    private void TextAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(DOVirtual.Float(1.5f, 0f, 5f, value => _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, value)));
        sequence.Join(_text.transform.DOLocalMoveY(1f, 5f).SetEase(Ease.Linear)).OnComplete(() =>
        {
            _text.enabled = false;
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1f);
            _text.transform.localPosition = Vector3.zero;
            _canvas.enabled = false;
        });
    }
}
