using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableTrailLinker : MonoBehaviour
{
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform interactableTransform;
    [SerializeField] private float speedMultiplier;

    public void SetTransformsLink(Transform transform1, Transform transform2, float speed = 12)
    {
        startTransform = transform1;
        interactableTransform = transform2;
        speedMultiplier = speed;
    }
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
        // prevention
        if (interactableTransform == null)
        {
            Unselect();
            return;
        }
        transform.position = Vector3.Lerp(startTransform.position, interactableTransform.position, ( ( Mathf.Cos(Time.time*speedMultiplier)) + 1f) /2f  );
    }
}
