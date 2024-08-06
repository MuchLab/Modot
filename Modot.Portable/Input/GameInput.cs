
using Godot;

public partial class GameInput : Node{
    public static string LEFT_MOUSE_BUTTON = "LeftMouseButton";
    public static string RIGHT_MOUSE_BUTTON = "RightMouseButton";
    public static string MOUSE_WHEEL_UP = "MouseWheelUp";
    public static string MOUSE_WHEEL_DOWN = "MouseWheelDown";
    public static string TEST = "Test";
    public static string ESCAPE_KEY = "EscapeKey";
    public static GameInput Instance { get; private set; }
    public bool IsScreenClicked => _isScreenClicked;
    private bool _isScreenClicked;
    public bool IsScreenTouched => _isScreenTouched;
    private bool _isScreenTouched;
    public bool IsLeftMouseJustPressed => Input.IsActionJustPressed(LEFT_MOUSE_BUTTON);
    public bool IsLeftMousePressed => Input.IsActionPressed(LEFT_MOUSE_BUTTON);
    public bool IsRightMouseJustPressed => Input.IsActionJustPressed(RIGHT_MOUSE_BUTTON);
    public bool IsRightMousePressed => Input.IsActionPressed(RIGHT_MOUSE_BUTTON);
    public bool IsMouseWheelUp => Input.IsActionJustPressed(MOUSE_WHEEL_UP);
    public bool IsMouseWheelDown => Input.IsActionJustPressed(MOUSE_WHEEL_DOWN);
    public bool IsEscapeKeyJustPressed => Input.IsActionJustPressed(ESCAPE_KEY);
    public bool IsTestJustPressed => Input.IsActionJustPressed(TEST);
    public override void _EnterTree()
    {
        base._EnterTree();
        if(Instance == null)
            Instance = this;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        if(Instance != null)
            Instance = null;
    }
}