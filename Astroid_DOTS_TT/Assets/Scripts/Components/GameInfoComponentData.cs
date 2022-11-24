using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
[GenerateAuthoringComponent]
public struct GameInfoComponentData : IComponentData
{
    public int m_life;
    public float m_score;

    public int m_currentPlayerCount;
    public int m_currentAsteroidSpawnCount;
    public int m_currentPickUpSpawnCount;
    public int m_currentUFOSpawnCount;
    
    public float3 m_nextSpawnPoint;

}
