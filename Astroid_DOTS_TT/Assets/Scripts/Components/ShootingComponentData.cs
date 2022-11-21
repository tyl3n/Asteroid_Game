using Unity.Entities;

[GenerateAuthoringComponent]
public struct ShootingComponentData : IComponentData
{
    public Entity m_projectilePrefab;
    public bool m_isFiring;
    public float m_fireRate;
    public float m_timer;
}

