using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;


namespace Modot.Portable;
/// <summary>
/// Simple state machine with an enum constraint. There are some rules you must follow when using this:
/// - before update is called initialState must be set (use the constructor or onAddedToEntity)
/// - if you implement update in your subclass you must call base.update()
/// 
/// Note: if you use an enum as the contraint you can avoid allocations/boxing in Mono by doing what the Core
/// Emitter does for its enum: pass in a IEqualityComparer to the constructor.
/// </summary>
public abstract partial class SimpleStateMachine<TEnum> : Node
	where TEnum : struct, IComparable, IFormattable
{
	class StateMethodCache
	{
		public Action EnterState;
		public Action<double> Tick;
		public Action ExitState;
	}

	protected float elapsedTimeInState = 0f;
	protected TEnum previousState;
	Dictionary<TEnum, StateMethodCache> _stateCache;
	StateMethodCache _stateMethods;

	TEnum _currentState;

	public TEnum CurrentState
	{
		get => _currentState;
		set
		{
			// dont change to the current state
			if (_stateCache.Comparer.Equals(_currentState, value))
				return;

			// swap previous/current
			previousState = _currentState;
			_currentState = value;

			// exit the state, fetch the next cached state methods then enter that state
			if (_stateMethods.ExitState != null)
				_stateMethods.ExitState();

			elapsedTimeInState = 0f;
			_stateMethods = _stateCache[_currentState];

			if (_stateMethods.EnterState != null)
				_stateMethods.EnterState();
		}
	}

	protected TEnum InitialState
	{
		set
		{
			_currentState = value;
			_stateMethods = _stateCache[_currentState];

			if (_stateMethods.EnterState != null)
				_stateMethods.EnterState();
		}
	}


	public SimpleStateMachine(IEqualityComparer<TEnum> customComparer = null)
	{
		_stateCache = new Dictionary<TEnum, StateMethodCache>(customComparer);

		// cache all of our state methods
		var enumValues = (TEnum[]) Enum.GetValues(typeof(TEnum));
		foreach (var e in enumValues)
			ConfigureAndCacheState(e);
	}

	public override void _Process(double delta)
	{
		elapsedTimeInState += (float)delta;

		if (_stateMethods.Tick != null)
			_stateMethods.Tick(delta);
	}

	void ConfigureAndCacheState(TEnum stateEnum)
	{
		var stateName = stateEnum.ToString();

		var state = new StateMethodCache();
		state.EnterState = GetActionDelegateForMethod(stateName + "_Enter");
		state.Tick = GetActionWithArgDelegateForMethod(stateName + "_Tick");
		state.ExitState = GetActionDelegateForMethod(stateName + "_Exit");

		_stateCache[stateEnum] = state;
	}

	Action GetActionDelegateForMethod(string methodName)
	{
		var methodInfo = GetMethodInfo(GetType(), methodName);
		if (methodInfo != null)
			return CreateDelegate<Action>(this, methodInfo);

		return null;
	}

	Action<double> GetActionWithArgDelegateForMethod(string methodName)
	{
		var methodInfo = GetMethodInfo(GetType(), methodName);
		if (methodInfo != null)
			return CreateDelegate<Action<double>>(this, methodInfo);

		return null;
	}

	public MethodInfo GetMethodInfo(Type type, string methodName, Type[] parameters = null)
	{
		if (parameters == null)
			return type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		return type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, Type.DefaultBinder, parameters, null);
	}

	public T CreateDelegate<T>(object targetObject, MethodInfo methodInfo) =>
		(T)(object)Delegate.CreateDelegate(typeof(T), targetObject, methodInfo);
}
