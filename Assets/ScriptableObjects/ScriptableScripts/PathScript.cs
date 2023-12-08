using System;
using System.Collections.Generic;
using DG.Tweening.Plugins.Options;
using UnityEngine;

[CreateAssetMenu(fileName = "Path", menuName = "ScriptableObjects/Path", order = 1)]
public class PathScript : ScriptableObject
{
    public Points[] pathPoints;

    [Serializable]
    public struct Points
    {
        public Vector3 point;
    } 

    public Vector3[] GetPath()
    {
        var pointList = new Vector3[pathPoints.Length];
        for (var i = 0; i < pathPoints.Length; i++)
        {
            pointList[i] = pathPoints[i].point;
        }
        return pointList; 
    }


}
