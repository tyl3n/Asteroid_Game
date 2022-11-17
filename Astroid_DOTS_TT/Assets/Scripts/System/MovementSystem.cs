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
                ref MovementControlComponentData movementControlComponentData, ref PhysicsVelocity velocity,
                in MovementParamsComponentData movementParamsComponentData,  
                in PhysicsMass physicsMass, in Translation translation ) =>
            {
                movementControlComponentData.previousPos = translation.Value;
                
                PhysicsComponentExtensions.ApplyLinearImpulse(
                    ref velocity, 
                    physicsMass, 
                    movementControlComponentData.currentMovementDirection * 
                    movementControlComponentData.currentLinearMovement * 
                    movementParamsComponentData.linearVelocity
                );
            
                if (math.length(velocity.Linear) > movementParamsComponentData.maxLinearVelocity) 
                    velocity.Linear = math.normalize(velocity.Linear) * movementParamsComponentData.maxLinearVelocity;

            }).ScheduleParallel();

    }
}