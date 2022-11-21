
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class OffScreenDetectionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var screenData = GetSingleton<ScreenInfoComponentData>();

        Entities.ForEach(
            (Entity _entity, ref OffScreenWrapperComponentData _offScreen,
                in MovementCommandsComponentData _moveComponent, in Translation _translation) =>
            {

                _offScreen.m_isOffScreenLeft = _translation.Value.x > screenData.m_width * .5f + _offScreen.m_bounds;
                _offScreen.m_isOffScreenRight = _translation.Value.x < -screenData.m_width * .5f - _offScreen.m_bounds;
                _offScreen.m_isOffScreenUp = _translation.Value.y < -screenData.m_height * .5f - _offScreen.m_bounds;
                _offScreen.m_isOffScreenDown = _translation.Value.y > screenData.m_height *.5f + _offScreen.m_bounds;
                
                
            }).Schedule();
    }
}
