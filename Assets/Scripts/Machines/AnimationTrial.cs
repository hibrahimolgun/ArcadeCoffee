using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AnimationTrial : MonoBehaviour
{
    [Header("Correct Animation")]
    [SerializeField] private float _yStrength=1.07f;
    [SerializeField] private float _xStrength=1.04f;
    [SerializeField] private float _zStrength=1.04f;
    [SerializeField] private float _duration=0.2f;
    
    [Header("Incorrect Animation")]
    [SerializeField] private Vector3 _shakeStrength=new(0,0,0.5f);
    [SerializeField] private float _shakeDuration=0.05f;
    [SerializeField] private float _shrinkStrength=0.97f;


    /* private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<PlayerController>(out var player)) return;
        //CorrectAnimation();
        IncorrectAnimation();
    } */

    public void CorrectAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScaleY(_yStrength, _duration).SetEase(Ease.OutBounce));
        sequence.Join(transform.DOScaleX(_xStrength, _duration).SetEase(Ease.OutBounce));
        sequence.Join(transform.DOScaleZ(_zStrength, _duration).SetEase(Ease.OutBounce));
        sequence.Append(transform.DOScaleY(1f, _duration).SetEase(Ease.OutBounce));
        sequence.Join(transform.DOScaleX(1f, _duration).SetEase(Ease.OutBounce));
        sequence.Join(transform.DOScaleZ(1f, _duration).SetEase(Ease.OutBounce));
    }

    public void IncorrectAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalRotateQuaternion(Quaternion.Euler(_shakeStrength), _shakeDuration).SetEase(Ease.OutBounce));
        sequence.Join(transform.DOScale(_shrinkStrength, _shakeDuration).SetEase(Ease.OutBounce));
        sequence.Append(transform.DOLocalRotateQuaternion(Quaternion.Euler(-2*_shakeStrength), _shakeDuration).SetEase(Ease.OutBounce));
        sequence.Append(transform.DOLocalRotateQuaternion(Quaternion.Euler(2*_shakeStrength), _shakeDuration).SetEase(Ease.OutBounce));
        sequence.Append(transform.DOLocalRotateQuaternion(Quaternion.Euler(-2*_shakeStrength), _shakeDuration).SetEase(Ease.OutBounce));
        sequence.Append(transform.DOLocalRotateQuaternion(Quaternion.Euler(2*_shakeStrength), _shakeDuration).SetEase(Ease.OutBounce));
        sequence.Append(transform.DOLocalRotateQuaternion(Quaternion.Euler(-_shakeStrength), _shakeDuration).SetEase(Ease.OutBounce));
        sequence.Join(transform.DOScale(1f, _shakeDuration).SetEase(Ease.OutBounce));

    }
}
