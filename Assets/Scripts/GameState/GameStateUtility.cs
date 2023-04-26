using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateUtility
{
    //the lower has the priority
    public const int RunTimePriority = 1000;
    public const int PausePriority = 5;
    public const int CinematiquePriority = 20;
    public const int MainMenuPriority = 2;
    public const int combatPriority = 50;
}
