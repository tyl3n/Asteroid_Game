
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;

public partial class OffScreenDetectionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var query = GetEntityQuery(typeof(ScreenInfoComponentData));
        var array = query.ToComponentDataArray<ScreenInfoComponentData>(Allocator.TempJob);

        if (array.Length == 0 || array.Length>1)
        {
            array.Dispose();
            return;
        }
        
        var screenData = array[0];

        Entities.ForEach(
            (Entity _entity, ref OffScreenWrapperComponentData _offScreen,
                in MovementCommandsComponentData _moveComponent, in Translation _translation) =>
            {

                _offScreen.m_isOffScreenLeft = _translation.Value.x > screenData.m_width * .5f + _offScreen.m_bounds;
                _offScreen.m_isOffScreenRight = _translation.Value.x < -screenData.m_width * .5f - _offScreen.m_bounds;
                _offScreen.m_isOffScreenUp = _translation.Value.y < -screenData.m_height * .5f - _offScreen.m_bounds;
                _offScreen.m_isOffScreenDown = _translation.Value.y > screenData.m_height *.5f + _offScreen.m_bounds;
                
                
            }).Schedule();
            array.Dispose();
    }
}
