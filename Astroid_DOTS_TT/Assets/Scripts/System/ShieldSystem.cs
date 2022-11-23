using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
public partial class ShieldActivationSystem : SystemBase
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
        var deltaTime = Time.DeltaTime;
        Entities.WithoutBurst().WithStructuralChanges().WithAll<PlayerTagComponent>().ForEach((ref PlayerInfoComponentData playerInfo) =>
        {
            playerInfo.m_shieldTimer += deltaTime;
            if (playerInfo.m_shieldActive)
            {
                if (playerInfo.m_shieldTimer > playerInfo.m_shieldTime)
                {
                    playerInfo.m_shieldActive = false;
                    playerInfo.m_shieldTimer = 0.0f;
                }
            }

        }).Run();
    }
}
