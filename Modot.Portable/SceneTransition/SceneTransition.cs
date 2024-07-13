using System;
using System.Threading.Tasks;
using Godot;

namespace Modot.Portable;

public abstract class SceneTransition
{
    /// <summary>
    /// 加载场景的Func
    /// </summary>
    protected Func<string> sceneLoadAction;

    /// <summary>
    /// 当加载场景时需要执行的操作
    /// </summary>
    public Action OnSceneObscured;
    
    /// <summary>
    /// 当场景加载完成时需要执行的操作
    /// </summary>
    public Action OnTransitionCompleted;

    /// <summary>
    /// 加载的场景是否是新场景
    /// </summary>
    private bool _loadNewScene;
    public bool IsFirstTick;

    protected SceneTransition(Func<string> sceneLoadAction)
    {
        this.sceneLoadAction = sceneLoadAction;
        _loadNewScene = sceneLoadAction != null;
        IsFirstTick = true;
    }

    public virtual void OnBeginTransition()
    {
        IsFirstTick = false;
        for (int i = 0; i < Scene.GlobalManagers.Count; i++)
        {
            var globalManager = Scene.GlobalManagers[i];
            if(globalManager.EraseWhenTransition)
            {
                if(globalManager is GameProcessHandler)
                    GameProcessHandler.ClearProcessQueue();
                Scene.UnregisterGlobalManager(globalManager);
                i--;
            }
        }
        OnSceneObscured?.Invoke();
        LoadAndChangeScene();
    }

    public virtual void OnUpdateTransition(float delta){}

    protected virtual void LoadAndChangeScene(){
        var packedScenePath = sceneLoadAction?.Invoke();
        var packedScene = ResourceLoader.Load<PackedScene>(packedScenePath);
        Scene.Instance.GetTree().ChangeSceneToPacked(packedScene);
    }
}