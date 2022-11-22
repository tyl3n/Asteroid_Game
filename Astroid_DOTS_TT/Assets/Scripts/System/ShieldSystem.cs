using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
public partial class ShieldSystem : SystemBase
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

        var deltaTime = Time.DeltaTime;

         Entities.WithoutBurst().WithAll<ShieldTagComponent>().ForEach((Entity _entity, SpriteRenderer _renderer ) =>
        {
            gameInfoComponentData.m_shieldTimer += deltaTime;
        
            if (gameInfoComponentData.m_shieldTime > gameInfoComponentData.m_shieldTimer)
            {
                gameInfoComponentData.m_shieldActive = true;
                _renderer.enabled = (true);
            }
            else 
            {
                _renderer.enabled =(false);
            }
        }).Run();
    }
}
