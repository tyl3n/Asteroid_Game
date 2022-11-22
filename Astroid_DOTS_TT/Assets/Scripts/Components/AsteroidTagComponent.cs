using Unity.Entities;

[GenerateAuthoringComponent]
public struct AsteroidTagComponent : IComponentData
{
    public Entity m_childrenPrefab;
    public int m_numberOfChildrenToSpawn;
}
