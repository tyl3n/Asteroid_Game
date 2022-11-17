using Unity.Entities;


[GenerateAuthoringComponent]
public struct MovementParamsComponentData : IComponentData
{
    public float linearVelocity;
    public float maxLinearVelocity;
    public float angularVelocity;
    public float maxAngularVelocity;
}
