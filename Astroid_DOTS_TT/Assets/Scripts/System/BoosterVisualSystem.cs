using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
public partial class BoosterVisualSystem : SystemBase
{
    private EntityManager m_entityManager;
    private GameInstance m_instance;
    private EndSimulationEntityCommandBufferSystem m_endSimulationEntityCommandBufferSystem;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        m_entityManager = World.EntityManager;
        m_endSimulationEntityCommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        
        var query = GetEntityQuery(typeof(InputComponentData));
        var array = query.ToComponentDataArray<InputComponentData>(Allocator.TempJob);

        if (array.Length == 0 || array.Length >1)
        {
            array.Dispose();
            return;
        }
        var inputData = array[0];

        Entities.WithoutBurst().WithAll<BoosterTagComponent>().ForEach(( SpriteRenderer _renderer ) =>
        {
            _renderer.enabled = inputData.m_inputForward;
        }).Run();
        array.Dispose();
    }
}
