using UnityEngine;
using UnityEngine.Events;
public class MonoBehaviourUnityEvent : MonoBehaviour
{
   public UnityEvent EventAwake;
   public UnityEvent EventUpdate;
   public UnityEvent EventDestroy;

   private void Awake()
   {
      EventAwake?.Invoke();
   }

   private void Update()
   {
      EventUpdate?.Invoke();
   }

   private void OnDestroy()
   {
      EventDestroy?.Invoke();
   }
}
