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
        [Aliase] [SerializeField] private int aliasOnClick;
        [Header("Alias Navigation")]
        [Aliase] [SerializeField] private int aliasOnMove;
        [Aliase] [SerializeField] private int aliasOnPointerDown;
        [Aliase] [SerializeField] private int aliasOnPointerUp;
        [Aliase] [SerializeField] private int aliasOnPointerEnter;
        [Aliase] [SerializeField] private int aliasOnPointerExit;
        [Aliase] [SerializeField] private int aliasOnSelect;
        [Aliase] [SerializeField] private int aliasOnDeselect;
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