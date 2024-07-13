using System;
using Godot;

namespace Modot.Portable;

/// <summary>
/// 默认的场景转换，自定义的SceneTransition可以加载一些效果
/// </summary>
public class DefaultSceneTransition(Func<string> sceneLoadAction) : SceneTransition(sceneLoadAction);