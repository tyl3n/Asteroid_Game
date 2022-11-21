using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class AsteroidLibrary_Authoring : MonoBehaviour, IConvertGameObjectToEntity,IDeclareReferencedPrefabs
{
    public GameInstance m_gameInstance;
    public GameObject[] m_asteroidsPrefabs;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var buffer = dstManager.AddBuffer<EntityBufferElement>(entity);
        foreach (var asteroid in m_asteroidsPrefabs)
        {
            buffer.Add(new EntityBufferElement()
            {
                m_entity = conversionSystem.GetPrimaryEntity(asteroid)
            });
        }

        m_gameInstance.m_asteroidLibrary = entity;
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.AddRange(m_asteroidsPrefabs);
    }
}

public struct EntityBufferElement : IBufferElementData
{
    public Entity m_entity;
}
