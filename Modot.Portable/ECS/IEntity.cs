namespace Modot.Portable;

public interface IEntity
{
    bool TryAddTag(string tag);

    bool TryRemoveTag(string tag);

    bool HasTag(string tag);
    
}