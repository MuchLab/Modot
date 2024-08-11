namespace Modot.Portable.BehaviorTree;
/// <summary>
/// inverts the result of the child node
/// </summary>
public class Inverter<T> : Decorator<T>
{
	public override TaskStatus Update(T context, float delta)
	{
		Insist.IsNotNull(Child, "child must not be null");

		var status = Child.Tick(context, delta);

		if (status == TaskStatus.Success)
			return TaskStatus.Failure;

		if (status == TaskStatus.Failure)
			return TaskStatus.Success;

		return TaskStatus.Running;
	}
}