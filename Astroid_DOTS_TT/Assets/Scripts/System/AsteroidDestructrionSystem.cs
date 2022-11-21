using Unity.Entities;

public partial class AsteroidDestructionSystem : SystemBase
{
    private EntityManager m_entityManager;

    protected override void OnCreate()
    {
        base.OnCreate();
        m_entityManager = World.EntityManager;
    }

    protected override void OnUpdate()
    {
        Entities.WithoutBurst().WithStructuralChanges().WithAll<AsteroidTagComponent>().ForEach((
            Entity _entity, 
            in DestroyableComponentData _destroyable) =>
        {
            if(_destroyable.m_mustBeDestroyed) m_entityManager.DestroyEntity(_entity);

        }).Run();
    }
}
