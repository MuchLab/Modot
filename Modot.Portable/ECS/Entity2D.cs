using Godot;

namespace Modot.Portable;

public partial class Entity2D : Node2D {
    
    /// <summary>
    /// 拥有该实体的场景
    /// </summary>
    public Scene Scene { get; private set; }
    /// <summary>
    /// 实体的名称
    /// </summary>
    public string Name { get; set; }
}