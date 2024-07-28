namespace Modot.Portable;
public interface IGameProcess {
    void OnBegin();
    void OnUpdate(float delta);
    void OnEnd();
}