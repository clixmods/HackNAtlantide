using System;
using UnityEngine;

namespace UI.UITransformFollower
{
    public class HealthBarTransformFollower : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float maxDistanceToShow = 45;
        [Tooltip("UI Element is connected with the manipulation of this component")]
        [SerializeField] private bool _UIElementIsIndependant;
        private UIHealthBarTransformFollower _uiHealthBar;
        private IDamageable _idamageable;
        private IFocusable _iFocusable;
        private void Awake()
        {
            _idamageable = GetComponent<IDamageable>();
            _idamageable.OnDamage += IdamageableOnOnDamage;
            _iFocusable = GetComponent<IFocusable>();
            _iFocusable.OnTargeted += FocusableOnOnTargeted; 
            _iFocusable.OnUntargeted += FocusableOnOnUntargeted;
        }

        private void FocusableOnOnUntargeted(object sender, EventArgs e)
        {
            if(_idamageable.health.Equals(_idamageable.maxHealth))
                enabled = false;
        }

        private void FocusableOnOnTargeted(object sender, EventArgs e)
        {
            enabled = true;
        }

        private void IdamageableOnOnDamage(object sender, EventArgs e)
        {
            enabled = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            _uiHealthBar = UITransformFollower.Create<UIHealthBarTransformFollower>(_prefab, transform , maxDistanceToShow);
            _uiHealthBar.Init(_idamageable);
            enabled = false;
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