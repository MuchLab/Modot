using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Modot.Portable;
public static class NodeUtils {
    /// <summary>
    /// 从parent节点上获取节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T FindNode<T>(Node parent, string name) where T : Node
    {
        T node = null;
        var child = parent.FindChild(name);
        if (child is T)
            node = child as T;
        Insist.IsNotNull(node, $"Can't find a node with type {typeof(T).Name}");
        return node;
    }

    /// <summary>
    /// 尝试获取节点
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="name"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public static bool TryFindNode<T>(Node parent, string name, out T node) where T : Node{
        node = parent.FindChild(name) as T;
        if (node != null)
            return true;
        return false;
    }

    /// <summary>
    /// 获取节点列表，深度为1
    /// </summary>
    /// <param name="name"></param>
    /// <param name="includeInternal"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> FindNodes<T>(Node parent, string name) where T : Node => parent.FindChildren(name).OfType<T>().ToList();

    /// <summary>
    /// 获取节点列表，无限深度
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static List<T> FindNodes<T>(Node parent) where T : Node => parent.FindChildren("*").OfType<T>().ToList();

    /// <summary>
    /// 判断节点是否有某个属性
    /// </summary>
    /// <param name="node"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    public static bool HasProperty(Node node, string property){
        var info = node.GetType().GetProperty(property);
        if(info == null){
            return false;
        }
        return true;
    }


    /// <summary>
    /// 节点延迟设置属性值
    /// </summary>
    /// <param name="node"></param>
    /// <param name="property"></param>
    /// <param name="args"></param>
    public static void SetDeferred(this Node node, string property, Variant value) {
        if(!HasProperty(node, property)){
            return;
        }
        node.SetDeferred(property, value);
    }

    /// <summary>
    /// 获取离source节点最近的节点
    /// </summary>
    /// <param name="source"></param>
    /// <param name="node2DList"></param>
    /// <returns></returns>
    public static Node2D GetNearestNode2D(Node2D source, List<Node2D> node2DList){
        Node2D target = null;
        float distanceMin = float.MaxValue;
        foreach (var node2D in node2DList)
        {
            var distance = source.GlobalPosition.DistanceSquaredTo(node2D.GlobalPosition);
            if(distance < distanceMin){
                distanceMin = distance;
                target = node2D;
            }
        }
        return target;
    }

    public static bool IsNodeInsideViewport(Node2D node, Vector2 size){
        var viewportSize = Scene.Instance.GetViewportRect().Size;
        return node.GlobalPosition.X > -(viewportSize.X / 2 + size.X / 2) &&
               node.GlobalPosition.X <  (viewportSize.X / 2 + size.X / 2) &&
               node.GlobalPosition.Y > -(viewportSize.Y / 2 + size.Y / 2) &&
               node.GlobalPosition.Y <  (viewportSize.Y / 2 + size.Y / 2);
    }
}