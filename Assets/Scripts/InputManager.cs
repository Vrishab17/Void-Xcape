using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    [SerializeField] Movement movement;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] Gun gun;

    InputSystem_Actions controls;
    InputSystem_Actions.PlayerActions playerActions;

    Vector2 xInput;
    Vector2 mouseInput;

    Coroutine fireCoroutine;

    
    private void Awake()
    {
        controls = new InputSystem_Actions();
        playerActions = controls.Player;

        playerActions.Move.performed += ctx => xInput = ctx.ReadValue<Vector2>();
        playerActions.Jump.performed += _ => movement.OnJumpPressed();

        // This replaces MouseX & MouseY — we use "Look" instead
        playerActions.Look.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();

        // This replaces Shoot — we use "Attack" instead
        playerActions.Attack.started += _ => StartFiring();
        playerActions.Attack.canceled += _ => StopFiring();
    }    
    
    private void Update ()
    {
        movement.ReceiveInput(xInput);
        mouseLook.ReceiveInput(mouseInput);
    }

    void StartFiring()
    {
        fireCoroutine = StartCoroutine(gun.RapidFire());
    }

    void StopFiring()
    {
        if (fireCoroutine != null) {
            StopCoroutine(fireCoroutine);
        }
    }

    private void OnEnable ()
    {
        controls.Enable();
    }

    private void OnDestroy ()
    {
        controls.Disable();
    }
}
