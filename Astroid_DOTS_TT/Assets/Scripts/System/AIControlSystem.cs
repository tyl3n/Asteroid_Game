using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class AIControlSystem : SystemBase
{
    private EntityManager m_entityManager;

    private EndSimulationEntityCommandBufferSystem m_endSimulationEntityCommandBufferSystem;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        m_entityManager = World.EntityManager;
        m_endSimulationEntityCommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {

        var deltaTime = Time.DeltaTime;

        Entities.WithAll<UFOTagComponent>().
            ForEach((ref MovementCommandsComponentData _movementCommandsComponentData,ref ShootingComponentData _weaponComponent, ref AIControlComponentData _AIControlComponent) =>
        {

            
            _AIControlComponent.m_shootTimer += deltaTime;
            _AIControlComponent.m_directionChangeTimer += deltaTime;

            if (_AIControlComponent.m_directionChangeTimer > _AIControlComponent.m_directionChangeTime)
            {
                var randomMoveDirection = math.normalize(new float3(Random.Range(-.8f, .8f), Random.Range(-.8f, .8f), 0));
                var randomRotation = math.normalize(new float3(0 , 0, Random.value));

                _movementCommandsComponentData.m_currentDirection = randomMoveDirection;
                _movementCommandsComponentData.m_currentAngularCommand = randomRotation;
                _movementCommandsComponentData.m_currentLinearCommand = 1;
                _AIControlComponent.m_directionChangeTimer = 0;
            } 

            if (_AIControlComponent.m_shootTimer > _AIControlComponent.m_shootTime)
            {
                _weaponComponent.m_isFiring = true;
                _AIControlComponent.m_shootTimer = 0.0f;
            }
            else 
            {
                _weaponComponent.m_isFiring = false;
            }

        }).Run();
    }
}
