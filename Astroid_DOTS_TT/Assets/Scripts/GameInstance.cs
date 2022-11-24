using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
public class GameInstance : MonoBehaviour
{
    public Entity m_asteroidLibrary;
    public Entity m_UFOLibrary;
    public Entity m_pickupLibrary;
    public Entity m_playerLibrary;

    public Entity m_gameInfoEntity;
    public Entity m_inputInfoEntity;
    private EntityManager m_entityManager;
    private World  m_world;

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
    
    public int m_maxAsteroidSpawnCount = 10;
    public int m_maxUFOSpawnCount = 1;
    public int m_maxPickUpSpawnCount = 10;

    private void Awake()
    {
       
        if (World.DefaultGameObjectInjectionWorld == null)
        {
            DefaultWorldInitialization.Initialize("Base World", false);
        }

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
        var gameParamQuery = m_entityManager.CreateEntityQuery(typeof(GameParamsComponentData));
        var gameParamArray = gameParamQuery.ToComponentDataArray<GameParamsComponentData>(Allocator.TempJob);

        if (gameParamArray.Length == 0 || gameParamArray.Length>1)
        {
            gameParamArray.Dispose();
            return;
        }
        
        var gameParams = gameParamArray[0];
        
        m_inputInfoEntity = m_entityManager.CreateEntity(typeof(InputComponentData));
        m_gameInfoEntity = m_entityManager.CreateEntity(typeof(GameInfoComponentData));
    
        m_entityManager.SetComponentData(m_gameInfoEntity,new GameInfoComponentData
        {
            m_life = gameParams.m_startLife,
            m_score = 0
        });
        gameParamArray.Dispose();

    }

    private void SpawnAsteroid()
    {
        if (m_gameInfoEntity == null)
        {
            return;
        } 
        
        GameInfoComponentData gameInfo = m_entityManager.GetComponentData<GameInfoComponentData>(m_gameInfoEntity);
        
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
            m_currentDirection = randomMoveDirection,
            m_currentLinearCommand = 1,
            m_currentAngularCommand = randomRotation
        });
    }

     private void SpawnUFO()
    {
         if (m_gameInfoEntity == null)
        {
            return;
        } 
        
        GameInfoComponentData gameInfo = m_entityManager.GetComponentData<GameInfoComponentData>(m_gameInfoEntity);
        
        if (gameInfo.m_currentUFOSpawnCount >= m_maxUFOSpawnCount)
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
        if (m_gameInfoEntity == null)
        {
            return;
        } 

        GameInfoComponentData gameInfo = m_entityManager.GetComponentData<GameInfoComponentData>(m_gameInfoEntity);

        if (gameInfo.m_currentPickUpSpawnCount >= m_maxPickUpSpawnCount)
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
        if (m_gameInfoEntity == null)
        {
            return;
        } 
        
        GameInfoComponentData gameInfo = m_entityManager.GetComponentData<GameInfoComponentData>(m_gameInfoEntity);

        if (gameInfo.m_currentPlayerCount > 0)
        {
            return;
        }

        var buffer = m_entityManager.GetBuffer<EntityBufferElement>(m_playerLibrary);
        var lengthOfBuffer = buffer.Length;
        var randomPlayerIndex = Random.Range(0, lengthOfBuffer);
        var newPlayer = m_entityManager.Instantiate(buffer[randomPlayerIndex].m_entity);

        var spawnPosition = gameInfo.m_nextSpawnPoint;
        
        m_entityManager.SetComponentData(newPlayer, new Translation()
        {
            Value = spawnPosition
        });
    }
    

    private void CheckGameOverCondition()
    {
        if (m_gameInfoEntity == null)
        {
            return;
        } 
        
        GameInfoComponentData gameInfo = m_entityManager.GetComponentData<GameInfoComponentData>(m_gameInfoEntity);

        if (gameInfo.m_life <= 0)
        {
            World.DefaultGameObjectInjectionWorld.QuitUpdate = true;
            World.DefaultGameObjectInjectionWorld.EntityManager.CompleteAllJobs();
            World.DefaultGameObjectInjectionWorld.EntityManager.GetAllEntities().Dispose();
            World.DefaultGameObjectInjectionWorld.Dispose();

            SceneManager.LoadScene(1);
        }
    
    }
    private void Update()
    {
        if (World.DefaultGameObjectInjectionWorld == null)
        {
            return;
        }

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

        
        CheckGameOverCondition();

    }
}
