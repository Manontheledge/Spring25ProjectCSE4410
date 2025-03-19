using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvent
{
    public const string ENEMY_HIT = "ENEMY HIT";
    public const string SPEED_CHANGED = "SPEED CHANGED";

    //Events for swapping weapons, either add to main GameEvent script or use this one and add other events
    public const string CHANGE_SHOTGUN = "GET EQUIPPED WITH: SHOTGUN";
    public const string CHANGE_RIFLE = "GET EQUIPPED WITH: ASSAULT RIFLE";
    public const string CHANGE_LAUNCHER = "GET EQUIPPED WITH: ROCKET LAUNCHER";
    public const string CHANGE_PISTOL = "GET EQUIPPED WITH: PISTOL?!";
}
