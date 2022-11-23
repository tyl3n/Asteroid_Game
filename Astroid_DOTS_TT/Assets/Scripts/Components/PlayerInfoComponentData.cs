using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerInfoComponentData : IComponentData
{
    public bool m_shieldActive;
    public float m_shieldTimer;
    public float m_shieldTime;
}
