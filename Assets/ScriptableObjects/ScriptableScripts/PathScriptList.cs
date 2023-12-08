using UnityEngine;

[CreateAssetMenu(fileName = "PathScriptList", menuName = "ScriptableObjects/PathScriptList", order = 1)]
public class PathScriptList : ScriptableObject
{
    public Vector3 _startPoint;
    public PathScript[] pathScripts;
    public Vector3[] _tablePoints;
}
