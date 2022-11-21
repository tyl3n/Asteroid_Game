using Unity.Entities;

[GenerateAuthoringComponent]
public struct DestroyableComponentData : IComponentData
{
    public bool m_mustBeDestroyed;
}
