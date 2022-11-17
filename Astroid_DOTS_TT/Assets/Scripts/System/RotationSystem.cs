using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;


public partial class RotateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((
            ref PhysicsVelocity velocity, ref MovementControlComponentData movementControlComponentData,
            ref Rotation rotation, in MovementParamsComponentData movementParamsComponentData, in PhysicsMass physicsMass) =>
        {
            PhysicsComponentExtensions.ApplyAngularImpulse(
                ref velocity, physicsMass,
                movementControlComponentData.currentAngularMovement * movementParamsComponentData.angularVelocity);

            var AngularMovement = PhysicsComponentExtensions.GetAngularVelocityWorldSpace(in velocity, in physicsMass, in rotation);
        
            if(math.length(AngularMovement)>movementParamsComponentData.maxAngularVelocity)
            {
                PhysicsComponentExtensions.SetAngularVelocityWorldSpace(ref velocity, physicsMass, rotation,
                    math.normalize(AngularMovement)* movementParamsComponentData.maxAngularVelocity );
            }
        
        }).Schedule();
    }
}

