
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    
    void Update()
    {
        if (_camera != null) transform.LookAt(_camera.transform);
    }
}
