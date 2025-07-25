using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {  get; private set; }
    private InputSystem_Actions _inputActions;

    public event Action OnClickStarted;
    public event Action OnClickCanceled;

    public event Action OnMiddleTriggerStarted;
    public event Action OnMiddleTriggerCanceled;

    public event Action OnAdditionalOptionStarted;
    public event Action OnAdditionalOptionCanceled;
    public event Action OnAdditionalOptionPerformed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Enable();

        _inputActions.Player.Click.started += Click_started;
        _inputActions.Player.Click.canceled += Click_canceled;

        _inputActions.Player.AdditionalOption.started += AdditionalOption_started;
        _inputActions.Player.AdditionalOption.canceled += AdditionalOption_canceled;
        _inputActions.Player.AdditionalOption.performed += AdditionalOption_performed;

        _inputActions.Player.MiddleTrigger.started += MiddleTrigger_started;
        _inputActions.Player.MiddleTrigger.canceled += MiddleTrigger_canceled;
    }

    

    private void MiddleTrigger_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMiddleTriggerCanceled?.Invoke();
    }

    private void MiddleTrigger_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMiddleTriggerStarted?.Invoke();
    }
    //
    private void AdditionalOption_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAdditionalOptionPerformed?.Invoke();

    }
    private void AdditionalOption_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAdditionalOptionCanceled?.Invoke();
    }
    private void AdditionalOption_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAdditionalOptionStarted?.Invoke();
    }
    //
    private void Click_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnClickCanceled?.Invoke();
    }

    private void Click_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnClickStarted?.Invoke();
    }

    public Vector2 GetPointerDelta()
    {
        return _inputActions.Player.Look.ReadValue<Vector2>();
    }

    public float GetScrollDelta()
    {
        return _inputActions.Player.Scroll.ReadValue<float>();
    }

    private void OnDisable()
    {
        _inputActions.Player.Click.started -= Click_started;
        _inputActions.Player.Click.canceled -= Click_canceled;

        _inputActions.Player.AdditionalOption.started -= AdditionalOption_started;
        _inputActions.Player.AdditionalOption.canceled -= AdditionalOption_canceled;
        _inputActions.Player.AdditionalOption.performed -= AdditionalOption_performed;

        _inputActions.Player.MiddleTrigger.started -= MiddleTrigger_started;
        _inputActions.Player.MiddleTrigger.canceled -= MiddleTrigger_canceled;

        _inputActions.Player.Disable();
    }
}
