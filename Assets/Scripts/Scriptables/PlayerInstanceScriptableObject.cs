using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Data/PlayerInstance")]
public class PlayerInstanceScriptableObject : ScriptableObject
{
    GameObject _player;
    public GameObject Player { get { return _player; } set { _player = value; } }
}
