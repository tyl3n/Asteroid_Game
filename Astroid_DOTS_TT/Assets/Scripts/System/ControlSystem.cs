using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public partial class ControlSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var query = GetEntityQuery(typeof(InputComponentData));
        var array = query.ToComponentDataArray<InputComponentData>(Allocator.TempJob);

        if (array.Length == 0)
        {
            array.Dispose();
            return;
        }
        var inputData = array[0];
        
        Entities.WithAll<PlayerTagComponentData>().
            ForEach((ref MovementControlComponentData movementComponentData,ref ShootingComponentData shootingComponent) =>
        {
            var turningLeft = inputData.inputLeft ? 1 : 0;
            var turningRight = inputData.inputRight ? 1 : 0;
            
            var rotationDirection = turningLeft - turningRight;

            shootingComponent.isFiring = inputData.inputShoot;
            
            movementComponentData.currentAngularMovement = new float3(0,0,rotationDirection);
            movementComponentData.currentLinearMovement = inputData.inputForward?1:0;

        }).ScheduleParallel();
        
        array.Dispose();
    }
}
