﻿namespace Modot.Portable.BehaviorTree;
/// <summary>
/// Wait a specified amount of time. The task will return running until the task is done waiting. It will return success after the wait
/// time has elapsed.
/// </summary>
public class WaitAction<T> : Behavior<T>
{
	/// <summary>
	/// the amount of time to wait
	/// </summary>
	public float WaitTime;

	float _startTime;


	public WaitAction(float waitTime)
	{
		WaitTime = waitTime;
	}


	public override void OnStart()
	{
		_startTime = 0;
	}


	public override TaskStatus Update(T context, float delta)
	{
		// we cant use Time.deltaTime due to the tree ticking at its own rate so we store the start time instead
		_startTime += delta;
		if (_startTime >= WaitTime)
			return TaskStatus.Success;
			

		return TaskStatus.Running;
	}
}