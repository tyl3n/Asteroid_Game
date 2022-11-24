using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Collections;
using Random = UnityEngine.Random;
public partial class UFODestructionSystem : SystemBase
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
        var query = GetEntityQuery(typeof(GameInfoComponentData));
        var array = query.ToComponentDataArray<GameInfoComponentData>(Allocator.TempJob);

        if (array.Length == 0 || array.Length>1)
        {
            array.Dispose();
            return;
        }
        
        var gameInfo = array[0];

        var gameInfoEntity = GetSingletonEntity<GameInfoComponentData>();

        var ecb = m_endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
        Entities.WithoutBurst().WithStructuralChanges().WithAll<UFOTagComponent>().ForEach((
            Entity _entity, int entityInQueryIndex ,
            in DestroyableComponentData _destroyable,
            in UFOTagComponent _UFOTagComponent,
            in Rotation _rotation,
            in Translation _translation) =>
        {
            if(_destroyable.m_mustBeDestroyed)
            {
                 var newScore = gameInfo.m_score + _UFOTagComponent.m_scoreValue;

                m_entityManager.SetComponentData(gameInfoEntity, new GameInfoComponentData
                {
                    m_score = newScore,
                    m_life = gameInfo.m_life
                });
                m_entityManager.DestroyEntity(_entity);
            } 
        }).Run();
        array.Dispose();
    }
}
