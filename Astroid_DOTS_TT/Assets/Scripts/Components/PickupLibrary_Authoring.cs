using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PickUpLibrary_Authoring : MonoBehaviour, IConvertGameObjectToEntity,IDeclareReferencedPrefabs
{
    public GameInstance m_gameInstance;
    public GameObject[] m_pickupPrefabs;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var buffer = dstManager.AddBuffer<EntityBufferElement>(entity);
        foreach (var pickup in m_pickupPrefabs)
        {
            buffer.Add(new EntityBufferElement()
            {
                m_entity = conversionSystem.GetPrimaryEntity(pickup)
            });
        }

        m_gameInstance.m_pickupPrefabs = entity;
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.AddRange(m_pickupPrefabs);
    }
}
