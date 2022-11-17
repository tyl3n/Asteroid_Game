
using Unity.Entities;
using UnityEngine;

public partial class InputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref InputComponentData input) =>
        {
            input.inputLeft = Input.GetKey(KeyCode.A);
            input.inputRight = Input.GetKey(KeyCode.D);
            input.inputForward = Input.GetKey(KeyCode.W);
            input.inputShoot = Input.GetMouseButton(0);
            
        }).Run();
    }
}
