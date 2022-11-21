using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ScreenInfoComponentData_Authoring : MonoBehaviour, IConvertGameObjectToEntity
{

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Camera.main.aspect;

        var size = new float2(height, width);

        dstManager.AddComponentData(entity, new ScreenInfoComponentData()
        {
            m_height = size.x,
            m_width = size.y
        });
    }
}
