namespace Modot.Portable.BehaviorTree;
/// <summary>
/// runs an entire BehaviorTree as a child and returns success
/// </summary>
public class BehaviorTreeReference<T> : Behavior<T>
{
	BehaviorTree<T> _childTree;


	public BehaviorTreeReference(BehaviorTree<T> tree)
	{
		_childTree = tree;
	}
	
	public override TaskStatus Update(T context, float delta)
	{
		_childTree._Process(delta);
		return TaskStatus.Success;
	}
}