using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
public partial class PlayerDestructionSystem : SystemBase
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

        if (array.Length == 0 ||array.Length > 1 )
        {
            array.Dispose();
            return;
        }
        
        var gameInfo = array[0];

        var gameInfoEntity = GetSingletonEntity<GameInfoComponentData>();
        
        var ecb = m_endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
        Entities.WithoutBurst().WithStructuralChanges().WithAll<PlayerTagComponent>().ForEach((
            Entity _entity, int entityInQueryIndex ,
            ref DestroyableComponentData _destroyable,
            ref PlayerInfoComponentData _playerInfo,
            in Rotation _rotation,
            in Translation _translation) =>
        {
            if(_destroyable.m_mustBeDestroyed)
            {
                if(!_playerInfo.m_shieldActive)
                {
                    m_entityManager.DestroyEntity(_entity);
                    var newLife = gameInfo.m_life - 1; 
                    m_entityManager.SetComponentData(gameInfoEntity,new GameInfoComponentData
                    {
                        m_life = newLife,
                        m_score = gameInfo.m_score
                    });
                    
                } 
                else 
                {
                    _destroyable.m_mustBeDestroyed = false;
                    
                }
            }
            
        }).Run();
        array.Dispose();
    }
}
