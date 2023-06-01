using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGameState : MonoBehaviour
{
    [SerializeField] bool executeOnStart = true;
    
    // Start is called before the first frame update
    void Start()
    {
        if(executeOnStart)
        {
            GameStateManager.Instance.mainMenuStateObject.SetActive(true);
        }
    }
    private void OnDisable()
    {
        GameStateManager.Instance.mainMenuStateObject.SetActive(false);
    }
}
