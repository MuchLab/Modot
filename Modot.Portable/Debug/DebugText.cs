using System.Collections;
using System.Collections.Generic;
using Godot;

namespace Modot.Portable;
public partial class DebugText : Control {
    private struct DebugLine{
        public string Text { get; set; }
        public Color Color { get; set; }
    }
    private int _lineNumberMax = 8;
    private int _marginButtom = 5;
    private int _fontSize = 8;
    private Queue<DebugLine> _lines = new Queue<DebugLine>();

    private float _space = 2;
    private float _beginPositionY;
    private Timer _timer;

    private float _showTime = 5f;

    public override void _Ready()
    {
        base._Ready();
        _beginPositionY = GetViewportRect().Size.Y;
        _timer = new Timer();
        AddChild(_timer);
        _timer.OneShot = true;
        _timer.Timeout += Timer_Timeout;
    }
    public override void _Draw()
    {
        int index = 0;
        foreach (var line in _lines)
        {
            var position = new Vector2( 0,  _beginPositionY - _marginButtom - index * (_fontSize + _space));

            DrawString(ThemeDB.FallbackFont, position, line.Text, HorizontalAlignment.Left, 480, _fontSize, line.Color);
            index++;
        }
    }
    private void Timer_Timeout(){
        var tween = CreateTween();
        tween.TweenProperty(this, "modulate:a", 0, 0.5).SetEase(Tween.EaseType.OutIn);
        tween.Finished += () =>{
            Hide();
            Modulate = Colors.White;
        };
    }
    public void DrawLog(string text, Color color){
        if(_lines.Count > _lineNumberMax - 1){
            _lines.Dequeue();
        }
        _lines.Enqueue(new DebugLine(){ Text = text, Color = color });
        _timer.Start(_showTime);
        Show();
        QueueRedraw();
    }

    public void DrawLog(string text, LogType type){
        var color = GetColorByType(type);
        DrawLog(text, color);
    }

    public void DrawLog(string text){
        if(_lines.Count > _lineNumberMax - 1){
            _lines.Dequeue();
        }
        _lines.Enqueue(new DebugLine(){ Text = text, Color = Colors.White });
        _timer.Start(_showTime);
        Show();
        QueueRedraw();
    }
   
   public void DrawInfo(string text) => DrawLog(text, LogType.Info);
   public void DrawError(string text) => DrawLog(text, LogType.Error);
   public void DrawWarn(string text) => DrawLog(text, LogType.Warn);
   public void DrawTrace(string text) => DrawLog(text, LogType.Trace);

    private Color GetColorByType(LogType type){
        Color color = Colors.White;
        switch(type){
            case LogType.Info:
                color = Colors.Green;
                break;
            case LogType.Error:
                color = Colors.Red;
                break;
            case LogType.Warn:
                color = Colors.Yellow;
                break;
            case LogType.Trace:
                color = Colors.Blue;
                break;
        }
        return color;
    }
}

public enum LogType
{
    Error,
    Warn,
    Log,
    Info,
    Trace
}