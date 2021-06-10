using System;
using System.Collections.Generic;


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