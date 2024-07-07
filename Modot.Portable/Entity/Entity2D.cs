using System;
using System.Collections.Generic;
using Godot;

namespace Modot.Portable;

public partial class Entity2D : Node2D, IEntity {

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

    #region Node2D Utils
    protected bool IsInsideViewport(Vector2 position, float offset){
        var size = GetViewport().GetVisibleRect().Size;
        if(position.X + offset < 0)
            return false;
        if(position.X - offset > size.X)
            return false;
        if(position.Y + offset < 0)
            return false;
        if(position.Y - offset > size.Y)
            return false;
        return true;
    }
    #endregion

    #region LifeTime Component
    // public override void _Process(double delta)
    // {
    //     base._Process(delta);
    //     foreach (var component in Components)
    //     {
    //         if(component.IsFirstTick){
    //             component.OnBegin();
    //             component.IsFirstTick = false;
    //         }
    //         else
    //             component.OnUpdate(delta);
    //     }
    // }

    // public override void _ExitTree()
    // {
    //     base._ExitTree();
    //     foreach (var component in Components)
    //         component.OnEnd();
    // }

    // protected void AddComponent(IComponent component){
    //     component.IsFirstTick = true;
    //     component.Entity = this;
    //     Components.Add(component);
    // }
    #endregion
}