using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;

public partial class RotationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((
            ref PhysicsVelocity _velocity, ref MovementCommandsComponentData _commandsComponentData,
            ref Rotation _rotation, in MovementParametersComponentData _parametersComponentData, in PhysicsMass _physicsMass) =>
        {
            PhysicsComponentExtensions.ApplyAngularImpulse(
                ref _velocity, _physicsMass,
                _commandsComponentData.m_currentAngularCommand * _parametersComponentData.m_angularVelocity);

            var currentAngularSpeed = PhysicsComponentExtensions.GetAngularVelocityWorldSpace(in _velocity, in _physicsMass, in _rotation);
        
            if(math.length(currentAngularSpeed)>_parametersComponentData.m_maxAngularVelocity)
            {
                PhysicsComponentExtensions.SetAngularVelocityWorldSpace(ref _velocity, _physicsMass, _rotation,
                    math.normalize(currentAngularSpeed)* _parametersComponentData.m_maxAngularVelocity );
            }
        
        }).Schedule();
    }
}
