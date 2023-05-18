using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableTrailLinker : MonoBehaviour
{
    [SerializeField] private Transform startTransform;
    private Transform _interactableTransform;
    [SerializeField] private float speedMultiplier;

    public void SetInteractable(IInteractable interactable)
    {
        this._interactableTransform = interactable.transform;
        gameObject.SetActive(true);
    }
    public void Unselect()
    {
        this._interactableTransform = null;
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(startTransform.position, _interactableTransform.position, ( ( Mathf.Cos(Time.time*speedMultiplier)) + 1f) /2f  );
    }
}
