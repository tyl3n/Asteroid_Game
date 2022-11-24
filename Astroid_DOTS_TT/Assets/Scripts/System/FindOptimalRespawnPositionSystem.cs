using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
public partial class FindOptimalRespawnSystem : SystemBase
{
    protected override void OnUpdate()
    { 
        var ScreenDataArray = GetEntityQuery(typeof(ScreenInfoComponentData)).ToComponentDataArray<ScreenInfoComponentData>(Allocator.TempJob);
        if (ScreenDataArray.Length == 0 || ScreenDataArray.Length >1)
        {
            ScreenDataArray.Dispose();
            return;
        }

        var GameParamQuery = GetEntityQuery(typeof(GameParamsComponentData)).ToComponentDataArray<GameParamsComponentData>(Allocator.TempJob);
        if (GameParamQuery.Length == 0 || GameParamQuery.Length >1)
        {
            GameParamQuery.Dispose();
            return;
        }

        var screenDataComponent = ScreenDataArray[0];
        var gameParams = GameParamQuery[0];

        Entities.WithoutBurst().WithAll<GameInfoComponentData>().
            ForEach((ref GameInfoComponentData gameInfo) =>
        {

        
            bool isSpawnPointValid = false;
            
            //isSpawnPointValid = IsSpawnPointValid(gameInfo.m_nextSpawnPoint, gameParams.m_minimalSpawnDistance);

            if (!isSpawnPointValid)
            {
   
                float height = screenDataComponent.m_height*0.5f;
                float width = screenDataComponent.m_width*0.5f;
                for (int i = 0; i < 5 || isSpawnPointValid; i++)
                {
                    var randomPostition = new float3(Random.Range(-height*0.7f, height*0.7f), Random.Range(-width*0.7f, width*0.7f), 0);
                    Debug.Log($"randomPos {randomPostition}");
                    isSpawnPointValid = IsSpawnPointValid(randomPostition,gameParams.m_minimalSpawnDistance);

                    if (isSpawnPointValid)
                    {
                        gameInfo.m_nextSpawnPoint = randomPostition;
                        return;
                    }
                    
                }
            }
            else 
            {
                return;
            }
        }).Run();

        ScreenDataArray.Dispose();
        GameParamQuery.Dispose();

    }
    public bool IsSpawnPointValid(float3 position, float minimalSpawnDistance){

        bool isSpawnPointValid = true;
         Entities
            .WithAll<RespawnBlockerTagComponent>()
            .ForEach((int entityInQueryIndex, in LocalToWorld localToWorld) =>
            {
                if (math.distancesq(localToWorld.Position,position)<minimalSpawnDistance*minimalSpawnDistance)
                {
                    isSpawnPointValid = false;
                }
            })
            .ScheduleParallel();
        return isSpawnPointValid;
    }
}
