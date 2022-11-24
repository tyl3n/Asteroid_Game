
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public partial class GameInfoCountSystem : SystemBase
{
    protected override void OnUpdate()
    {
       Entities.WithoutBurst().WithAll<GameInfoComponentData>().
            ForEach((ref GameInfoComponentData gameInfo) =>
        {
            EntityQuery query1 = GetEntityQuery(ComponentType.ReadOnly<AsteroidTagComponent>());
 
            int asteroidCount = query1.CalculateEntityCount();

            gameInfo.m_currentAsteroidSpawnCount = asteroidCount;

            EntityQuery query2 = GetEntityQuery(ComponentType.ReadOnly<PickUpTagComponent>());
    
            int pickUpCount = query2.CalculateEntityCount();

            gameInfo.m_currentPickUpSpawnCount = pickUpCount;

            EntityQuery query3 = GetEntityQuery(ComponentType.ReadOnly<UFOTagComponent>());
    
            int UFOCount = query3.CalculateEntityCount();
            gameInfo.m_currentUFOSpawnCount = UFOCount;
    
            EntityQuery query4 = GetEntityQuery(ComponentType.ReadOnly<PlayerTagComponent>());
    
            int playerCount = query4.CalculateEntityCount();
            gameInfo.m_currentPlayerCount = playerCount;
        }).Run();
        
    }
}
