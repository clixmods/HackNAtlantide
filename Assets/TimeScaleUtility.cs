using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeScaleUtility : ScriptableObject
{
    public static void SetTimeScale(float amount)
    {
        Time.timeScale = amount;
    }
}
