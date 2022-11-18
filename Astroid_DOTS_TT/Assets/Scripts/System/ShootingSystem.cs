using Unity.Entities;
using Unity.Transforms;


public partial class ShootingSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        endSimulationEntityCommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        var ecb = endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

        Entities.ForEach((Entity _entity, int entityInQueryIndex , ref ShootingComponentData shootingComponent,
            in Translation translation,
            in Rotation rotation) =>
        {
            shootingComponent.timer += deltaTime;
            
            if (!shootingComponent.isFiring) return;
            if (!(shootingComponent.timer > shootingComponent.fireRate)) return;
            
            shootingComponent.timer = 0;
            var newProjectile = ecb.Instantiate(entityInQueryIndex, shootingComponent.projectilePrefab);

            ecb.SetComponent(entityInQueryIndex , newProjectile, new Translation
            {
                Value = translation.Value
            });

            ecb.SetComponent(entityInQueryIndex , newProjectile, new Rotation()
            {
                Value = rotation.Value
            });

            ecb.SetComponent(entityInQueryIndex , newProjectile, new MovementControlComponentData()
            {
                currentLinearMovement = 1,
            });

            ecb.AddComponent(entityInQueryIndex, newProjectile, new ParticleEffectLink()
            {
                value = 1
            });

        }).ScheduleParallel();
        endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}



public struct ParticleEffectLink : IComponentData
{
    public int value;
}


public struct EffectIDSystemState : IComponentData
{
}