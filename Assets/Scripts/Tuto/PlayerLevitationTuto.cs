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
            /*case InputType.Interact:
                StartCoroutine(WaitForNewQTE());
                this.enabled = false;*/
                //break;
            case InputType.Focus:
                _hasDoneFocusQte = true;
                _qTEHandler.enabled = false;
                this.enabled = false;
                
                break;
            default:
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
        if (!_hasDoneFocusQte)
        {
            if (_focus != null && _focus.CurrentTarget != null && !_isInCutScene)
            {
                _qTEHandler.LaunchCutScene(InputType.Focus);
                _isInCutScene = true;
            }
        }
            
        /*if(interactDetection.ClosestInteractable() != null && _hasDoneFocusQte)
        {
            _qTEHandler.LaunchCutScene(InputType.Interact);
            _isInCutScene = true;
        }*/

    }
}
