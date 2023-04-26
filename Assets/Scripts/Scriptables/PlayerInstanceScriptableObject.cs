using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Data/PlayerInstance")]
public class PlayerInstanceScriptableObject : ScriptableObject
{
    static GameObject _player;
    public static GameObject Player 
    { 
        get 
        { 
            if (_player == null)
            { 
                _player = FindObjectOfType<PlayerMovement>().gameObject; 
            } 
            return _player; 
        } 
        set 
        { 
            _player = value; 
        } 
    }
}
