using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class SetFowardMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.WithAll<FowardMovementDirectionComponent>().ForEach((
            ref MovementCommandsComponentData _movementCommandsComponentData, 
            in Rotation _rotation) =>
        {
            var direction = math.mul(_rotation.Value, math.up());
            _movementCommandsComponentData.m_currentDirection = direction;

        }).Schedule();
    }
}
