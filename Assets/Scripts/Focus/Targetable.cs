using System;
using UnityEngine;


    public class Targetable : MonoBehaviour , ITargetable
    {
        [SerializeField] private Transform targeter;
        [SerializeField] private bool useDistance;
        [SerializeField] private float maxDistanceWithTargeter = 50f;
        [SerializeField] private bool canBeTarget;
        public bool CanBeTarget
        {
            get
            {
                if (canBeTarget)
                {
                    if (useDistance && targeter != null)
                    {
                        if (Vector3.Distance(targeter.position, transform.position) < maxDistanceWithTargeter)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return canBeTarget;
                }

                return false;

            }
        }

        private void Awake()
        {
            if (targeter == null)
            {
                targeter = PlayerInstanceScriptableObject.Player.transform;
            }
            
        }

        public void OnTarget()
        {
            Debug.Log("Object is target",gameObject);
        }

        public void OnUntarget()
        {
            Debug.Log("Object is untarget", gameObject);
        }
    }
