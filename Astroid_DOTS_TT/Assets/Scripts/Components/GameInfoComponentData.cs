using Unity.Entities;

[GenerateAuthoringComponent]
public struct GameInfoComponentData : IComponentData
{
    
    public int m_currentAsteroidSpawnCount;
    public int m_maxAsteroidSpawnCount;
    public bool m_shieldActive;
    public float m_shieldTimer;
    public float m_shieldTime;
}
