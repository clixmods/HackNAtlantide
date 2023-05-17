using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevitationTuto : MonoBehaviour
{
    private QTEHandler _qTEHandler;
    private Focus _focus;

    bool _hasDoneFocusQte = false;
    bool _hasDoneLevitationQte;

    bool _isInCutScene;

    [SerializeField] PlayerInteractDetection interactDetection;
    void CutSceneSuccess(InputType inputType)
    {
        switch (inputType)
        {
            case InputType.Interact:
                StartCoroutine(WaitForNewQTE());
                break;
            case InputType.Focus:
                StartCoroutine(WaitForNewQTE());
                _hasDoneFocusQte = true;
                break;
        }
    }
    IEnumerator WaitForNewQTE()
    {
        _isInCutScene = true;
        yield return new WaitForSeconds(1f);
        _isInCutScene = false;
    }
    private void Awake()
    {
        _focus = FindObjectOfType<Focus>();
        _qTEHandler = FindObjectOfType<QTEHandler>();
    }
    private void OnEnable()
    {
        GameStateManager.Instance.tutoStateObject.SetActive(true);
        _qTEHandler.cutSceneSuccess += CutSceneSuccess;
    }
    private void OnDisable()
    {
        GameStateManager.Instance.tutoStateObject.SetActive(false);
        _qTEHandler.cutSceneSuccess -= CutSceneSuccess;
    }
    private void Update()
    {
        if(interactDetection.ClosestInteractable() != null)
        {
            Debug.Log("launchtutolevit");
        }
    }
}
