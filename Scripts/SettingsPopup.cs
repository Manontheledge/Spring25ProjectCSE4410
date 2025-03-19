using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPopup : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnSubmitName(string name)
    {
        UnityEngine.Debug.Log(name);
    }

    public void OnSpeedValue(float speed)
    {
        UnityEngine.Debug.Log($"Speed: {speed}");
        Messenger<float>.Broadcast(GameEvent.SPEED_CHANGED, speed);
    }
}
