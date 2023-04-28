using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    #region Singleton

    public static PlayerHandler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public GameObject player;
}