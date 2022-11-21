using Unity.Entities;
using Unity.Transforms;

public partial class ShootingSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem m_endSimulationEntityCommandBufferSystem;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        m_endSimulationEntityCommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        var ecb = m_endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

        Entities.ForEach((Entity _entity, int entityInQueryIndex , ref ShootingComponentData _shootingComponent,
            in Translation _translation,
            in Rotation _rotation) =>
        {
            _shootingComponent.m_timer += deltaTime;
            
            if (!_shootingComponent.m_isFiring) 
            {
                //Debug.Log("Firing");
                return;
            }

            if ((_shootingComponent.m_timer <_shootingComponent.m_fireRate)) 
            {
                //Debug.Log("Timer to small");
                return;
            }
             //Debug.Log($"FIre !!");
            _shootingComponent.m_timer = 0;
            var newProjectile = ecb.Instantiate(entityInQueryIndex, _shootingComponent.m_projectilePrefab);

            ecb.SetComponent(entityInQueryIndex , newProjectile, new Translation
            {
                Value = _translation.Value
            });

            ecb.SetComponent(entityInQueryIndex , newProjectile, new Rotation()
            {
                Value = _rotation.Value
            });

            ecb.SetComponent(entityInQueryIndex , newProjectile, new MovementCommandsComponentData()
            {
                m_currentLinearCommand = 1,
            });


        }).ScheduleParallel();
        m_endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}



