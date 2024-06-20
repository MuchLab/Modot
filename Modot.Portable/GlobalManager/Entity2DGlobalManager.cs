using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Modot.Portable;

class Entity2DGlobalManager : GlobalManager
{
    public List<Entity2D> entity2Ds;

    public Entity2DGlobalManager(){
        entity2Ds = new List<Entity2D>();
    }
    public Entity2DGlobalManager(List<Entity2D> entity2Ds){
        this.entity2Ds = entity2Ds;
    }

    public Entity2D GetEntity2DById(uint id){
        var entity = entity2Ds.FirstOrDefault(entity => entity.Id == id);
        Insist.IsNotNull(entity, $"Can't find a entity with {id}.");
        return entity;
    }

    public bool TryGetEntity2DById(uint id, out Entity2D entity2D){
        entity2D = entity2Ds.FirstOrDefault(entity => entity.Id == id);
        return entity2D != null;
    }

    public bool TryRemoveEntity2DById(uint id){
        var removedEntity = entity2Ds.Where(entity => entity.Id == id).ToList();
        var canRemoved = removedEntity.Count() == 1;
        Insist.IsTrue(canRemoved, $"Remove entity with {id} more than 1.");
        if(canRemoved)
        {
            entity2Ds.Remove(removedEntity[0]);
            removedEntity[0].QueueFree();
        }
        return canRemoved;
    }

    public bool TryAddEntity2D(Entity2D _entity){
        var any = entity2Ds.Any(entity => entity.Id == _entity.Id);
        Insist.IsFalse(any, $"Can't add this entity, there is a entity with {_entity.Id}");
        entity2Ds.Add(_entity);
        return !any;
    }

    public List<Entity2D> GetEntity2DsByTag(string tag){
        var entity2DsWithTag = entity2Ds.Where(entity => entity.HasTag(tag));
        return entity2DsWithTag.ToList();
    }

    public void RemoveEntity2DsByTag(string tag) => 
        entity2Ds.Where(entity => entity.HasTag(tag)).ToList().ForEach(e => TryRemoveEntity2DById(e.Id));
    public override void OnEnabled()
    {
        base.OnEnabled();
        entity2Ds.ForEach(e => e.Enabled = true);
    }
    public override void OnDisabled()
    {
        base.OnDisabled();
        entity2Ds.ForEach(e => e.Enabled = false);
    }
}