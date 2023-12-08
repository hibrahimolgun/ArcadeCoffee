using UnityEngine;

[CreateAssetMenu(fileName = "RangedFloat", menuName = "ScriptableObjects/RangedFloat", order = 1)]
public class SORangedFloat : ScriptableObject
{
    public float min;
    public float max;
    [Range(0.5f, 1f)] public float value;
}
