using Unity.Entities;

[GenerateAuthoringComponent]
public struct LifeTimeComponentData : IComponentData
{
    public float m_maxLifeTime;
    public float m_currentLifeTime;
}

