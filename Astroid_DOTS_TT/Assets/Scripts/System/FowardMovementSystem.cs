using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class FowardMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.WithAll<FowardMovementVectorComponentData>().ForEach((
            ref MovementControlComponentData movementControlComponentData, 
            in Rotation rotation) =>
        {
            var direction = math.mul(rotation.Value, math.up());
            movementControlComponentData.currentMovementDirection = direction;

        }).Schedule();
    }
    
}
