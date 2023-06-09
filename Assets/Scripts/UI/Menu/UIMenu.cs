using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;


[AddComponentMenu("#Survival Game/UI/UI Menu")]
public class UIMenu : MonoBehaviour
{
    public enum MenuType
    {
        Active,
        Passive
    }
    public delegate void MenuEvent();
    
    public static event MenuEvent EventMenuOpened;
    public static event MenuEvent EventMenuClosed;
        
     /// <summary>
     /// Event when the volume is triggered by Enter
     /// </summary>
     public UnityEvent EventOnOpenMenu;
     /// <summary>
     /// Event when the volume is triggered by Exit
     /// </summary>
     public UnityEvent EventOnCloseMenu;
    /// <summary>
    /// Indicate the current active menu
    /// </summary>
    public static UIMenu ActiveMenu { get; private set; }
    [Tooltip("Assign the button to select when the menu is opened")]
    [SerializeField] private GameObject _firstSelectedGameObject;
    [Header("Settings")]
    [SerializeField] private bool closeMenuOnReleaseButton;
    [Tooltip("The menu can be opened when an another menu is open ?")]
    [SerializeField] private bool canBeOpenedAnywhere;
    [Tooltip("Reopen the parent menu when the menu is closed ?")]
    [SerializeField] private bool reopenPreviousMenuOnClose;
    [Tooltip("The menu can erase the active menu and close it ?")]
    [SerializeField] private bool EraseActiveMenu;

    // [Header("Timescale")] 
    // [SerializeField] private bool canAdjustTimescale = true;
    // [Tooltip("The timescale when the menu is opened")]
    // [SerializeField] [Range(0,10)] private float timeScale = 1;
    // private float _previousTimeScale;
    private GameObject _previousMenu;
    private double TOLERANCE = 0.05f;

    [SerializeField] private bool StayOpenOnAwake = true;
    [SerializeField] private MenuType _menuType = MenuType.Active;
    public bool IsOpen { get; private set; }

    public MenuType menuType => _menuType;

    private void Awake()
    {
        EventMenuOpened += OnEventMenuOpened;
    }

    private void OnEventMenuOpened()
    {
        if (menuType == MenuType.Passive)
        {
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!StayOpenOnAwake)
        {
            gameObject.SetActive(false);
        }
        else
        {
            OpenMenu();
        }
       
    }

    private void OnEnable()
    {
        if( EventSystem.current != null)
        {
            if (_firstSelectedGameObject != null)
            {
                EventSystem.current.SetSelectedGameObject(_firstSelectedGameObject);
            }
            else
            {
                var buttonInChild = GetComponentInChildren<Button>();
                if (buttonInChild != null )
                {
                    EventSystem.current.SetSelectedGameObject(buttonInChild.gameObject);
                }
            }
        }
      
    }

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        switch(context.phase)
        {
            case InputActionPhase.Started:
                if (ActiveMenu == this && !closeMenuOnReleaseButton)
                {
                    this.CloseMenu(false);
                }
                else
                {
                    this.OpenMenu(null);
                }
                break;
            case InputActionPhase.Canceled:
                if(closeMenuOnReleaseButton)
                    this.CloseMenu(false);
                break;
        }
    }

    public void OpenCloseMenu()
    {
        if (IsOpen)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }
    public void OpenMenu()
    {
        OpenMenu(null);
    }
    public virtual void OpenMenu(GameObject parentMenu = null)
    {
        if (this.enabled == false)
        {
            return;
        }
        if (!canBeOpenedAnywhere && ActiveMenu != null && parentMenu != ActiveMenu.gameObject)
        {
            return;
        }
        EventOnOpenMenu?.Invoke();
        EventMenuOpened?.Invoke();
        
        // Erase the current active menu to replace it
        if (EraseActiveMenu && ActiveMenu != null && ActiveMenu != this)
        {
            ActiveMenu.CloseMenu();
        }
        ActiveMenu = this;
        // Menu opened by a parent ? Go save it in cache
        if (parentMenu != null)
        {
            _previousMenu = parentMenu;
            _previousMenu.SetActive(false);
        }
        /*if (canAdjustTimescale)
        {
            // Menu affect Timescale 
            _previousTimeScale = Time.timeScale;
            if (Math.Abs(Time.timeScale - 1) < TOLERANCE)
            {
                _previousTimeScale = Time.timeScale;
                Time.timeScale = timeScale;
            }
        }*/
        // Open the menu
        gameObject.SetActive(true);
        IsOpen = true;
    }
    public virtual void CloseMenu(bool openPreviousMenu = false)
    {
        if (ActiveMenu == null || !IsOpen)
        {/*
            if (canAdjustTimescale)
            {
                Time.timeScale = 1;
            }*/
            return;
        }
           
        
     
        /*if (canAdjustTimescale)
        {
            if (ActiveMenu != null)
            {
                Time.timeScale = ActiveMenu._previousTimeScale;
            }
            else
            {
                Time.timeScale = 1;
            }
          
        }*/

        // Parent menu
        if(_previousMenu != null)
        {
            if (openPreviousMenu)
            {
                _previousMenu.SetActive(true);
            }
            else
            {
                _previousMenu = null;
            }
        }
        // Close menu
        IsOpen = false;
        gameObject.SetActive(false);
        ActiveMenu = null;
        EventOnCloseMenu?.Invoke();
        EventMenuClosed?.Invoke();
    }

    private void OnDestroy()
    {
        CloseMenu();
    }
}