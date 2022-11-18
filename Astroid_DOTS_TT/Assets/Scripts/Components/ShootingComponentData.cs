using Unity.Entities;

[GenerateAuthoringComponent]
public struct ShootingComponentData : IComponentData
{
    public Entity projectilePrefab;
    public bool isFiring;
    public float fireRate;
    public float timer;
}

