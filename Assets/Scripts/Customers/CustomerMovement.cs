using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    [Header("CustomerBehaviour Link")]
    [SerializeField] private CustomerBehaviour1 _customerBehaviour;

    [Header("Movement")]
    [SerializeField] private PathScriptList _pathList;
    [SerializeField] private float _speed = 1f;

    //for path calculation
    private Vector3 _startPoint;
    private Vector3[] _path;

    public void GoToTable(int pathIndex)
    {
        _path = _pathList.pathScripts[pathIndex].GetPath();
        _startPoint = _pathList._startPoint;
        transform.position = _pathList._startPoint;
        StartWalk();
    }

    private void StartWalk()
    {
        var sequence = DOTween.Sequence();
        var point0 = _startPoint;
        for (int i = 0; i < _path.Length-1; i++) // until one before last
        {
            sequence.Append(transform.DOMove(_path[i], (_path[i] - point0).magnitude/_speed).SetEase(Ease.Linear));
            sequence.Join(transform.DOLookAt(_path[i], 0.5f).SetEase(Ease.Linear));
            point0 = _path[i];
        }
        sequence.Append(transform.DOJump(_path[^1], 1f, 1, 1f).SetEase(Ease.OutElastic));
        sequence.OnComplete(() => _customerBehaviour.ReadyToOrder());
    }

    public void Leave(float timeToFinish)
    {
        _path = _path.Reverse().ToArray();
        _path = _path.Append(_startPoint).ToArray();

        var sequence = DOTween.Sequence();
        var point0 = _path[0];
        for (int i = 0; i < _path.Length; i++)
        {
            sequence.Append(transform.DOMove(_path[i], (_path[i] - point0).magnitude/_speed).SetDelay(timeToFinish).SetEase(Ease.Linear));
            sequence.Join(transform.DOLookAt(_path[i], 0.5f).SetEase(Ease.Linear));
            point0 = _path[i];
            timeToFinish = 0;
        }
        //sequence.Append(transform.DOMove(_startPoint, (_startPoint - point0).magnitude/_speed).SetEase(Ease.Linear));
        sequence.AppendCallback(() => _customerBehaviour.DisableCallback());
    }
}
