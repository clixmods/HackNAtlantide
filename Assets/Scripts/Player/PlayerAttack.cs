using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] InputButtonScriptableObject _interact;

    [SerializeField] private float cooldownTime = 2.0f;
    public static int numberOfClicks = 0;

    private float nextFireTime = 0f;
    private float lastClickedTime = 0f;
    private float maxComboDelay = 1f;


    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        _interact.OnValueChanged += InteractInput;
    }
    private void OnDisable()
    {
        _interact.OnValueChanged -= InteractInput;
    }

    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.25f && _animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            _animator.SetBool("hit1", false);
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.25f && _animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            _animator.SetBool("hit2", false);
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.25f && _animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            _animator.SetBool("hit3", false);
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.25f && _animator.GetCurrentAnimatorStateInfo(0).IsName("hit4"))
        {
            _animator.SetBool("hit4", false);
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            numberOfClicks = 0;
        }
    }

    void InteractInput(bool value)
    {
        if (Time.time > nextFireTime)
        {
            OnClick();
        }
    }

    void OnClick()
    {
        lastClickedTime = Time.time;
        numberOfClicks++;
        if (numberOfClicks == 1)
        {
            _animator.SetBool("hit1", true);
        }
        numberOfClicks = Mathf.Clamp(numberOfClicks, 0, 4);

        if (numberOfClicks >= 2 && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.25f && _animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            _animator.SetBool("hit1", false);
            _animator.SetBool("hit2", true);
        }
        if (numberOfClicks >= 3 && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.25f && _animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            _animator.SetBool("hit2", false);
            _animator.SetBool("hit3", true);
        }
        if (numberOfClicks >= 4 && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.25f && _animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            _animator.SetBool("hit3", false);
            _animator.SetBool("hit4", true);
        }
    }
}