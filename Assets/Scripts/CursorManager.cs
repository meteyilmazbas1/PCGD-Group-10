using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UrbanNinja.Input;

public class CursorManager : MonoBehaviour
{
    private static CursorManager _instance;
    private bool _isVisible=true;
    public static Action<InputDevice> InputChangeAction;
    private InputDevice _lastDevice;
    private GameplayInput _input;

    public static CursorManager Instance => _instance;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        _input = new GameplayInput();
    }
    private void OnEnable()
    {
        _input.Enable();
        _input.UIInputActions.Navigate.started += ctx => InputChangeAction?.Invoke(ctx.control.device);
        _input.UIInputActions.Submit.started += ctx => InputChangeAction?.Invoke(ctx.control.device);
        _input.UIInputActions.ScrollDown.started += ctx => InputChangeAction?.Invoke(ctx.control.device);
        _input.UIInputActions.ScrollUp.started += ctx => InputChangeAction?.Invoke(ctx.control.device);
        _input.UIInputActions.Cancel.started += ctx => InputChangeAction?.Invoke(ctx.control.device);
        _input.UIInputActions.Point.started += ctx => InputChangeAction?.Invoke(ctx.control.device);
        InputChangeAction += OnInputDeviceChanged;
    }
    private void OnDisable()
    {
        InputChangeAction -= OnInputDeviceChanged;
    }
    private void OnInputDeviceChanged(InputDevice device)
    {
        if (!_isVisible) return;
        Cursor.visible = device is not Gamepad;
    }
}
