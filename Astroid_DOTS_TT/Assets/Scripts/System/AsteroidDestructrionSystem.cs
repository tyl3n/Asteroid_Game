using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Collections;
using Random = UnityEngine.Random;

public partial class AsteroidDestructionSystem : SystemBase
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
        Entities.WithoutBurst().WithStructuralChanges().WithAll<AsteroidTagComponent>().ForEach((
            Entity _entity, int entityInQueryIndex ,
            in DestroyableComponentData _destroyable,
            in AsteroidTagComponent _asteroidTag,
            in Rotation _rotation,
            in Translation _translation) =>
        {
            if(_destroyable.m_mustBeDestroyed)
            {
                m_entityManager.DestroyEntity(_entity);
                var newScore = gameInfo.m_score + _asteroidTag.m_scoreValue;

                m_entityManager.SetComponentData(gameInfoEntity, new GameInfoComponentData
                {
                    m_score = newScore,
                    m_life = gameInfo.m_life
                });

                if (_asteroidTag.m_childrenPrefab != null )
                {
                    
                    for (int i =0;i<_asteroidTag.m_numberOfChildrenToSpawn;++i)
                    {
                        var newAsteroid = ecb.Instantiate(entityInQueryIndex, _asteroidTag.m_childrenPrefab);
                        var randomMoveDirection = math.normalize(new float3(Random.Range(-.8f, .8f), Random.Range(-.8f, .8f), 0));
                        var randomRotation = math.normalize(new float3(0 , 0, Random.value));
                        Debug.Log($"randomMove {randomMoveDirection}");

                        ecb.SetComponent(entityInQueryIndex , newAsteroid, new Translation
                        {
                            Value = _translation.Value
                        });

                        ecb.SetComponent(entityInQueryIndex , newAsteroid, new Rotation()
                        {
                            Value = Quaternion.Euler(randomRotation.x,randomRotation.y, randomRotation.z)
                        });

                        ecb.SetComponent(entityInQueryIndex , newAsteroid, new MovementCommandsComponentData()
                        {
                            m_lastPosition = _translation.Value,
                            m_currentDirection = randomMoveDirection,
                            m_currentLinearCommand = 1,
                            m_currentAngularCommand = randomRotation
                        });
                    } 

                } 
            } 
        }).Run();
        array.Dispose();
    }
}
