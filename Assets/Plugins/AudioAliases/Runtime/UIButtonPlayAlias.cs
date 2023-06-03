using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AudioAliase
{
    public class UIButtonPlayAlias : MonoBehaviour, IMoveHandler,
        IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler,
        ISelectHandler, IDeselectHandler, IPointerClickHandler
    {
        [Header("Alias Click")] [SerializeField]
        private Alias aliasOnClick;
        

        [Header("Alias Navigation")] [SerializeField]
        private Alias aliasOnMove;

        [SerializeField] private Alias aliasOnPointerDown;
        [SerializeField] private Alias aliasOnPointerUp;
        [SerializeField] private Alias aliasOnPointerEnter;
        [SerializeField] private Alias aliasOnPointerExit;
        [SerializeField] private Alias aliasOnSelect;
        [SerializeField] private Alias aliasOnDeselect;

        private Button _button;

        private bool IsInteractable(PointerEventData eventData)
        {
            return _button.interactable 
                   && eventData.hovered.Contains(_button.gameObject) 
                   && eventData.eligibleForClick; 
        }
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Call);
        }

        private void Call()
        {
            AudioManager.PlaySoundAtPosition(aliasOnClick);
        }

        public void OnMove(AxisEventData eventData)
        {
            if(_button.interactable)
                AudioManager.PlaySoundAtPosition(aliasOnMove);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            
            if(IsInteractable(eventData))
                AudioManager.PlaySoundAtPosition(aliasOnPointerDown);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if(IsInteractable(eventData))
                AudioManager.PlaySoundAtPosition(aliasOnPointerUp);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(IsInteractable(eventData))
                AudioManager.PlaySoundAtPosition(aliasOnPointerEnter);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if(IsInteractable(eventData))
                AudioManager.PlaySoundAtPosition(aliasOnPointerExit);
        }
        public void OnSelect(BaseEventData eventData)
        {
            if(_button.interactable)
                AudioManager.PlaySoundAtPosition(aliasOnSelect);
        }
        public void OnDeselect(BaseEventData eventData)
        {
            if(_button.interactable)
                AudioManager.PlaySoundAtPosition(aliasOnDeselect);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            // if(IsInteractable(eventData) )
            //     AudioManager.PlaySoundAtPosition(aliasOnClick);
        }
    }
}