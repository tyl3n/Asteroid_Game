using Unity.Entities;
using UnityEngine;

public class OffScreenWrapperComponentData_Authoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public SpriteRenderer m_sprite;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Debug.Log($" Bounds : {m_sprite.bounds.extents.magnitude}");
        dstManager.AddComponentData(entity, new OffScreenWrapperComponentData
        {
            m_bounds = m_sprite.bounds.extents.magnitude
        });
    }
}
