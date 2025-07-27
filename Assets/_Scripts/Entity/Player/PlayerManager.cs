
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Rendering.UI;
using FishNet.Object;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;
    public Entity entity;
    // public DashController dashController; 
    public PlayerInput playerInput;

    public InputAction movementAction { get; private set; }

    [Header("Look")]
    public InputAction lookAction { get; private set; }

    [Header("Interact")]
    [SerializeField] private PlayerInteracter playerInteracter;
    public InputAction primaryAction { get; private set; }
    public InputAction shiftAction { get; private set; }

    [Header("Attack")]
    public InputAction attackAction { get; private set; }

    // [Header("Menus")] 
    public InputAction secondaryAction { get; private set; }
    public MenuManager menuManager => MenuManager.instance;

    public InputAction scrollAction { get; private set; }
    [Header("BattleUI")]
    public Image playButton;
    public Image itemButton;
    public Image fleeButton;
    public Color inactiveColor = Color.gray;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Two Player Managers Found, Deleting Second");
            Destroy(gameObject);
        }
    }

    public void Initalize(PlayerController player)
    {

        // // Lock cursor
        // Cursor.lockState = CursorLockMode.Locked; 

        // Get inputs
        movementAction = playerInput.actions.FindAction("Move");
        lookAction = playerInput.actions.FindAction("Look");
        attackAction = playerInput.actions.FindAction("Attack");
        primaryAction = playerInput.actions.FindAction("PrimaryAction");
        secondaryAction = playerInput.actions.FindAction("SecondaryAction");
        shiftAction = playerInput.actions.FindAction("Sprint");
        scrollAction = playerInput.actions.FindAction("Scroll");

        // Set all of the actions to their corresponding functions.
        // ! Remember to set the OnEnable(), OnDisable(), and OnDestroy()
        lookAction.performed += Look;
        shiftAction.started += StartSprint;
        shiftAction.canceled += EndSprint;
        attackAction.started += StartAttack;
        primaryAction.started += StartInteract;
        secondaryAction.started += ToggleMenu;

        entity = player.entity;
        entity.entityHealth.OnDie += OnPlayerDie;
    }

    public void UnInitalize()
    {
        if (entity != null)
        {
            entity.entityHealth.OnDie -= OnPlayerDie;
        }
    }


    // ? Don't think the player manager should ever get disabled fully but ehhhhh
    // private void OnEnable()
    // {
    //     movementAction.Enable();
    //     lookAction.Enable();
    //     attackAction.Enable();
    //     primaryAction.Enable();
    //     shiftAction.Enable();
    //     secondaryAction.Enable();
    //     scrollAction.Enable();
    // }

    // private void OnDisable()
    // {
    //     movementAction.Disable();
    //     lookAction.Disable();
    //     attackAction.Disable();
    //     primaryAction.Disable();
    //     shiftAction.Disable();
    //     secondaryAction.Disable();
    //     scrollAction.Disable();
    // }

    public void OnDestroy()
    {
        if (lookAction != null)
        {
            lookAction.performed -= Look;
        }
        if (shiftAction != null)
        {
            shiftAction.started -= StartSprint;
            shiftAction.canceled -= EndSprint;
        }
        if (attackAction != null)
        {
            attackAction.started -= StartAttack;
        }
        if (primaryAction != null)
        {
            primaryAction.started -= StartInteract;
        }
        if (secondaryAction != null)
        {
            secondaryAction.started -= ToggleMenu;
        }
    }


    // void FixedUpdate()
    // {
    //     if (entity != null)
    //     {
    //         Vector2 planarMovementInput = movementAction.ReadValue<Vector2>();
    //         // print(planarMovementInput);
    //         entity.entityMovement.Move(planarMovementInput);

    //     }

    // }

    public void SetLookState(bool canLook = true)
    {
        if (canLook)
        {
            lookAction.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            lookAction.Disable();
        }
    }

    public void LockInputs(bool inputsLocked, bool cursorLocked, bool menuButtonsLocked = false)
    {
        SetLookState(!cursorLocked);
        if (inputsLocked)
        {
            movementAction.Disable();
            // dashAction.Disable();
            attackAction.Disable();
            // itemAction.Disable();
            primaryAction.Disable();
            shiftAction.Disable();
            scrollAction.Disable();
        }
        else
        {
            movementAction.Enable();
            // dashAction.Enable();
            attackAction.Enable();
            // itemAction.Enable();
            primaryAction.Enable();
            shiftAction.Enable();
            scrollAction.Enable();
        }

        if (menuButtonsLocked)
        {
            secondaryAction.started -= ToggleMenu;
            // secondaryAction.Disable();
            // inventoryAction.Disable();
        }
        else
        {
            secondaryAction.started += ToggleMenu;
            // secondaryAction.Enable();
            // inventoryAction.Enable();
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        // Debug.Log(context);
        Vector2 dir = context.ReadValue<Vector2>();
        // Add Sensitivity
        dir.x *= OptionsManager.instance.xSensitivity;
        dir.y *= OptionsManager.instance.ySensitivity;
        // playerCameraControl.AddRotation(dir);
    }

    // public void Dash(InputAction.CallbackContext context)
    // {
    //     dashController.AttemptDash(movementAction.ReadValue<Vector2>());
    // }

    public void StartSprint(InputAction.CallbackContext context)
    {
    }

    public void EndSprint(InputAction.CallbackContext context)
    {
    }

    public void StartAttack(InputAction.CallbackContext context)
    {
        // // TODO: Remove this once all player variants have attackController.
        // if (attackController != null)
        // {
        //     attackController.StartAttacking();
        // }
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        // switch (menuManager.curState)
        // {
        //     case MenuManager.MenuState.None:
        //         menuManager.SetState(MenuManager.MenuState.Pause);
        //         break;
        //     default:
        //         menuManager.SetState(MenuManager.MenuState.None);
        //         break;
        // }
        // Debug.Log("HOWWW");
        // Debug.Log(secondaryAction.enabled);
        if (menuManager.curState == null)
        {
            menuManager.SetPause();
        }
        else
        {
            menuManager.SetState(null);
        }
    }

    public void StartInteract(InputAction.CallbackContext context)
    {
        // playerInteracter.interactionHeld = true;
        // playerInteracter.areaToggle = shiftAction.IsPressed();
        playerInteracter.AttemptInteract();
    }

    private void OnPlayerDie()
    {
        // LockInputs(true, true, true);
        entity.rb.Velocity(Vector3.zero);
        // entity.rb.isKinematic = true;
        // MenuManager.instance.SetState(MenuManager.MenuState.Death);
        // MenuManager.instance.SetDeath();
    }

}
