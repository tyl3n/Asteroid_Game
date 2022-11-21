using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
public partial class OffScreenWrappingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var screenDataComponent = GetSingleton<ScreenInfoComponentData>();
        
        Entities.WithAll<OffScreenWrapperComponentData>().ForEach((
            Entity _entity, ref OffScreenWrapperComponentData _offScreenWrapperComponent, ref Translation _translation) => 
        {
            if (_offScreenWrapperComponent.m_isOffScreenLeft)
            {
                _translation.Value = SpawnOnRight(_translation.Value, _offScreenWrapperComponent.m_bounds, screenDataComponent);
            }
            else if (_offScreenWrapperComponent.m_isOffScreenRight)
            {
                _translation.Value = SpawnOnLeft(_translation.Value, _offScreenWrapperComponent.m_bounds, screenDataComponent);
            }
            else if (_offScreenWrapperComponent.m_isOffScreenUp)
            {
                _translation.Value = SpawnOnBottom(_translation.Value,_offScreenWrapperComponent.m_bounds, screenDataComponent);
            }
            else if (_offScreenWrapperComponent.m_isOffScreenDown)
            {
                _translation.Value = SpawnOnTop(_translation.Value,_offScreenWrapperComponent.m_bounds, screenDataComponent);
            }
            
            _offScreenWrapperComponent.m_isOffScreenDown = false;
            _offScreenWrapperComponent.m_isOffScreenRight = false;
            _offScreenWrapperComponent.m_isOffScreenUp = false;
            _offScreenWrapperComponent.m_isOffScreenLeft = false;
            
        }).ScheduleParallel();
    }

    private static float3 SpawnOnLeft(float3 _position, float _bounds, ScreenInfoComponentData _screenDataComponent)
    {
        return new float3((_bounds + _screenDataComponent.m_width)*.5f, _position.y, 0);;
    }
    private static float3 SpawnOnRight(float3 _position,float _bounds, ScreenInfoComponentData _screenDataComponent)
    {
        return new float3(-(_bounds + _screenDataComponent.m_width)*.5f,_position.y, 0);;
    }
    private static float3 SpawnOnTop(float3 _position,float _bounds, ScreenInfoComponentData _screenDataComponent)
    {
        return  new float3(_position.x,-(_bounds + _screenDataComponent.m_height)*.5f,0);
    }
    private static float3 SpawnOnBottom(float3 _position,float _bounds, ScreenInfoComponentData _screenDataComponent)
    {
        return new float3(_position.x,(_bounds + _screenDataComponent.m_height)*.5f,0);
    }
}
