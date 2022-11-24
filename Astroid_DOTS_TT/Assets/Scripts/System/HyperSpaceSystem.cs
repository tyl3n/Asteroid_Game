using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
public partial class HyperSpaceSystem : SystemBase
{
    
    protected override void OnUpdate()
    {
        var gameInfoQuery = GetEntityQuery(typeof(GameInfoComponentData));
        var gameInfoArray = gameInfoQuery.ToComponentDataArray<GameInfoComponentData>(Allocator.TempJob);

        if (gameInfoArray.Length == 0 || gameInfoArray.Length>1)
        {
            gameInfoArray.Dispose();
            return;
        }

        var gameInfo = gameInfoArray[0];

        var gameParamQuery = GetEntityQuery(typeof(GameParamsComponentData));
        var gameParamArray = gameParamQuery.ToComponentDataArray<GameParamsComponentData>(Allocator.TempJob);

        if (gameParamArray.Length == 0 || gameParamArray.Length>1)
        {
            gameParamArray.Dispose();
            return;
        }
        
        var gameParams = gameParamArray[0];
        
        Entities.WithoutBurst().WithAll<PlayerTagComponent>().ForEach((
            Entity _entity, ref PlayerInfoComponentData _playerInfo, ref Translation _translation) => 
        {
            if (_playerInfo.m_hyperSpaceActive && _playerInfo.m_hyperSpaceCooldownTimer >= gameParams.m_hyperSpaceCooldown)
            {
                _translation.Value = gameInfo.m_nextSpawnPoint;
                _playerInfo.m_hyperSpaceActive = false;
                _playerInfo.m_hyperSpaceCooldownTimer = 0.0f;
            }
            _playerInfo.m_hyperSpaceCooldownTimer += Time.DeltaTime;

            
        }).Run();
        gameParamArray.Dispose();
        gameInfoArray.Dispose();
    }

    private static float3 SpawnOnLeft(float3 _position, float _bounds, ScreenInfoComponentData _screenDataComponent)
    {
        return new float3((_bounds + _screenDataComponent.m_width)*.5f, _position.y, 0);;
    }
    private static float3 SpawnOnRight(float3 _position,float _bounds, ScreenInfoComponentData _screenDataComponent)
    {
        return new float3(-(_bounds + _screenDataComponent.m_width)*.5f,_position.y, 0);;
    }
    private static float3 SpawnOnTop(float3 _position,float _bounds, ScreenInfoComponentData _screenDataComponent)
    {
        return  new float3(_position.x,-(_bounds + _screenDataComponent.m_height)*.5f,0);
    }
    private static float3 SpawnOnBottom(float3 _position,float _bounds, ScreenInfoComponentData _screenDataComponent)
    {
        return new float3(_position.x,(_bounds + _screenDataComponent.m_height)*.5f,0);
    }
}
