using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour, IDragHandler
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider.value = 0.3f;
        _audioSource.volume = slider.value;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _audioSource.volume = slider.value;
    }

}
