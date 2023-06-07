using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIMenuWatcher : MonoBehaviour
{
    public UnityEvent OnOpenMenu;
    public UnityEvent OnCloseMenu;
    // Start is called before the first frame update
    void Start()
    {
        UIMenu.EventMenuOpened += UIMenuOnEventMenuOpened; 
        UIMenu.EventMenuClosed += UIMenuOnEventMenuClosed;
    }

    private void UIMenuOnEventMenuClosed()
    {
        OnCloseMenu?.Invoke();
    }

    private void UIMenuOnEventMenuOpened()
    {
        OnOpenMenu?.Invoke();
    }
}
