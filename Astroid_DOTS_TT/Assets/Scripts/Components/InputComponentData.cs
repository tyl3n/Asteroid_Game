using Unity.Entities;

public struct InputComponentData : IComponentData
{
    public bool inputLeft;
    public bool inputRight;
    public bool inputForward;
    public bool inputShoot;
}
