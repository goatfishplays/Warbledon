using System;
using UnityEngine;


public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;


    // public MenuState curState { get; private set; } //! might want to make this a stack that clears upon null to allow going backwards through menus with esc instead straight to game
    public MenuState curState;

    // * Menus
    public void SetNone()
    {
        SetState(null);
    }
    public MenuState pauseMenu;
    public void SetPause()
    {
        SetState(pauseMenu);
    }
    public MenuState audioMenu;
    public void SetAudio()
    {
        SetState(audioMenu);
    }
    public MenuState controlsMenu;
    public void SetControls()
    {
        SetState(controlsMenu);
    }
    public MenuState deathMenu;
    public void SetDeath()
    {
        SetState(deathMenu);
    }
    public MenuState winMenu;
    public void SetWin()
    {
        SetState(winMenu);
    }
    public InvnetoryMenuState inventoryMenu;
    public void SetInventory()
    {
        SetState(inventoryMenu);
    }


    // [SerializeField] private GameObject activeMenu = null;

    // public void SetState(string stateName)
    // {
    //     try
    //     {
    //         MenuState state = (MenuState)Enum.Parse(typeof(MenuState), stateName);
    //         SetState(state);
    //     }
    //     catch (ArgumentException)
    //     {
    //         Debug.LogError($"'{stateName}' could not be parsed as an Enum for MenuState");
    //     }

    // } 

    public void SetState(MenuState newState = null)
    {
        if (curState != null)
        {
            curState.SetActive(false);
        }
        curState = newState;
        if (curState != null)
        {
            curState.SetActive(true);
            // PlayerManager.instance.LockInputs(curState.locksInputs, curState.locksCursor, curState.locksMenuButtons);
        }
        else
        {
            Debug.Log("This is a menu non-locking"); // ? wth does this mean idr
            PlayerManager.instance.LockInputs(inputsLocked: false, cursorLocked: true, menuButtonsLocked: false);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Two MenuManagers detected, deleting second");
            Destroy(this);
        }
    }

}
