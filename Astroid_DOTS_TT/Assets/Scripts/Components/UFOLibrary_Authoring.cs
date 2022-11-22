using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class UFOLibrary_Authoring : MonoBehaviour, IConvertGameObjectToEntity,IDeclareReferencedPrefabs
{
    public GameInstance m_gameInstance;
    public GameObject[] m_UFOPrefabs;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var buffer = dstManager.AddBuffer<EntityBufferElement>(entity);
        foreach (var UFO in m_UFOPrefabs)
        {
            buffer.Add(new EntityBufferElement()
            {
                m_entity = conversionSystem.GetPrimaryEntity(UFO)
            });
        }

        m_gameInstance.m_UFOLibrary = entity;
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.AddRange(m_UFOPrefabs);
    }
}