using UnityEngine;


    public class Targetable : MonoBehaviour , ITargetable
    {
        [SerializeField] private bool canBeTarget;
        public bool CanBeTarget => canBeTarget;
        public void OnTarget()
        {
            Debug.Log("Object is target",gameObject);
        }

        public void OnUntarget()
        {
            Debug.Log("Object is untarget", gameObject);
        }
    }
