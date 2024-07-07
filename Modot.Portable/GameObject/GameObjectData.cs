using System;
using Godot;
using MiniExcelLibs.Attributes;

public class GameObjectData{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string TextureDirName { get; set; }
    [ExcelIgnore]
    public PackedScene Prefab { get; set; }
}