using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class GameInstance : MonoBehaviour
{
    private EntityManager entityManager;

    private void Awake()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    // Start is called before the first frame update
    void Start()
    {
        entityManager.CreateEntity(typeof(InputComponentData));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
