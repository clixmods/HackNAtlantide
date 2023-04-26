using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public List<AttackSO> combo;
    float lastClickedTime;
    float lastComboEnd;
    int comboCounter;

    Animator anim;
    [SerializeField] Weapon weapon;

    [SerializeField] private InputButtonScriptableObject _inputAttack;

    public bool CanGiveDamage { get { return _canGiveDamage; } }
    private bool _canGiveDamage;

    private void OnEnable()
    {
        _inputAttack.OnValueChanged += Attack;
    }
    private void OnDisable()
    {
        _inputAttack.OnValueChanged -= Attack;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Attack(bool value)
    {
        if (value)
        {
            if (Time.time - lastComboEnd > 0.5f && comboCounter <= combo.Count)
            {
                CancelInvoke("EndCombo");

                if (Time.time - lastClickedTime >= 0.2f)
                {
                    anim.runtimeAnimatorController = combo[comboCounter].animatorOV;
                    anim.Play("Attack", 0, 0);
                    weapon.damage = combo[comboCounter].damage;
                    comboCounter++;
                    lastClickedTime = Time.time;

                    if (comboCounter + 1 > combo.Count)
                    {
                        comboCounter = 0;
                    }
                }
            }
            ExitAttack();
        }
    }

    void ExitAttack()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo", 1);
        }
    }

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
    }

    public void SetDamageActive(int value)
    {
        _canGiveDamage = value == 1;
    }
}