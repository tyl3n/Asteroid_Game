using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public partial class ControlSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var query = GetEntityQuery(typeof(InputComponentData));
        var array = query.ToComponentDataArray<InputComponentData>(Allocator.TempJob);

        if (array.Length == 0 || array.Length > 1 )
        {
            array.Dispose();
            return;
        }
        var inputData = array[0];
        
        Entities.WithAll<PlayerTagComponent>().
            ForEach((ref MovementCommandsComponentData _movementCommandsComponentData,ref ShootingComponentData _weaponComponent, ref PlayerInfoComponentData _playerInfo) =>
        {
            var turningLeft = inputData.m_inputLeft ? 1 : 0;
            var turningRight = inputData.m_inputRight ? 1 : 0;
            
            var rotationDirection = turningLeft - turningRight;

            _weaponComponent.m_isFiring = inputData.m_inputShoot;
            _playerInfo.m_hyperSpaceActive = inputData.m_inputHyperspace;
            
            _movementCommandsComponentData.m_currentAngularCommand = new float3(0,0,rotationDirection);
            _movementCommandsComponentData.m_currentLinearCommand = inputData.m_inputForward?1:0;

        }).ScheduleParallel();
        
        array.Dispose();
    }
}
