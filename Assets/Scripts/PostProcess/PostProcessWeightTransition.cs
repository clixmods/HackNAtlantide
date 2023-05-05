
using UnityEngine;
using UnityEngine.Rendering;
public class PostProcessWeightTransition : MonoBehaviour
{
    private Volume _volume;

    private float _weightTarget;
    [SerializeField] private float _multiplierSpeed = 3;

    public void SetWeightVolume(float value)
    {
        _weightTarget = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        _volume = GetComponent<Volume>();
    }
    // Update is called once per frame
    void Update()
    {
        _volume.weight = Mathf.Clamp(Mathf.Lerp(_volume.weight, _weightTarget, Time.unscaledDeltaTime * _multiplierSpeed),0,1);
    }
}
