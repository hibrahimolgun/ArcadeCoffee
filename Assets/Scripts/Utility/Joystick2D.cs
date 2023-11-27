using System.Collections;
using UnityEngine;

public class Joystick3D : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    
    private Vector3 _initialClickPosition;
    private Vector3 _currentClickPosition;
    [SerializeField] private float _joystickSize; //figure out to make it scale with screen size

    [SerializeField] private GameObject _initialClickVisual;
    [SerializeField] private GameObject _currentClickVisual;
    [SerializeField] private RectTransform _initRect;
    [SerializeField] private RectTransform _currentRect;

    [Header("Double Tap")]
    private float lastTapTime;
    private float doubleTapTimeThreshold = 0.3f;
    [SerializeField] private ScriptableEvent _doubleTapEvent;

    private void Start()
    {
        _initialClickVisual.SetActive(false);
        _currentClickVisual.SetActive(false);
    }

    private void Update()
    {
        JoyStrength();
        JoyVisual();
        CheckIfDoubleTap();
    }

    private void JoyStrength()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _initialClickPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            _currentClickPosition = Input.mousePosition;
            _joystick.value = Vector3.ClampMagnitude(_currentClickPosition - _initialClickPosition, _joystickSize);
            _joystick.value.z = _joystick.value.y;
            _joystick.value.y = 0;
            _joystick.value /= _joystickSize; // normalize
        }
        if (Input.GetMouseButtonUp(0))
        {
            _joystick.value = Vector3.zero;
        }
    }

    private void JoyVisual()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _initRect.position = new Vector3(_initialClickPosition.x, _initialClickPosition.y, 0);
            _currentRect.position = _initRect.position;
            _initialClickVisual.SetActive(true); //TODO make visual delay for double tap
            _currentClickVisual.SetActive(true);
        }

        if (Input.GetMouseButton(0))
        {
            _currentRect.position = _initRect.position + Vector3.ClampMagnitude(_currentClickPosition - _initialClickPosition, _joystickSize);
            _currentRect.position = new Vector3(_currentRect.position.x, _currentRect.position.y, 0);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _initialClickVisual.SetActive(false);
            _currentClickVisual.SetActive(false);
        }
    }

    public void CheckIfDoubleTap()
    {
        if (Input.GetMouseButtonDown(0)) // Assuming left mouse button for simplicity
        {
            if (Time.time - lastTapTime < doubleTapTimeThreshold)
            {
                _doubleTapEvent.InvokeAction();
            }
            // Update the last tap time
            lastTapTime = Time.time;
        }
    }
}