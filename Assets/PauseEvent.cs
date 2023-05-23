using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseEvent : MonoBehaviour
{
    [SerializeField] ScriptableEventBool m_PauseEvent;

    public void Pause(bool pause)
    {
        Debug.Log("game is pause :" + pause);
        if(GameStateManager.Instance.GameStateOverride.canPause)
        {
            m_PauseEvent.LaunchEvent(pause);
        }
        
    }
}
