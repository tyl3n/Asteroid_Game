using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class LifeWriter : MonoBehaviour
{   
    private EntityManager m_entityManager;
    public Text m_text;
    public GameInstance m_gameInstance;
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (World.DefaultGameObjectInjectionWorld == null)
        {
            return;
        } 
        if (m_gameInstance.m_gameInfoEntity == null)
        {
            return;
        } 
        
        GameInfoComponentData gameInfo = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<GameInfoComponentData>(m_gameInstance.m_gameInfoEntity);
        m_text.text = gameInfo.m_life.ToString();
        
        
    }
}
