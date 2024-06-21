namespace Modot.Portable;

public interface IEntity
{
    uint Id { get; set; }
    string Name { get; set; }
    bool TryAddTag(string tag);

    bool TryRemoveTag(string tag);

    bool HasTag(string tag);
    
}