using System.Collections.Generic;

namespace Modot.Portable;

public static class EventBus<T> where T : IEvent{
    static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();
    public static void Subscribe(EventBinding<T> binding) => bindings.Add(binding);
    public static void UnSubscribe(EventBinding<T> binding) => bindings.Remove(binding);
    
    public static void Raise(T @event){
        foreach (var binding in bindings)
        {
            binding.OnEvent.Invoke(@event);
            binding.OnEventNoArgs.Invoke();
        }
    }
}