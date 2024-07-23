using System;
using System.Collections.Generic;
namespace Modot.Portable;
public abstract partial class GameObject : Entity2D {
    private static uint newId = 1;
    public override void _EnterTree()
    {
        base._EnterTree();
        Id = newId;
        newId++;
    }
    public uint Id { get; set; }
    public Dictionary<StatType, GameObjectStat> Stats { get; private set; } = new Dictionary<StatType, GameObjectStat>();

    protected void InitializeStatFromData(GameObjectData data){
        foreach (var name in StatType.GetNames(typeof(StatType)))
        {
            var property = data.GetType().GetProperty(name);
            if(property != null){
                var value = property.GetValue(data);
                Stats.Add((StatType)Enum.Parse(typeof(StatType), name), new GameObjectStat(float.Parse(value.ToString())));
            }
        }
    }
    protected abstract void InitializeVisualFromData(GameObjectData data);
}
public enum StatType{
    Health,
    Damage,
    DamageMin,
    DamageMax,
    CritRate,
    CritTime,
    FireRangeRadius,
    FireDuration,
    FireSpeed,
    MoveSpeed,
    UnConditional
}