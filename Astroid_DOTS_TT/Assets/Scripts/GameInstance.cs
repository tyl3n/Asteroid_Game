using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;
public class GameInstance : MonoBehaviour
{
    public Entity m_asteroidLibrary;
    private EntityManager m_entityManager;

    public Transform[] m_asteroidsSpawnPositions;
    private Vector3[] m_spawnPositionsVectors;

    private float m_currentTimer;
    public float m_asteroidSpawnFrequency = 2f;

    private void Awake()
    {
        m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        m_spawnPositionsVectors = new Vector3[m_asteroidsSpawnPositions.Length];
        for (int i = 0; i < m_spawnPositionsVectors.Length; i++)
        {
            m_spawnPositionsVectors[i] = m_asteroidsSpawnPositions[i].position;
        }
    }


    void Start()
    {
        m_entityManager.CreateEntity(typeof(InputComponentData));
    }

    private void SpawnAsteroid()
    {
        var buffer = m_entityManager.GetBuffer<EntityBufferElement>(m_asteroidLibrary);
        var lengthOfBuffer = buffer.Length;
        var randomAsteroidIndex = Random.Range(0, lengthOfBuffer);
        var newAsteroid = m_entityManager.Instantiate(buffer[randomAsteroidIndex].m_entity);

        var randomSpawnPositionIndex = Random.Range(0, m_spawnPositionsVectors.Length);
        var spawnPosition = m_spawnPositionsVectors[randomSpawnPositionIndex];
        
        m_entityManager.SetComponentData(newAsteroid, new Translation()
        {
            Value = spawnPosition
        });

        var randomMoveDirection = math.normalize(new float3(Random.Range(-.8f, .8f), Random.Range(-.8f, .8f), 0));
        var randomRotation = math.normalize(new float3(0 , 0, Random.value));
        Debug.Log($"randomMove {randomMoveDirection}");
        m_entityManager.SetComponentData(newAsteroid, new Rotation()
        {
            Value = Quaternion.Euler(randomRotation.x,randomRotation.y, randomRotation.z)
        });

        m_entityManager.SetComponentData(newAsteroid, new MovementCommandsComponentData()
        {
            m_lastPosition = spawnPosition,
            m_currentDirection = randomMoveDirection,
            m_currentLinearCommand = 1,
            m_currentAngularCommand = randomRotation
        });
    }

    private void Update()
    {
        m_currentTimer += Time.deltaTime;

        if (m_currentTimer > m_asteroidSpawnFrequency)
        {
            m_currentTimer = 0;
            SpawnAsteroid();
        }

    }
}
