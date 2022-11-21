using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MovementCommandsComponentData : IComponentData
{
    public float3 m_currentDirection;
    public float3 m_currentAngularCommand;
    public float m_currentLinearCommand;
    public float3 m_lastPosition;
}
