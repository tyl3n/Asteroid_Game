using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
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
        var gameParamQuery = GetEntityQuery(typeof(GameParamsComponentData));
        var gameParamArray = gameParamQuery.ToComponentDataArray<GameParamsComponentData>(Allocator.TempJob);

        if (gameParamArray.Length == 0 || gameParamArray.Length>1)
        {
            gameParamArray.Dispose();
            return;
        }
        
        var gameParams = gameParamArray[0];
        
        Entities.WithoutBurst().WithAll<PlayerTagComponent>().ForEach((ref PlayerInfoComponentData _playerInfo) =>
        {
            if ( _playerInfo.m_shieldActive)
            {
                _playerInfo.m_shieldTimer += Time.DeltaTime;
                Debug.Log($"Shield Active :{_playerInfo.m_shieldTimer}");
            }
            
            if (_playerInfo.m_shieldTimer >= gameParams.m_shieldDuration)
            {
                _playerInfo.m_shieldActive = false;
                _playerInfo.m_shieldTimer = 0.0f;
            }
        }).Run();
        gameParamArray.Dispose();
    }
}
