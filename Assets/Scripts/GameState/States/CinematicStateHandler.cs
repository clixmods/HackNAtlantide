using UnityEngine;

public class CinematicStateHandler : MonoBehaviour
{
    public void SetCinematicState(bool value)
    {
        GameStateManager.Instance.cinematicStateObject.SetActive(value);
    }
}