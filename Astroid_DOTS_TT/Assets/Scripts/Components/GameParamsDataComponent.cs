using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
[GenerateAuthoringComponent]
public struct GameParamsComponentData : IComponentData
{
    public float m_minimalSpawnDistance;
    public int m_startLife;
    public float m_hyperSpaceCooldown;
    public float m_shieldDuration;
}
