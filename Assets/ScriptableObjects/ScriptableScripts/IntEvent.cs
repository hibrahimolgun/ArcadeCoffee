using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Int Event", menuName = "ScriptableObjects/New Int Event", order = 0)]
public class IntEvent : ScriptableObject
{
    private Action<int> action;

    public void InvokeAction(int invokedInt)
    {
        action?.Invoke(invokedInt);
    }
        
    public void Subscribe(Action<int> action)
    {
        this.action += action;
    }
        
    public void Unsubscribe(Action<int> action)
    {
        this.action -= action;
    }
}
