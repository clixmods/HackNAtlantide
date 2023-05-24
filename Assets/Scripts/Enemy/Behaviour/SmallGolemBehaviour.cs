using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallGolemBehaviour : EnemyBehaviour
{
    private void Start()
    {
        StartCoroutine(Attack());
    }
}
