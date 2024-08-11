namespace Modot.Portable.BehaviorTree;
/// <summary>
/// will keep executing its child task until the child task returns success
/// </summary>
public class UntilSuccess<T> : Decorator<T>
{
	public override TaskStatus Update(T context, float delta)
	{
		Insist.IsNotNull(Child, "child must not be null");

		var status = Child.Tick(context, delta);

		if (status != TaskStatus.Success)
			return TaskStatus.Running;

		return TaskStatus.Success;
	}
}