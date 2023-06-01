using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateUtility
{
    //the lower has the priority
    public const int LoadingPriority = 1;
    public const int MainMenuPriority = 2;
    public const int DeadPriority = 3;
    public const int PausePriority = 5;
    public const int CinematiquePriority = 20;
    public const int TutoPriority = 30;
    public const int combatPriority = 50;
    public const int RunTimePriority = 1000;
}
