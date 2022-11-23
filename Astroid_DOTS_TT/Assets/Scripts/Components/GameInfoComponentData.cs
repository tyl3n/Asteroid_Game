using Unity.Entities;

[GenerateAuthoringComponent]
public struct GameInfoComponentData : IComponentData
{
    public int m_currentPlayerCount;
    public int m_currentAsteroidSpawnCount;
    public int m_currentPickUpSpawnCount;
    public int m_currentUFOSpawnCount;
}
