using System;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using Modot.Portable;

public class ProgressSceneTransition : SceneTransition
{
    // public event Action OnDataLoad;
    // public event Action OnSceneLoad;
    // private string _loadDetail;
    private string _packedScenePath;
    private float _endingDelay;
    private float _timer;
    public Godot.Collections.Array Progress { get; private set; }
    public bool IsLoading { get; private set; }
    private PackedScene _progressPrefab;
    public ProgressSceneTransition(Func<string> sceneLoadAction, PackedScene progressPrefab, float endingDelay = 0) : base(sceneLoadAction)
    {
        _progressPrefab = progressPrefab;
        _endingDelay = endingDelay;
        _timer = 0;
        IsLoading = false;
        Progress = new Godot.Collections.Array();
    }
    public override void OnBeginTransition()
    {
        
        //先转换到进度条场景。
        Scene.Instance.UiStage.AddChild(_progressPrefab.Instantiate());
        //暂停所有场景的运行，需要把progressPrefab设置为Alway
        // Scene.Instance.ProcessMode = Node.ProcessModeEnum.Disabled;

        base.OnBeginTransition();
    }
    protected override void LoadAndChangeScene()
    {
        _packedScenePath = sceneLoadAction?.Invoke();
        ResourceLoader.LoadThreadedRequest(_packedScenePath);
    }

    public override void OnUpdateTransition(float delta)
    {
        base.OnUpdateTransition(delta);
        var sceneLoadStatus = ResourceLoader.LoadThreadedGetStatus(_packedScenePath, Progress);
        if(sceneLoadStatus == ResourceLoader.ThreadLoadStatus.Loaded){
            IsLoading = true;
            _timer += delta;
            if(_timer > _endingDelay)
            {
                Scene.Instance.GetTree().ChangeSceneToPacked(ResourceLoader.LoadThreadedGet(_packedScenePath) as PackedScene);
                _timer = 0;
                IsLoading = false;
            }
        }
        
    }
}