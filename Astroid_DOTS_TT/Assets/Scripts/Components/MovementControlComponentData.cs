using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MovementControlComponentData : IComponentData
{
    public float3 currentMovementDirection;
    public float3 currentAngularMovement;
    public float currentLinearMovement;
    public float3 previousPos;
}
