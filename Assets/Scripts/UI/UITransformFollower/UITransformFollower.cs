using UnityEngine;

namespace UI.UITransformFollower
{
    /// <summary>
    /// Abstract class used for UI element, give the behavior to follow a given tranform
    /// </summary>
    public abstract class UITransformFollower : MonoBehaviour
    {
        private Transform _transformToFollow;
        private Vector3 _offset;
        private Vector2 _initialScale;
        
        private static UIFollowerContainer _followerContainer;
        [Header("Settings")]
        [SerializeField] private float distanceToShow = 10;

        [SerializeField] private bool updateScale;
        [SerializeField] private bool updateOpacity;
        public virtual void Create()
        {
            
        }
        public static T Create<T>(GameObject prefab, Transform transformTarget, float maxDistanceToShow) where T : UITransformFollower
        {
            T component = null;
            if (transformTarget == null) return null;

            if (_followerContainer == null)
            {
                _followerContainer = FindObjectOfType<UIFollowerContainer>();
            }
            var inputHelperObject = Instantiate(prefab, Vector3.zero, Quaternion.identity, _followerContainer.transform);
            // Setup
            component = inputHelperObject.GetComponentInChildren<T>();
            component._transformToFollow = transformTarget;
            component.distanceToShow = maxDistanceToShow;
            component.Create();
            return component;
        }
        
         // Update is called once per frame
            void Update()
            {
                UpdatePosition();
               // UpdateScale();
                UpdateOpacity();
            }
            private void UpdateOpacity()
            {
                var distance = Vector3.Distance(PlayerInstanceScriptableObject.Player.transform.position, _transformToFollow.position);
            }
            private void UpdateScale()
            {
                var distance = Vector3.Distance(PlayerInstanceScriptableObject.Player.transform.position, _transformToFollow.position);
                var calcul = distance / distanceToShow ;
                var clampT = Mathf.Clamp(calcul, 0, 1);
                ((RectTransform) transform).sizeDelta = Vector2.Lerp(_initialScale , _initialScale * 0.5f, clampT);
            }
        
            void UpdatePosition()
            {
                if(_transformToFollow == null)
                {
                    transform.position = new Vector3(-10000,0,-100);
                    return;
                }
                Vector3 position = _transformToFollow.position.GetPositionInWorldToScreenPoint();
                if(_transformToFollow.position.IsOutOfCameraVision() )
                {
                    transform.position = new Vector3(-10000,0,0);
                }
                else
                {
                    var positionInCanvas = new Vector3(position.x, position.y, 0);
                    transform.position = positionInCanvas + _offset;
                }
            }
    }
}