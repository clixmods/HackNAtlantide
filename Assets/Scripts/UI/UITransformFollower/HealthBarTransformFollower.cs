using UnityEngine;

namespace UI.UITransformFollower
{
    public class HealthBarTransformFollower : MonoBehaviour
    {
        [SerializeField] private Sprite _iconObject;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float maxDistanceToShow = 45;
        [Tooltip("If is true, the UI Element is not connected with the manipulation of this component")]
        [SerializeField] private bool _UIElementIsIndependant;
        private UIHealthBarTransformFollower _uiHealthBar;
        // Start is called before the first frame update
        void Start()
        {
            _uiHealthBar = UITransformFollower.Create<UIHealthBarTransformFollower>(_prefab, transform , maxDistanceToShow);
            _uiHealthBar.Init(GetComponent<IDamageable>());
        }

        private void OnDisable()
        {
            if(!_UIElementIsIndependant && _uiHealthBar != null)
                _uiHealthBar.gameObject.SetActive(false);
        }
        private void OnEnable()
        {
            if(!_UIElementIsIndependant && _uiHealthBar != null)
                _uiHealthBar.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            if(!_UIElementIsIndependant && _uiHealthBar != null)
                Destroy(_uiHealthBar.gameObject);
        }
    }
}