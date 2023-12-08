using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CountdownIndicator : MonoBehaviour
{
    [SerializeField] private Image countdownImage;
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private SelectFillMethod _selectFillMethod;


    private void Update()
    {
        if (_camera != null)
        {
            _canvas.transform.LookAt(_camera.transform);
        }
    }

    public void StartTimer(float duration)
    {
        CoundownVisual(duration);
    }

    private void CoundownVisual(float duration)
    {
        countdownImage.fillMethod = (Image.FillMethod) _selectFillMethod;
        countdownImage.DOFillAmount(1, duration).SetEase(Ease.Linear).OnComplete(() => Boink());
    }

    private void Boink()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(countdownImage.transform.DOScale(1.5f, 0.1f).SetEase(Ease.OutBounce));
        sequence.Append(countdownImage.transform.DOScale(1f, 0.1f).SetEase(Ease.OutBounce));
    }

    public void Reset()
    {
        countdownImage.fillAmount = 0;
    }

    private enum SelectFillMethod
    {
        Horizontal,
        Vertical,
        Radial90,
        Radial180,
        Radial360,
    }
}
