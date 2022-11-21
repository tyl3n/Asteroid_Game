using Unity.Entities;

[GenerateAuthoringComponent]
public struct GameInfoComponentData : IComponentData
{
    public int m_currentAsteroidSpawnCount;
    public int m_maxAsteroidSpawnCount;
    public int m_playerLife;
}
