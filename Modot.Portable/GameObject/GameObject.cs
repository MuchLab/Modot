using System;
using System.Linq;
using Godot;
namespace Modot.Portable;
public abstract partial class GameObject : Node2D {
    private static uint newId = 1;
    public override void _EnterTree()
    {
        base._EnterTree();
        Id = newId;
        newId++;
    }
    public uint Id { get; set; }

    protected abstract void InitializeStatFromData(GameObjectData data);
    protected abstract void InitializeVisualFromData(GameObjectData data);
    public void AddStatModifier(string statName, StatModifier modifier)
    {
        var stat = GetType().GetProperties().First(field => field.Name == statName).GetValue(this) as GameObjectStat;
        stat.AddModifier(modifier);
    }
    public void RemoveStatModifier(string statName, StatModifier modifier)
    {
        var stat = GetType().GetProperties().First(field => field.Name == statName).GetValue(this) as GameObjectStat;
        stat.RemoveModifier(modifier);
    }
}