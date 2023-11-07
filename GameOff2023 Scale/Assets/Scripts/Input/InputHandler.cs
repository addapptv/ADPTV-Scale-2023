using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance { get; private set; }

    [SerializeField] MovementController movementController;

    public InputManager controls;

    public Vector2 _moveInput;
    Vector2 _mousePosition;
    float _freeLookToggle;

    public InputManager.PlayerControlsActions playerControls;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Handler in the scene.");
        }
        instance = this;

        //Intialise all controls

        controls = new InputManager();

        playerControls = controls.PlayerControls;

        //Movement controls events
/*        playerControls.Movement.started += ctx => _moveInput = ctx.ReadValue<Vector2>();
        playerControls.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        playerControls.Movement.canceled += ctx => _moveInput = ctx.ReadValue<Vector2>();*/

/*        //Camera
        cameraControl.FreeLook.performed += ctx => _freeLookToggle = ctx.ReadValue<float>();
        cameraControl.FreeLook.canceled += ctx => _freeLookToggle = ctx.ReadValue<float>();


        //Menus
        menusControl.ShowBackpack.performed += _ => GameEventsManager.instance.inputEvents.MenuPressed();
        menusControl.ShowBackpack.canceled += _ => GameEventsManager.instance.inputEvents.MenuPressed();

        menusControl.ShowQuestList.performed += _ => GameEventsManager.instance.inputEvents.MenuPressed();
        menusControl.ShowQuestList.canceled += _ => GameEventsManager.instance.inputEvents.MenuPressed();

        //Save/Load
        gameControl.NewGame.performed += _ => GameEventsManager.instance.saveEvents.NewGame();

        gameControl.Save.performed += _ => GameEventsManager.instance.saveEvents.SaveGame();

        gameControl.Load.performed += _ => GameEventsManager.instance.saveEvents.LoadGame();*/
    }

    private void Update()
    {
        /*        _mousePosition = playerMovement.MousePos.ReadValue<Vector2>();*/

        _moveInput = playerControls.Movement.ReadValue<Vector2>();
        movementController.ReceiveMoveInput(_moveInput);
/*        tPMovement.ReceiveFreeLook(_freeLookToggle);*/

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

}