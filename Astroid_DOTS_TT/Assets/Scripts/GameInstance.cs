using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;
public class GameInstance : MonoBehaviour
{
    public Entity m_asteroidLibrary;
    public Entity m_UFOLibrary;
    public Entity m_pickupLibrary;
    public Entity m_playerLibrary;
    public Entity m_gameInfo;
    private EntityManager m_entityManager;

    public Transform[] m_asteroidsSpawnPositions;
    public Transform[] m_pickUpSpawnPositions;
    public Transform[] m_playerSpawnPositions;
    private Vector3[] m_spawnPositionsVectors;
    private Vector3[] m_pickUpSpawnPositionsVectors;
    private Vector3[] m_playerSpawnPositionsVectors;

    private float m_currentTimer;
    private float m_UFOTimer;
    private float m_PickUpTimer;
    private float m_playerTimer;

    public float m_asteroidSpawnFrequency = 2f;
    public float m_UFOSpawnFrequency = 2f;
    public float m_pickUpSpawnFrequency = 2f;
    public float m_playerSpawnFrequency = 2f;

    public float m_shieldMaxTime = 4f;
    
    public int m_maxAsteroidSpawnCount = 10;
    public int m_maxUFOSpawnCount = 1;
    public int m_maxPickUpSpawnCount = 10;


    private void Awake()
    {
        m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        m_spawnPositionsVectors = new Vector3[m_asteroidsSpawnPositions.Length];
        for (int i = 0; i < m_spawnPositionsVectors.Length; i++)
        {
            m_spawnPositionsVectors[i] = m_asteroidsSpawnPositions[i].position;
        }

        m_pickUpSpawnPositionsVectors = new Vector3[m_pickUpSpawnPositions.Length];
        for (int i = 0; i < m_pickUpSpawnPositions.Length; i++)
        {
            m_pickUpSpawnPositionsVectors[i] = m_pickUpSpawnPositions[i].position;
        }

        m_playerSpawnPositionsVectors = new Vector3[m_playerSpawnPositions.Length];
        for (int i = 0; i < m_playerSpawnPositions.Length; i++)
        {
            m_playerSpawnPositionsVectors[i] = m_playerSpawnPositions[i].position;
        }
    }


    void Start()
    {
        m_entityManager.CreateEntity(typeof(InputComponentData));
        m_gameInfo = m_entityManager.CreateEntity(typeof(GameInfoComponentData));

    }

    private void SpawnAsteroid()
    {
        GameInfoComponentData gameInfo = m_entityManager.GetComponentData<GameInfoComponentData>(m_gameInfo);
        
        if (gameInfo.m_currentAsteroidSpawnCount >= m_maxAsteroidSpawnCount)
        {
            return;
        }

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

     private void SpawnUFO()
    {
         GameInfoComponentData gameInfo = m_entityManager.GetComponentData<GameInfoComponentData>(m_gameInfo);
        
        if (gameInfo.m_currentUFOSpawnCount > 0)
        {
            return;
        }

        var buffer = m_entityManager.GetBuffer<EntityBufferElement>(m_UFOLibrary);
        var lengthOfBuffer = buffer.Length;
        var randomUFOIndex = Random.Range(0, lengthOfBuffer);
        var newUFO = m_entityManager.Instantiate(buffer[randomUFOIndex].m_entity);

        var randomSpawnPositionIndex = Random.Range(0, m_spawnPositionsVectors.Length);
        var spawnPosition = m_spawnPositionsVectors[randomSpawnPositionIndex];
        
        m_entityManager.SetComponentData(newUFO, new Translation()
        {
            Value = spawnPosition
        });

        var randomMoveDirection = math.normalize(new float3(Random.Range(-.8f, .8f), Random.Range(-.8f, .8f), 0));
        var randomRotation = math.normalize(new float3(0 , 0, Random.value));
        Debug.Log($"randomMove {randomMoveDirection}");
        m_entityManager.SetComponentData(newUFO, new Rotation()
        {
            Value = Quaternion.Euler(randomRotation.x,randomRotation.y, randomRotation.z)
        });

        m_entityManager.SetComponentData(newUFO, new MovementCommandsComponentData()
        {
            m_lastPosition = spawnPosition,
            m_currentDirection = randomMoveDirection,
            m_currentLinearCommand = 1,
            m_currentAngularCommand = randomRotation
        });
    }

    private void SpawnPickUp()
    {
        GameInfoComponentData gameInfo = m_entityManager.GetComponentData<GameInfoComponentData>(m_gameInfo);
        
        if (gameInfo.m_currentPickUpSpawnCount > 0)
        {
            return;
        }

        var buffer = m_entityManager.GetBuffer<EntityBufferElement>(m_pickupLibrary);
        var lengthOfBuffer = buffer.Length;
        var randomPickUpIndex = Random.Range(0, lengthOfBuffer);
        var newPickup = m_entityManager.Instantiate(buffer[randomPickUpIndex].m_entity);

        var randomSpawnPositionIndex = Random.Range(0, m_pickUpSpawnPositionsVectors.Length);
        var spawnPosition = m_pickUpSpawnPositionsVectors[randomSpawnPositionIndex];
        
        m_entityManager.SetComponentData(newPickup, new Translation()
        {
            Value = spawnPosition
        });
    }

    private void SpawnPlayer()
    {
        GameInfoComponentData gameInfo = m_entityManager.GetComponentData<GameInfoComponentData>(m_gameInfo);
        
        if (gameInfo.m_currentPlayerCount > 0)
        {
            return;
        }

        var buffer = m_entityManager.GetBuffer<EntityBufferElement>(m_playerLibrary);
        var lengthOfBuffer = buffer.Length;
        var randomPlayerIndex = Random.Range(0, lengthOfBuffer);
        var newPlayer = m_entityManager.Instantiate(buffer[randomPlayerIndex].m_entity);

        var randomSpawnPositionIndex = Random.Range(0, m_playerSpawnPositionsVectors.Length);
        var spawnPosition = m_playerSpawnPositionsVectors[randomSpawnPositionIndex];
        
        m_entityManager.SetComponentData(newPlayer, new Translation()
        {
            Value = spawnPosition
        });
    }

    private void Update()
    {
        m_currentTimer += Time.deltaTime;
        m_UFOTimer += Time.deltaTime;
        m_PickUpTimer += Time.deltaTime;
        m_playerTimer += Time.deltaTime;
        
        if (m_playerTimer > m_playerSpawnFrequency)
        {
            m_playerTimer =  0;
            SpawnPlayer();
        }
        
        if (m_currentTimer > m_asteroidSpawnFrequency)
        {
            m_currentTimer = 0;
            SpawnAsteroid();
        }

        if (m_UFOTimer > m_UFOSpawnFrequency)
        {
            m_UFOTimer = 0;
            SpawnUFO();
        }

        if (m_PickUpTimer > m_pickUpSpawnFrequency)
        {
            m_PickUpTimer = 0;
            SpawnPickUp();
        }

    }
}
