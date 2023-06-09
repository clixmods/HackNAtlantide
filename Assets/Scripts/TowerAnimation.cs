using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerAnimation : MonoBehaviour
{
    private Animation _animation;
    [SerializeField] private AnimationClip animationTowerA;
    public UnityEvent OnAnimationStart;
    public UnityEvent OnAnimationStop;
    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _animation.AddClip(animationTowerA,animationTowerA.name);
        _animation.clip = animationTowerA;
    }
    public void PlayAnimation()
    {
        this.enabled = true;
        OnAnimationStart?.Invoke();
        _animation.clip = animationTowerA;
        _animation.Play();
    }

    private void Update()
    {
        if (!_animation.isPlaying)
        {
            OnAnimationStop?.Invoke();
            this.enabled = false;
        }
    }
}
