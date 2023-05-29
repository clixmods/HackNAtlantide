using UnityEngine;
using UnityEngine.EventSystems;

namespace AudioAliase
{
    public class UIButtonPlayAlias : MonoBehaviour ,IMoveHandler,
        IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler,
        ISelectHandler, IDeselectHandler,IPointerClickHandler
    {
        [Header("Alias Click")]
         [SerializeField] private Alias aliasOnClick;
        [Header("Alias Navigation")]
       [SerializeField] private Alias aliasOnMove;
        [SerializeField] private Alias aliasOnPointerDown;
         [SerializeField] private Alias aliasOnPointerUp;
       [SerializeField] private Alias aliasOnPointerEnter;
        [SerializeField] private Alias aliasOnPointerExit;
         [SerializeField] private Alias aliasOnSelect;
         [SerializeField] private Alias aliasOnDeselect;
        public void OnMove(AxisEventData eventData)
        {
            AudioManager.PlaySoundAtPosition(aliasOnMove);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            AudioManager.PlaySoundAtPosition(aliasOnPointerDown);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            AudioManager.PlaySoundAtPosition(aliasOnPointerUp);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.PlaySoundAtPosition(aliasOnPointerEnter);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            AudioManager.PlaySoundAtPosition(aliasOnPointerExit);
        }
        public void OnSelect(BaseEventData eventData)
        {
            AudioManager.PlaySoundAtPosition(aliasOnSelect);
        }
        public void OnDeselect(BaseEventData eventData)
        {
            AudioManager.PlaySoundAtPosition(aliasOnDeselect);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.PlaySoundAtPosition(aliasOnClick);
        }
    }
}