namespace Modot.Portable;

public class GlobalManager
{
    /// <summary>
    /// 当true时，GlobalManager可用，否则不可用
    /// </summary>
    /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
    public bool Enabled
    {
        get => _enabled;
        set => SetEnabled(value);
    }

    /// <summary>
    /// 可用/不可用 GlobalManager
    /// </summary>
    /// <returns>The enabled.</returns>
    /// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
    public void SetEnabled(bool isEnabled)
    {
        if (_enabled != isEnabled)
        {
            _enabled = isEnabled;

            if (_enabled)
                OnEnabled();
            else
                OnDisabled();
        }
    }

    bool _enabled;

    /// <summary>
    /// 当GlobalManager可用时调用
    /// </summary>
    public virtual void OnEnabled()
    {
    }

    /// <summary>
    /// 当GlobalManager不可用时调用
    /// </summary>
    public virtual void OnDisabled()
    {
    }

    /// <summary>
    /// 在Scene.update之前调用
    /// </summary>
    public virtual void Update(float delta)
    {
    }
}