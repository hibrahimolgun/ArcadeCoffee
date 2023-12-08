using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using DG.Tweening;


public class PanelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject PanelToOpen;
    [SerializeField] private Image ButtonImage;
    [SerializeField] private Joystick3D joystick;

    private void Start()
    {
        PanelToOpen.transform.localScale = Vector3.zero;
        PanelToOpen.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonImage.transform.DOScale(1.2f, 0.2f);
        //joystick.enabled = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonImage.transform.DOScale(1f, 0.2f);
        //joystick.enabled = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ButtonImage.transform.DOScale(1.1f, 0.2f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ButtonImage.transform.DOScale(1.2f, 0.2f);
        PanelToggle();
    }

    private void PanelToggle()
    {
        if (PanelToOpen.activeSelf)
        {
            PanelToOpen.transform.DOScale(0f, 0.2f).SetEase(Ease.InBack).OnComplete(() => PanelToOpen.SetActive(false));
            joystick.enabled = true;
        }
        else
        {
            joystick.enabled = false;
            PanelToOpen.SetActive(true);
            PanelToOpen.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        }
    }
}
