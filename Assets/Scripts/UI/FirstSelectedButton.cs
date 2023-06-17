using UnityEngine;
using UnityEngine.EventSystems;


public class FirstSelectedButton : MonoBehaviour
{
    private void OnEnable()
    {
        if( EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
      
    }
}
