using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableTrailLinker : MonoBehaviour
{
    public Transform playerTransform;
    [FormerlySerializedAs("interactable")] public Transform interactableTransform;
    public float speedMultiplier;

    public void SetInteractable(IInteractable interactable)
    {
        this.interactableTransform = interactable.transform;
        gameObject.SetActive(true);
    }
    public void Unselect()
    {
        this.interactableTransform = null;
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(playerTransform.position, interactableTransform.position, ( ( Mathf.Cos(Time.time*speedMultiplier)) + 1f) /2f  );
    }
}
