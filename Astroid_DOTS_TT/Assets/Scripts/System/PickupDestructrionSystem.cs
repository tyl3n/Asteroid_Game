using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
public partial class PickUpDestructionSystem : SystemBase
{
    private EntityManager m_entityManager;

    private EndSimulationEntityCommandBufferSystem m_endSimulationEntityCommandBufferSystem;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        m_entityManager = World.EntityManager;
        m_endSimulationEntityCommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var gameInfoComponentData = GetSingleton<GameInfoComponentData>();

        var ecb = m_endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
        Entities.WithoutBurst().WithStructuralChanges().WithAll<PickUpTagComponent>().ForEach((
            Entity _entity, int entityInQueryIndex ,
            in DestroyableComponentData _destroyable,
            in Rotation _rotation,
            in Translation _translation) =>
        {
            if(_destroyable.m_mustBeDestroyed)
            {
                m_entityManager.DestroyEntity(_entity);
                gameInfoComponentData.m_shieldActive = true;
            } 
        }).Run();
    }
}
