using Unity.Entities;

[GenerateAuthoringComponent]
public struct AIControlComponentData : IComponentData
{
    public float m_shootTime;
    public float m_shootTimer;
    public float m_directionChangeTime;
    public float m_directionChangeTimer;
}

