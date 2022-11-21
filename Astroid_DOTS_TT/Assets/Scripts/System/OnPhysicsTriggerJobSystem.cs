 using Unity.Collections;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
 using UnityEngine;
 
 [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
 [UpdateAfter(typeof(ExportPhysicsWorld))]
 [UpdateBefore(typeof(EndFramePhysicsSystem))]
 public partial class OnPhysicsTriggerJobSystem : SystemBase
 {
    private BuildPhysicsWorld m_physicsWorld;
    private StepPhysicsWorld m_stepPhysicsWorld;

    [Unity.Burst.BurstCompile]
    private struct TriggerJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<DestroyableComponentData> m_destroyables;

        public void Execute ( TriggerEvent triggerEvent )
        {
       if (m_destroyables.HasComponent(triggerEvent.EntityA))
        {
            var destroyable = m_destroyables[triggerEvent.EntityA];
            destroyable.m_mustBeDestroyed = true;
            m_destroyables[triggerEvent.EntityA] = destroyable;
            }
        if (m_destroyables.HasComponent(triggerEvent.EntityB))
        {
            var destroyable = m_destroyables[triggerEvent.EntityB];
            destroyable.m_mustBeDestroyed = true;
            m_destroyables[triggerEvent.EntityB] = destroyable;
        }
        }
    }

    protected override void OnCreate()
    {
        base.OnCreate();
        m_physicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
        m_stepPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>();
    }
     protected override void OnStartRunning()
     {
         base.OnStartRunning();
         this.RegisterPhysicsRuntimeSystemReadOnly();
     }
     protected override void OnUpdate ()
     {
            var destroyables = GetComponentDataFromEntity<DestroyableComponentData>();

            var job = new TriggerJob
            {
                m_destroyables = destroyables
            };

            var jobHandle = job.Schedule(m_stepPhysicsWorld.Simulation, Dependency);
            jobHandle.Complete();
     }
 }