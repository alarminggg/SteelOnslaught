using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerUINavigation : MonoBehaviour, PlayerControls.IPlayerUINavigationActions
{
    public PlayerControls PlayerControls { get; private set; }
    public Vector2 NavigateMenu {  get; private set; }
    public bool SelectPressed { get; private set; }
    public bool BackPressed { get; private set; }

    private void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.PlayerUINavigation.Enable();
        PlayerControls.PlayerUINavigation.SetCallbacks(this);
    }

    private void OnDisable()
    {
        PlayerControls.PlayerUINavigation.Disable();
        PlayerControls.PlayerUINavigation.RemoveCallbacks(this);
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        NavigateMenu = context.ReadValue<Vector2>();
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SelectPressed = true;

        }
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            BackPressed = true;

        }
    }
}
