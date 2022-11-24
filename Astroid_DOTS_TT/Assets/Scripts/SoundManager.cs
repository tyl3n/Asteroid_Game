using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Collections;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public GameInstance m_gameInstace;
    public AudioClip m_explosionSound;
    public AudioClip m_laserSound;
    public AudioClip m_boosterSound;
    public AudioSource m_boosterSource;
    public AudioSource m_laserSource;
    public AudioSource m_explosionSource;
    private EntityManager m_entityManager;
    
    // Start is called before the first frame update
    void Start()
    {
        m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    // Update is called once per frame
    void Update()
    {
        PlayThrustAudio();
        PlayLaserAudio();
        PlayExplosionAudio();
    }

    private void PlayExplosionAudio()
    {
       EntityQueryDesc description = new EntityQueryDesc
       {
           All = new ComponentType[] { typeof(DestroyableComponentData), typeof(PlayExplosionSoundTagComponent) }
       };
        var query = m_entityManager.CreateEntityQuery(description);
        var array = query.ToComponentDataArray<DestroyableComponentData>(Allocator.TempJob);

        Debug.Log($" destroyable Length {array.Length}");
        if (array.Length == 0)
        {
            array.Dispose();
            return;
        }

        bool shouldPlayExplosion = false;

        for (int i = 0;i<array.Length;++i)
        {
            var _destroyableComponentData = array[i];
            if (_destroyableComponentData.m_mustBeDestroyed)
            {
                shouldPlayExplosion = true;
            }
            
        }

        if(shouldPlayExplosion && !m_explosionSource.isPlaying)
        {
            m_explosionSource.PlayOneShot(m_explosionSound);
        }
        array.Dispose();
    }

    private void PlayThrustAudio()
    {
        InputComponentData inputInfo = m_entityManager.GetComponentData<InputComponentData>(m_gameInstace.m_inputInfoEntity);
        if (inputInfo.m_inputForward && !m_boosterSource.isPlaying)
        {
            m_boosterSource.clip = m_boosterSound;
            m_boosterSource.Play();
        }
        else
        {
            m_boosterSource.Stop();
        }
    }

    private void PlayLaserAudio()
    {
        var query = m_entityManager.CreateEntityQuery(typeof(ShootingComponentData));
        var array = query.ToComponentDataArray<ShootingComponentData>(Allocator.TempJob);

        if (array.Length == 0)
        {
            array.Dispose();
            return;
        }
        
        bool shouldPlayFiring = false;

        for (int i = 0;i<array.Length;++i)
        {
            var shootingComp = array[i];
            if(shootingComp.m_isFiring)
            {
                shouldPlayFiring = true;
            }
        }

        if(shouldPlayFiring && !m_laserSource.isPlaying)
        {
            m_laserSource.PlayOneShot(m_laserSound);
        }
        array.Dispose();
    }
}
