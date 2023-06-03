using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public enum RumblePattern
{
    Constant,
    Pulse,
    Linear
}

public class Rumbler : MonoBehaviour
{
    public static Rumbler instance;
    private PlayerInput _playerInput;
    private RumblePattern activeRumbePattern;
    private float rumbleDurration;
    private float pulseDurration;
    private float lowA;
    private float lowStep;
    private float highA;
    private float highStep;
    private float rumbleStep;
    private bool isMotorActive = false;
    private void Start()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this);

    }
    // Public Methods
    public void RumbleConstant(RumblerDataConstant rumblerDataConstant)
    {
        
        switch(Settings.Instance.RumblerIntensity)
        {
            case (RumblerIntensity.none):
                break;
            case (RumblerIntensity.low):
                ApplyValuesConstant(rumblerDataConstant, 0.1f);
                break;
            case (RumblerIntensity.mid):
                ApplyValuesConstant(rumblerDataConstant, 1f);
                break;
            case (RumblerIntensity.high):
                ApplyValuesConstant(rumblerDataConstant, 5f);
                break;
        }
        
    }
    public void RumbleConstant(float duration, float low, float high)
    {

        switch (Settings.Instance.RumblerIntensity)
        {
            case (RumblerIntensity.none):
                break;
            case (RumblerIntensity.low):
                ApplyValuesConstant(duration, low, high, 0.1f);
                break;
            case (RumblerIntensity.mid):
                ApplyValuesConstant(duration, low, high, 1f);
                break;
            case (RumblerIntensity.high):
                ApplyValuesConstant(duration, low, high, 5f);
                break;
        }

    }
    private void ApplyValuesConstant(float duration, float low, float high, float intensity)
    {
        activeRumbePattern = RumblePattern.Constant;
        lowA = low *intensity;
        highA = high*intensity;
        rumbleDurration = Time.unscaledTime + duration;
    }
    private void ApplyValuesConstant(RumblerDataConstant data, float intensity)
    {
        activeRumbePattern = RumblePattern.Constant;
        lowA = data.low * intensity;
        highA = data.high * intensity;
        rumbleDurration = Time.unscaledTime + data.duration;
    }

    public void RumblePulse(RumblerDataPulse rumblerDataPulse)
    {
        switch (Settings.Instance.RumblerIntensity)
        {
            case (RumblerIntensity.none):
                break;
            case (RumblerIntensity.low):
                ApplyValuesPulse(rumblerDataPulse, 0.1f);
                break;
            case (RumblerIntensity.mid):
                ApplyValuesPulse(rumblerDataPulse, 1f);
                break;
            case (RumblerIntensity.high):
                ApplyValuesPulse(rumblerDataPulse, 5f);
                break;
        }
        
    }
    public void RumblePulse( float duration, float burstTime, float low, float high)
    {
        switch (Settings.Instance.RumblerIntensity)
        {
            case (RumblerIntensity.none):
                break;
            case (RumblerIntensity.low):
                ApplyValuesPulse(duration, burstTime, low, high, 0.1f);
                break;
            case (RumblerIntensity.mid):
                ApplyValuesPulse(duration, burstTime, low, high, 1f);
                break;
            case (RumblerIntensity.high):
                ApplyValuesPulse(duration, burstTime, low, high, 5f);
                break;
        }

    }
    private void ApplyValuesPulse(RumblerDataPulse data, float intensity)
    {
        activeRumbePattern = RumblePattern.Pulse;
        lowA = data.low*intensity;
        highA = data.high*intensity;
        rumbleStep = data.burstTime;
        pulseDurration = Time.unscaledTime + data.burstTime;
        rumbleDurration = Time.unscaledTime + data.duration;
        isMotorActive = true;
        var g = GetGamepad();
        g?.SetMotorSpeeds(lowA, highA);
    }
    private void ApplyValuesPulse(float duration, float burstTime, float low, float high, float intensity)
    {
        activeRumbePattern = RumblePattern.Pulse;
        lowA = low * intensity;
        highA = high * intensity;
        rumbleStep = burstTime;
        pulseDurration = Time.unscaledTime + burstTime;
        rumbleDurration = Time.unscaledTime + duration;
        isMotorActive = true;
        var g = GetGamepad();
        g?.SetMotorSpeeds(lowA, highA);
    }

    public void RumbleLinear(float lowStart, float lowEnd, float highStart, float highEnd, float durration)
    {
        //if (Settings.UseRumbler)
        {
            activeRumbePattern = RumblePattern.Linear;
            lowA = lowStart;
            highA = highStart;
            lowStep = (lowEnd - lowStart) / durration;
            highStep = (highEnd - highStart) / durration;
            rumbleDurration = Time.unscaledTime + durration;
        }
    }

    public void StopRumble()
    {
        var gamepad = GetGamepad();
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0, 0);
        }
    }


    // Unity MonoBehaviors
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        var gamepad = GetGamepad();
        if (Time.unscaledTime > rumbleDurration)
        {
            StopRumble();
            return;
        }

        if (gamepad == null)
            return;

        switch (activeRumbePattern)
        {
            case RumblePattern.Constant:
                gamepad.SetMotorSpeeds(lowA, highA);
                break;

            case RumblePattern.Pulse:

                if (Time.unscaledTime > pulseDurration)
                {
                    isMotorActive = !isMotorActive;
                    pulseDurration = Time.unscaledTime + rumbleStep;
                    if (!isMotorActive)
                    {
                        gamepad.SetMotorSpeeds(0, 0);
                    }
                    else
                    {
                        gamepad.SetMotorSpeeds(lowA, highA);
                    }
                }

                break;
            case RumblePattern.Linear:
                gamepad.SetMotorSpeeds(lowA, highA);
                lowA += (lowStep * Time.unscaledDeltaTime);
                highA += (highStep * Time.unscaledDeltaTime);
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        StopRumble();
    }

    // Private helpers

    private Gamepad GetGamepad()
    {
        
        return Gamepad.current;

        #region Linq Query Equivalent Logic
        //Gamepad gamepad = null;
        //foreach (var g in Gamepad.all)
        //{
        //    foreach (var d in _playerInput.devices)
        //    {
        //        if(d.deviceId == g.deviceId)
        //        {
        //            gamepad = g;
        //            break;
        //        }
        //    }
        //    if(gamepad != null)
        //    {
        //        break;
        //    }
        //}
        //return gamepad;
        #endregion
    }
}