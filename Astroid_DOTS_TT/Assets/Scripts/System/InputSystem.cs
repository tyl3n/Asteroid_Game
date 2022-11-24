using Unity.Entities;
using UnityEngine;

public  partial class InputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref InputComponentData _input) =>
        {
            _input.m_inputLeft = Input.GetKey(KeyCode.A);
            _input.m_inputRight = Input.GetKey(KeyCode.D);
            _input.m_inputForward = Input.GetKey(KeyCode.W);
            _input.m_inputShoot = Input.GetKey(KeyCode.P);
            _input.m_inputHyperspace = Input.GetKey(KeyCode.O);
            
        }).Run();
    }
}

