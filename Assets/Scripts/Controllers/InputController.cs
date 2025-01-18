using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public Action<Vector2> OnTouchStarted;
    public Action<Vector2, Vector2> OnTouching;
    public Action<Vector2> OnTouchEnded;

    public PlayerInput Input;

    private InputAction OnPressStateChange;
    private InputAction OnPositionChange;

    private void Awake()
    {
        OnPressStateChange = Input.actions.FindAction("ChangeTouch");
        OnPositionChange = Input.actions.FindAction("Touching");
    }

    private void OnTouch(InputAction.CallbackContext context)
    {
        Vector2 CurrentTouchPosition = Touchscreen.current.primaryTouch.position.value;

        OnTouchStarted?.Invoke(CurrentTouchPosition);
    }

    private void OnTouchingDelta(InputAction.CallbackContext context)
    {
        Vector2 CurrentTouchPosition = Touchscreen.current.primaryTouch.position.value;
        Vector2 CurrentDelta = context.ReadValue<Vector2>();

        OnTouching?.Invoke(CurrentTouchPosition, CurrentDelta);
    }

    private void OnEndTouch(InputAction.CallbackContext context)
    {
        Vector2 CurrentTouchPosition = Touchscreen.current.primaryTouch.position.value;

        OnTouchEnded?.Invoke(CurrentTouchPosition);
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        OnPressStateChange.started += OnTouch;
        OnPositionChange.performed += OnTouchingDelta;
        OnPressStateChange.canceled += OnEndTouch;
    }
    private void UnsubscribeFromEvents()
    {
        OnPressStateChange.started -= OnTouch;
        OnPositionChange.performed -= OnTouchingDelta;
        OnPressStateChange.canceled -= OnEndTouch;
    }
}
