 using Unity.Collections;
 using Unity.Entities;
 using Unity.Jobs;
 using Unity.Physics;
 using Unity.Physics.Systems;
 using UnityEngine;

 public class TriggerWarning_Authoring : MonoBehaviour, IConvertGameObjectToEntity
 {
     [SerializeField] string _message = "How dare you!";
     void IConvertGameObjectToEntity.Convert ( Entity e , EntityManager dst , GameObjectConversionSystem cs )
     {
         if( !enabled ) return;
 
         dst.AddComponentData( e , new TriggerWarning{
             Message = _message
         } );
     }
 }
 
 public struct TriggerWarning : IComponentData
 {
     public FixedString4096Bytes Message;
 }
 
