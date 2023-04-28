using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if(instance == null)
        {
            instance = this;
        }
        
    }
    // Public Methods
    public void RumbleConstant(RumblerDataConstant rumblerDataConstant)
    {
        //if(Settings.UseRumbler)
        {
            activeRumbePattern = RumblePattern.Constant;
            lowA = rumblerDataConstant.low;
            highA = rumblerDataConstant.high;
            rumbleDurration = Time.unscaledTime + rumblerDataConstant.duration;
        }
        
    }

    public void RumblePulse(RumblerDataPulse rumblerDataPulse)
    {
        //if (Settings.UseRumbler)
        {
            activeRumbePattern = RumblePattern.Pulse;
            lowA = rumblerDataPulse.low;
            highA = rumblerDataPulse.high;
            rumbleStep = rumblerDataPulse.burstTime;
            pulseDurration = Time.unscaledTime + rumblerDataPulse.burstTime;
            rumbleDurration = Time.unscaledTime + rumblerDataPulse.duration;
            isMotorActive = true;
            var g = GetGamepad();
            g?.SetMotorSpeeds(lowA, highA);
        }
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