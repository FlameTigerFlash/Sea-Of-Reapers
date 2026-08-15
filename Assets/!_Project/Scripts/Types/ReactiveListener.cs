using System;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;

public class ReactiveListener<T> : IDisposable
{
    public event Action<T> ValueChangedEvent;

    public T Value
    {
        get => _value;
        private set
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

    private readonly ReactiveField<T> _field;

    public static implicit operator T(ReactiveListener<T> field)
    {
        return field.Value;
    }

    public ReactiveListener(ReactiveField<T> field)
    {
        _field = field;
        Value = field.Value;
        _field.ValueChangedEvent += OnValueChanged;
    }

    public ReactiveListener(ReactiveListener<T> listener)
    {
        _field = listener._field;
        Value = listener.Value;
        _field.ValueChangedEvent += (T newVal) => Value = newVal;
    }

    public void Dispose()
    {
        _field.ValueChangedEvent -= OnValueChanged;
    }

    private void OnValueChanged(T newVal)
    {
        Value = newVal;
    }
}
