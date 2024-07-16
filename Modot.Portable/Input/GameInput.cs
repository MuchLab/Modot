
using System;
using Godot;
using Modot.Portable;

public partial class GameInput : Entity{
    public static string MOUSE_LEFT_CLICK = "MouseLeftClick";
    public static string MOUSE_RIGHT_CLICK = "MouseRightClick";
    public static string MOUSE_WHEEL_UP = "MouseWheelUp";
    public static string MOUSE_WHEEL_DOWN = "MouseWheelDown";
    public static string TEST = "Test";
    public static string MENU_PANEL_SHOW = "MenuPanelShow";
    public static GameInput Instance { get; private set; }
    public bool IsScreenClicked => _isScreenClicked;
    private bool _isScreenClicked;
    public bool IsScreenTouched => _isScreenTouched;
    private bool _isScreenTouched;
    public bool IsLeftMouseClicked => Input.IsActionJustPressed(MOUSE_LEFT_CLICK);
    public bool IsLeftMousePressed => Input.IsActionPressed(MOUSE_LEFT_CLICK);
    public bool IsRightMouseClicked => Input.IsActionJustPressed(MOUSE_RIGHT_CLICK);
    public bool IsRightMousePressed => Input.IsActionPressed(MOUSE_RIGHT_CLICK);
    public bool IsMouseWheelUp => Input.IsActionJustPressed(MOUSE_WHEEL_UP);
    public bool IsMouseWheelDown => Input.IsActionJustPressed(MOUSE_WHEEL_DOWN);
    public bool IsMenuPanelShowed => Input.IsActionJustPressed(MENU_PANEL_SHOW);
    public bool IsTestPressed => Input.IsActionJustPressed(TEST);
    private static bool _isFirstTickTouched;
    public override void _Input(InputEvent e)
    {
        if(e is InputEventScreenTouch touchEvent){
            if(e.IsPressed()){
                _isScreenTouched = true;
                if(_isFirstTickTouched){
                    _isScreenClicked = true;
                    _isFirstTickTouched = false;
                }else{
                    _isScreenClicked = false;
                }
            }
            else{
                _isScreenTouched = false;
                _isScreenClicked = false;
                _isFirstTickTouched = true;
            }
        }
    }
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