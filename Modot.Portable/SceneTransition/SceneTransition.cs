using System;
using Godot;

namespace Modot.Portable;

public abstract class SceneTransition
{
    /// <summary>
    /// 加载场景的Func
    /// </summary>
    protected Func<PackedScene> sceneLoadAction;

    /// <summary>
    /// 当场景出现时可以触发的动作
    /// </summary>
    public Action OnSceneObscured;
    
    /// <summary>
    /// 当场景加载完成时触发的动作
    /// </summary>
    public Action OnTransitionCompleted;

    /// <summary>
    /// 加载的场景是否是新场景
    /// </summary>
    private bool _loadNewScene;

    protected SceneTransition(Func<PackedScene> sceneLoadAction)
    {
        this.sceneLoadAction = sceneLoadAction;
        _loadNewScene = sceneLoadAction != null;
    }

    public virtual void OnBeginTransition()
    {
        OnSceneObscured?.Invoke();
        var packScene = sceneLoadAction?.Invoke();
        Scene.Instance.GetTree().ChangeSceneToPacked(packScene);
    }
}