using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct AsteroidTagComponent : IComponentData
{
    public Entity m_childrenPrefab;
    public int m_numberOfChildrenToSpawn;
    public float m_scoreValue;
}
