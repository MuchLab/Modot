using System.Collections.Generic;
namespace Modot.Portable;
public class GameProcessHandler : GlobalManager {
    private static Queue<IGameProcess> ProcessQueue = new Queue<IGameProcess>();
    private static bool IsPlayingProcess;
    public static IGameProcess CurrentProcess;

    public GameProcessHandler() : base() {
        IsPlayingProcess = false;
        CurrentProcess = null;
    }

    public override void Update(float delta)
    {
        if(IsPlayingProcess){
            CurrentProcess.OnUpdate(delta);
        }
    }

    public GameProcessHandler AddProcessToQueue(IGameProcess process){
        ProcessQueue.Enqueue(process);
        if (!IsPlayingProcess){
            PlayFirstProcess();
        }

        return this;
    }

    public static void PlayFirstProcess(){
        IsPlayingProcess = true;
        if(ProcessQueue.Count > 0){
            CurrentProcess = ProcessQueue.Dequeue();
            Debug.Log($"{CurrentProcess.GetType().Name} Begin");
            CurrentProcess.OnBegin();
        }
    }

    public static void EndPlayingProcess(){
        Debug.Log($"{CurrentProcess.GetType().Name} End");
        CurrentProcess.OnEnd();
        if(ProcessQueue.Count > 0)
            PlayFirstProcess();
        else
            IsPlayingProcess = false;
    }

    public static void ClearProcessQueue(){
        ProcessQueue.Clear();
        CurrentProcess = null;
        IsPlayingProcess = false;
    }
}