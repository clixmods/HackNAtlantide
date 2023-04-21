using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerAnimation : MonoBehaviour
{
    private Animation _animation;
    [SerializeField] private AnimationClip animationTowerA;
    [SerializeField] private AnimationClip animationTowerB;
    public UnityEvent OnAnimationStart;
    public UnityEvent OnAnimationStop;
    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _animation.AddClip(animationTowerA,"AnimationTowerA");
        _animation.AddClip(animationTowerB,"AnimationTowerB");
    }
    public void PlayAnimationTowerA()
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

    public void PlayAnimationTowerB()
    {
        this.enabled = true;
        OnAnimationStart?.Invoke();
        _animation.clip = animationTowerB;
        _animation.Play();
    }
}
