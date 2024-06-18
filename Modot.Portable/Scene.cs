using Godot;

namespace Modot.Portable;

public partial class Scene : Node
{
    public static Scene Instance { get; private set; }
    private static SceneTransition _sceneTransition;

    private List<GlobalManager> _globalManagers = new List<GlobalManager>();

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
        Instance._globalManagers.Add(manager);
        manager.Enabled = true;
    }
    
    /// <summary>
    /// 注销全局管理器
    /// </summary>
    /// <param name="globalManager"></param>
    public static void UnregisterGlobalManager(GlobalManager manager)
    {
        Instance._globalManagers.Remove(manager);
        manager.Enabled = false;
    }
    
    /// <summary>
    /// 按类型获取一个全局管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetGlobalManager<T>() where T : GlobalManager =>
        Instance._globalManagers.First((manager) => manager is T) as T;

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
        
    }
    
    public override void _Process(double delta)
    {
        foreach (var globalManager in _globalManagers)
        {
            if(globalManager.Enabled)
                globalManager.Update();
        }
        _sceneTransition?.OnBeginTransition();
    }

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