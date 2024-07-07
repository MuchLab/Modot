using System;
using System.Collections.Generic;
using Godot;

namespace Modot.Portable;

public partial class Entity : Node, IEntity {

    List<string> Tags = new List<string>();

    private bool _enabled;
    public bool  Enabled
    {
        get => _enabled;
        set => SetEnabled(value);
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