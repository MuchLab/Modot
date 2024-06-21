using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Godot;

namespace Modot.Portable;

public partial class Scene : Node
{
    public static Scene Instance { get; private set; }

    private static List<GlobalManager> GlobalManagers = new List<GlobalManager>();
    private static SceneTransition _sceneTransition;

    public Vector2 SCALER = new Vector2(5f, 5f);

    #region SceneTransition

    /// <summary>
    /// 开启一个场景转换
    /// </summary>
    /// <param name="sceneTransition"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T StartSceneTransition<T>(T sceneTransition) where T : SceneTransition
    {
        //当上一个场景转换未结束时，不得开启下一个场景转换
        Insist.IsNull(_sceneTransition, "SceneTransition should be null before start scene transition.");
        _sceneTransition = sceneTransition;
        return sceneTransition;
    }

    #endregion
    
    #region GlobalManager

    /// <summary>
    /// 注册全局管理器
    /// </summary>
    /// <param name="globalManager"></param>
    public static void RegisterGlobalManager(GlobalManager manager)
    {
        GlobalManagers.Add(manager);
        manager.Enabled = true;
    }
    
    /// <summary>
    /// 注销全局管理器
    /// </summary>
    /// <param name="globalManager"></param>
    public static void UnregisterGlobalManager(GlobalManager manager)
    {
        GlobalManagers.Remove(manager);
        manager.Enabled = false;
    }
    
    /// <summary>
    /// 按类型获取一个全局管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetGlobalManager<T>() where T : GlobalManager =>
        GlobalManagers.First((manager) => manager is T) as T;

    #endregion

    #region SceneHolder LifeTime
    
    /// <summary>
    /// 场景加载后的操作
    /// </summary>
    public override void _EnterTree()
    {
        if (_sceneTransition != null)
        {
            _sceneTransition.OnTransitionCompleted?.Invoke();
            _sceneTransition = null;
        }
        Instance = this;
    }
    
    /// <summary>
    /// 场景初始化操作
    /// </summary>
    public override void _Ready()
    {
        var entity2DGlobalManager = new Entity2DGlobalManager(FindNodes<Entity2D>());
        RegisterGlobalManager(entity2DGlobalManager);
    }
    
    public override void _Process(double delta)
    {
        foreach (var globalManager in GlobalManagers)
        {
            if(globalManager.Enabled)
                globalManager.Update();
        }
        _sceneTransition?.OnBeginTransition();
    }

    /// <summary>
    /// 退出游戏需要做的操作
    /// </summary>
    public void ExitGame()
    {
        GetTree().Quit();
    }

    #endregion

    #region Node
    
    /// <summary>
    /// 从根节点开始获取节点
    /// </summary>
    /// <param name="name"></param>
    /// <param name="includeInternal">是否会查找除子节点外的其他节点</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T FindNode<T>(string name) where T : Node
    {
        T node = null;
        var child = FindChild(name);
        if (child is T)
            node = child as T;
        Insist.IsNotNull(node, $"Can't find a node with type {typeof(T).Name}");
        return node;
    }
    
    /// <summary>
    /// 从一个节点开始获取节点
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <param name="includeInternal">是否会查找除子节点外的其他节点</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T FindNode<T>(Node parent, string name) where T : Node
    {
        T node = null;
        var child = parent.FindChild(name);
        if (child is T)
            node = child as T;
        Insist.IsNotNull(node, $"Can't find a node with type {typeof(T).Name}");
        return node;
    }

    public bool TryFindNode<T>(string name, out T node) where T : Node{
        node = FindChild(name) as T;
        if (node != null)
            return true;
        return false;

    }

    /// <summary>
    /// 获取节点列表
    /// </summary>
    /// <param name="name"></param>
    /// <param name="includeInternal"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List<T> FindNodes<T>(string name) where T : Node => FindChildren(name).OfType<T>().ToList();
    public List<T> FindNodes<T>() where T : Node => FindChildren("*").OfType<T>().ToList();

    
    /// <summary>
    /// 获取节点列表
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <param name="includeInternal"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List<T> GetNodes<T>(Node parent, string name) where T : Node => parent.FindChildren(name).OfType<T>().ToList();

    #endregion
    
    #region Input

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (keyEvent.Keycode == Key.T)
            {
                StartSceneTransition(new DefaultSceneTransition(() => ResourceLoader.Load<PackedScene>("res://Content/Scenes/test1.tscn")));
                
            }
            if (keyEvent.Keycode == Key.Y)
            {
                StartSceneTransition(new DefaultSceneTransition(() => ResourceLoader.Load<PackedScene>("res://Content/Scenes/test2.tscn")));
            }
        }
    }

    #endregion
}