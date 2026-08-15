using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReactiveField<T>
{
    public event Action<T> ValueChangedEvent;

    public T Value
    {
        get => _value;
        set
        {
            T temp = _value;
            _value = value;
            if (!EqualityComparer<T>.Default.Equals(_value, temp))
            {
                ValueChangedEvent?.Invoke(_value);
            }
        }
    }

    private T _value;

    public static implicit operator ReactiveListener<T>(ReactiveField<T> field)
    {
        return new ReactiveListener<T>(field);
    }

    public static implicit operator T(ReactiveField<T> field)
    {
        return field.Value;
    }

    public ReactiveField()
    {
        Value = default(T);
    }

    public ReactiveField(T value)
    {
        Value = value;
    }
}
