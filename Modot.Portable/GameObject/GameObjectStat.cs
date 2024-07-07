using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Modot.Portable;


public class GameObjectStat {
    private float _lastBaseValue = float.MinValue;
    public float BaseValue { get; set; }
    private bool _isDirty = true;
    private float _value;
    public float Value { 
        get {
            if(_isDirty || _lastBaseValue != BaseValue){
                _lastBaseValue = BaseValue;
                _value = CalculateFinalValue();
                _isDirty = false;
            }
            return _value;
        } 
    }
    private readonly List<StatModifier> _statModifiers;

    public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    public GameObjectStat()
    {
        _statModifiers = new List<StatModifier>();
        StatModifiers = _statModifiers.AsReadOnly();
    }

    public GameObjectStat(float baseValue) : this(){
        BaseValue = baseValue;
    }

    public virtual void AddModifier(StatModifier modifier){
        _isDirty = true;
        _statModifiers.Add(modifier);
        _statModifiers.Sort();
    }
    public virtual bool RemoveModifier(StatModifier modifier){
        if(_statModifiers.Remove(modifier)){
            _isDirty = true;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public virtual bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;
    
        for (int i = _statModifiers.Count - 1; i >= 0; i--)
        {
            if (_statModifiers[i].Source == source)
            {
                _isDirty = true;
                didRemove = true;
                _statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }

    /// <summary>
    /// 计算最终的数值
    /// </summary>
    /// <returns></returns>
    protected virtual float CalculateFinalValue(){
        float finalValue = BaseValue;
        float sumPercent = 0;
        for (int i = 0; i < _statModifiers.Count; i++){
            StatModifier mod = _statModifiers[i];
    
            if (mod.Type == StatModifierType.FlatAdd){
                finalValue += mod.Value;
            }
            else if(mod.Type == StatModifierType.FlatReduce){
                finalValue -= mod.Value;
            }
            else if (mod.Type == StatModifierType.PercentMult){
                finalValue *= 1 + mod.Value;
            }
            else{
                if(mod.Type == StatModifierType.PercentAdd)
                    sumPercent += mod.Value;
                if(mod.Type == StatModifierType.PercentReduce)
                    sumPercent -= mod.Value;
                if (i + 1 >= _statModifiers.Count || _statModifiers[i + 1].Type != StatModifierType.PercentAdd){
                    finalValue *= 1 + sumPercent;
                    sumPercent = 0;
                }
            }
        }
        return (float)Math.Round(finalValue, 4);
    }
}