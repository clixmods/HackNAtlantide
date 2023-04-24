using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    [SerializeField] Image greenbar;
    [SerializeField] Image bluebar;
    [SerializeField] Image redbar;
    [SerializeField] Health health;

    float MaxHP;

    private void Start()
    {
        MaxHP = health.HP;
        redbar.fillAmount = MaxHP / 100;
    }

    // Update is called once per frame
    void Update()
    {
        greenbar.fillAmount = health.HP / 100;
        bluebar.fillAmount = (health.HP - 100) /100;
    }
}
