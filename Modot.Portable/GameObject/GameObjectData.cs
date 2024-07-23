using Godot;
using MiniExcelLibs.Attributes;
namespace Modot.Portable;

public class GameObjectData : Data{
    public string Name { get; set;}
    public string Type { get; set;}
    public string TextureDirName { get; set; }
    [ExcelIgnore]
    public PackedScene Prefab { get; set; }
}