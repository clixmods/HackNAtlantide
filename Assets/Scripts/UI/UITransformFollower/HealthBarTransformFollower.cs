using System;
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
        private IDamageable _idamageable;
        private ITargetable _iTargetable;
        private void Awake()
        {
            _idamageable = GetComponent<IDamageable>();
            _idamageable.OnDamage += IdamageableOnOnDamage;
            _iTargetable = GetComponent<ITargetable>();
            _iTargetable.OnTargeted += ITargetableOnOnTargeted; 
            _iTargetable.OnUntargeted += ITargetableOnOnUntargeted;
        }

        private void ITargetableOnOnUntargeted(object sender, EventArgs e)
        {
            if(_idamageable.health.Equals(_idamageable.maxHealth))
                enabled = false;
        }

        private void ITargetableOnOnTargeted(object sender, EventArgs e)
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