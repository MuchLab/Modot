using System.Collections.Generic;
using Godot;

namespace Modot.Portable;

public partial class EntityUi : Control, IEntity {
    
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
    public uint Id { get; set; }

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

    public override void _Ready()
    {

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

    #region Node
    protected bool TryFindNode<T>(string nodeName, out T node) where T : Node{
        node = FindNode<T>(nodeName);
        if(node != null)
            return true;
        return false;
    }

    protected T FindNode<T>(string nodeName) where T : Node{
        var node = FindChild(nodeName) as T;
        Insist.IsNotNull(node != null, $"Can't find a node with {nodeName}");
        return node;
    }
    #endregion
}