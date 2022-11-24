using Unity.Collections;
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
        var query = GetEntityQuery(typeof(PlayerInfoComponentData));
        var array = query.ToComponentDataArray<PlayerInfoComponentData>(Allocator.TempJob);

        if (array.Length == 0 || array.Length>1)
        {
            array.Dispose();
            return;
        }
        
        var playerInfo = array[0];

        var playerInfoEntity = GetSingletonEntity<PlayerInfoComponentData>();
        
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
                m_entityManager.SetComponentData(playerInfoEntity,new PlayerInfoComponentData(){
                    m_shieldActive = true,
                    m_shieldTimer = 0.0f,
                    m_hyperSpaceActive = playerInfo.m_hyperSpaceActive,
                    m_hyperSpaceCooldownTimer = playerInfo.m_hyperSpaceCooldownTimer

                });
            } 
        }).Run();
        array.Dispose();
    }
}
