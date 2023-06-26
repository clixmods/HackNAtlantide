using UnityEngine;
using UnityEngine.Events;
public class AnimationHandler : MonoBehaviour
{
    private Animation _animation;
    [SerializeField] private AnimationClip animationToPlay;
    public UnityEvent OnAnimationStart;
    public UnityEvent OnAnimationStop;
    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _animation.AddClip(animationToPlay,animationToPlay.name);
        _animation.clip = animationToPlay;
    }
    public void PlayAnimation()
    {
        this.enabled = true;
        OnAnimationStart?.Invoke();
        _animation.clip = animationToPlay;
        _animation.Play();
    }

    private void LateUpdate()
    {
        if (!_animation.isPlaying)
        {
            OnAnimationStop?.Invoke();
            this.enabled = false;
        }
    }
}