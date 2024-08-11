namespace Modot.Portable.BehaviorTree;
/// <summary>
/// will keep executing its child task until the child task returns failure
/// </summary>
public class UntilFail<T> : Decorator<T>
{
public override TaskStatus Update(T context, float delta)
{
	Insist.IsNotNull(Child, "child must not be null");

	var status = Child.Update(context, delta);

	if (status != TaskStatus.Failure)
		return TaskStatus.Running;

	return TaskStatus.Success;
}
}