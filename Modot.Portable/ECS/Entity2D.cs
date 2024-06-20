using System;
using System.Collections.Generic;
using Godot;

namespace Modot.Portable;

public partial class Entity2D : Node2D {
    
    /// <summary>
    /// 拥有该实体的场景
    /// </summary>
    public Scene Scene { get; private set; }

    /// <summary>
    /// 实体的名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 实体的唯一标识
    /// </summary>
    public uint Id { get; private set; }

    List<string> Tags = new List<string>();

    private static uint newId = 0;

    private bool _enabled;
    public bool  Enabled
    {
        get => _enabled;
        set => SetEnabled(value);
    }
    

    public override void _EnterTree()
    {
        //TODO: Id应该从配置文件中读取
        Id = newId;
        newId++;
    }

    private void SetEnabled(bool enabled){

    }

    public bool TryAddTag(string tag){
        var contain = HasTag(tag);
        Insist.IsFalse(contain, $"Entity {Name} can't add tag {tag}");
        if(!contain)
            Tags.Add(tag);
        return !contain;
    }

    public bool TryRemoveTag(string tag){
        var contain = HasTag(tag);
        Insist.IsTrue(contain, $"Entity {Name} can't add tag {tag}");
        if(contain)
            Tags.Remove(tag);
        return contain;
    }

    public bool HasTag(string tag) => Tags.Contains(tag);
}