using System;
using System.Collections.Generic;
using System.Linq;
using Arch.Core;
using Arch.Core.Utils;
using Arch.System;
using Godot;

namespace Modot.Portable;

public partial class Scene : Node2D
{
    public string DrawText { get; set; }
    public static Scene Instance { get; private set; }
    public static List<GlobalManager> GlobalManagers { get; private set; } = new List<GlobalManager>();
    public static SceneTransition SceneTransition;
    public Camera2D MainCamera => _mainCamera;
    private Camera2D _mainCamera;
    public Control UiStage => _uiStage;
    private Control _uiStage;
    public DebugText DebugText => _debugText;
    private DebugText _debugText;
    public static RootController RootController = new RootController();


    

    #region SceneTransition

    /// <summary>
    /// 开启一个场景转换
    /// </summary>
    /// <param name="sceneTransition"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public void StartSceneTransition<T>(T sceneTransition) where T : SceneTransition
    {
        //当上一个场景转换未结束时，不得开启下一个场景转换
        if(SceneTransition == null)
            SceneTransition = sceneTransition;
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

    public static void UnregisterGlobalManager<T>() where T : GlobalManager{
        var manager = GetGlobalManager<T>();
        if(manager != null)
            UnregisterGlobalManager(manager);
    }
    
    /// <summary>
    /// 按类型获取一个全局管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetGlobalManager<T>() where T : GlobalManager =>
        GlobalManagers.FirstOrDefault(manager => manager is T) as T;

    public static bool HasGlobalManager<T>() where T : GlobalManager =>
        GlobalManagers.Any(manager => manager is T);
    
    public static bool TryGetGlobalManager<T>(out T globalManager) where T : GlobalManager {
        globalManager = GetGlobalManager<T>();
        if(globalManager != null)
            return true;
        return false;
    }

    #endregion

    #region SceneHolder LifeTime
    
    /// <summary>
    /// 场景加载后的操作
    /// </summary>
    public override void _EnterTree()
    {
        if (SceneTransition != null)
        {
            SceneTransition.OnTransitionCompleted?.Invoke();
            SceneTransition = null;
        }
        Instance = this;
        if(TryFindNode("Camera2D", out _mainCamera)){
            Debug.Log("MainCamera attach successed");
        } else {
            _mainCamera = new Camera2D();
            AddChild(_mainCamera);
            Debug.Log("Create MainCamera");
        }
        if(TryFindNode("Ui", out _uiStage)){
            Debug.Log("UiStage attach successed");
            _debugText = new DebugText();
            _uiStage.AddChild(_debugText);
        }else{
            _uiStage = null;
        }

        //Arch World
        _world = World.Create();

    }

    /// <summary>
    /// 场景初始化操作
    /// </summary>
    public override void _Ready(){
        Initialized();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Initialized(){}
    
    public override void _Process(double delta)
    {
        foreach (var globalManager in GlobalManagers)
        {
            if(globalManager.Enabled)
                globalManager.Update((float)delta);
        }
        if(SceneTransition != null){
            if(SceneTransition.IsFirstTick)
                SceneTransition.OnBeginTransition();
            else
                SceneTransition.OnUpdateTransition((float)delta);
        }
        foreach (var group in _systemGroups)
        {
            group.BeforeUpdate((float) delta);
            group.Update((float)delta);
            group.AfterUpdate((float)delta);
        }
        base._Process(delta);
    }

    /// <summary>
    /// 退出游戏需要做的操作
    /// </summary>
    public void ExitGame()
    {
        World.Destroy(_world);
        foreach (var group in _systemGroups)
        {
            group.Dispose();
        }
        GetTree().Quit();
    }

    #endregion

    #region Node
    public T FindNode<T>(string name) where T : Node => NodeUtils.FindNode<T>(Instance, name);
    public bool TryFindNode<T>(string name, out T node) where T : Node => NodeUtils.TryFindNode(Instance, name, out node);
    public List<T> FindNodes<T>(string name) where T : Node => NodeUtils.FindNodes<T>(Instance, name);
    public List<T> FindNodes<T>() where T : Node => NodeUtils.FindNodes<T>(Instance);

    #endregion

    #region ArchECS
    public World World => _world;
    private World _world;
    private List<Arch.System.Group<float>> _systemGroups = [new ("default")];
    public void AddSystemGroup(string name) => _systemGroups.Add(new (name));
    public bool AddSystemByGroupIndex<G>(int index) where G : class, ISystem<float>{
        if(index > _systemGroups.Count - 1)
            return false;
        var system = Activator.CreateInstance(typeof(G), [_world]) as G;
        Insist.IsNotNull($"System {typeof(G).Name} must have a constructor with a world args");
        _systemGroups[index].Add(system);
        system.Initialize();
        return true;
    }
    public bool AddSystem<G>() where G : class, ISystem<float>{
        var system = Activator.CreateInstance(typeof(G), [_world]) as G;
        Insist.IsNotNull($"System {typeof(G).Name} must have a constructor with a world args");
        _systemGroups[0].Add(system);
        system.Initialize();
        return true;
    }
    public bool GetSystemByGroupIndex<G>(int index, out G system) where G : class, ISystem<float>{
        if(index > _systemGroups.Count - 1){
            system = null;
            return false;
        }
        system = _systemGroups[index].Get<G>();
        return true;
    }
    public G GetSystem<G>() where G : ISystem<float> => _systemGroups[0].Get<G>();
    public Entity CreateEntity(params ComponentType[] types) => _world.Create(types);
    public Entity CreateEntity<E>() => _world.Create<E>();
    #endregion
}