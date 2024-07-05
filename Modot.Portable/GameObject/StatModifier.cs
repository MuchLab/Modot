using System;

namespace Modot.Portable;

public class StatModifier : IComparable<StatModifier> {
    public readonly float Value;
    public readonly StatModifierType Type;
    public readonly int Order;
    public readonly object Source;

    public StatModifier(float value, StatModifierType type, int order, object source){
        Value = value;
        Type = type;
        Order = order;
        Source = source;
    }

    // Requires Value and Type. Calls the "Main" constructor and sets Order and Source to their default values: (int)type and null, respectively.
public StatModifier(float value, StatModifierType type) : this(value, type, (int)type, null) { }
 
// Requires Value, Type and Order. Sets Source to its default value: null
public StatModifier(float value, StatModifierType type, int order) : this(value, type, order, null) { }
 
// Requires Value, Type and Source. Sets Order to its default value: (int)Type
public StatModifier(float value, StatModifierType type, object source) : this(value, type, (int)type, source) { }

    public int CompareTo(StatModifier other)
    {
        if(Order < other.Order)
            return -1;
        else if(Order > other.Order)
            return 1;
        return 0;
    }
}

public enum StatModifierType{
    Flat = 100,
    PercentAdd = 200,
    PercentMult = 300,
}