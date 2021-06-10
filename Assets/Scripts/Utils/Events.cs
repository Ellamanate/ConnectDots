using System;
using System.Collections.Generic;
using UnityEngine.Events;


public static class Events
{
    public static EventAggregator<Dot[]> OnLineRelease = new EventAggregator<Dot[]>();
    public static EventAggregator<Dot> OnDeleteDot = new EventAggregator<Dot>();
    public static EventAggregator<ColorInfo> OnSquareRelease = new EventAggregator<ColorInfo>();
    public static EventAggregator<int> OnChangePoints = new EventAggregator<int>();
    public static readonly UnityEvent OnEndTurn = new UnityEvent();
    public static readonly UnityEvent OnGameOver = new UnityEvent();
    public static readonly UnityEvent OnExit = new UnityEvent();
    public static readonly UnityEvent OnRestart = new UnityEvent();
}

public class EventAggregator<T>
{
    private readonly List<Action<T>> _callbacks = new List<Action<T>>();

    public void Subscribe(Action<T> callback)
    {
        _callbacks.Add(callback);
    }

    public void Publish(T unit)
    {
        foreach (Action<T> callback in _callbacks)
            callback(unit);
    }

    public void UnSubscribe(Action<T> callback)
    {
        _callbacks.Remove(callback);
    }
}
