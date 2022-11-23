using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerLibrary_Authoring : MonoBehaviour, IConvertGameObjectToEntity,IDeclareReferencedPrefabs
{
    public GameInstance m_gameInstance;
    public GameObject[] m_playerPrefabs;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var buffer = dstManager.AddBuffer<EntityBufferElement>(entity);
        foreach (var player in m_playerPrefabs)
        {
            buffer.Add(new EntityBufferElement()
            {
                m_entity = conversionSystem.GetPrimaryEntity(player)
            });
        }

        m_gameInstance.m_playerLibrary = entity;
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.AddRange(m_playerPrefabs);
    }
}