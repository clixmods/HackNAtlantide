using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LabyrintheTrigger : MonoBehaviour
{
    public UnityEvent OnBallEnter;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<BallInteractable>(out BallInteractable ball))
        {
            OnBallEnter?.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}
