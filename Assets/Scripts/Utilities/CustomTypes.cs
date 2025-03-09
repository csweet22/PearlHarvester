using System;
using System.Collections.Generic;

public class Trackable<T>
{
    public event Action<T, T> OnValueSet;
    public event Action<T, T> OnValueChanged;

    private T value; 
    public T Value
    {
        get { return this.value; }
        set
        {
            T oldValue = this.value;
            T newValue = value;
            
            this.value = value;
            
            if (!EqualityComparer<T>.Default.Equals(oldValue, newValue))
                OnValueChanged?.Invoke(oldValue, newValue);
            
            OnValueSet?.Invoke(oldValue, newValue);
        } 
    }

    public Trackable(T initialValue)
    {
        this.value = initialValue;
    }
}
