using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

public partial class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((
            ref MovementCommandsComponentData _movementCommandsComponentData, ref PhysicsVelocity _velocity,
            in MovementParametersComponentData _movementParametersComponentData,  
            in PhysicsMass _physicsMass, in Translation _translation ) =>
        {
            
            PhysicsComponentExtensions.ApplyLinearImpulse(
                ref _velocity, 
                _physicsMass, 
                _movementCommandsComponentData.m_currentDirection * 
                _movementCommandsComponentData.m_currentLinearCommand * 
                _movementParametersComponentData.m_linearVelocity
            );
        
            if (math.length(_velocity.Linear) > _movementParametersComponentData.m_maxLinearVelocity) 
                _velocity.Linear = math.normalize(_velocity.Linear) * _movementParametersComponentData.m_maxLinearVelocity;

        }).Schedule();

    }
}
