using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Collections;
using Random = UnityEngine.Random;
public partial class ShieldVisualSystem : SystemBase
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

        if (array.Length == 0)
        {
            array.Dispose();
            return;
        }
        var gameInfo = array[0];
        var ecb = m_endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
        Entities.WithoutBurst().WithStructuralChanges().WithAll<ShieldTagComponent>().ForEach(( SpriteRenderer _renderer ) =>
        {
            _renderer.enabled = gameInfo.m_shieldActive;
        }).Run();
        array.Dispose();
    
        
    }
}
