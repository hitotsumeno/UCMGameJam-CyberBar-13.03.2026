using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

[CreateAssetMenu(menuName = "PlayersInputReader")]
public class InputReaderScript : ScriptableObject, GameInputActions.IFirstPlayerActions,GameInputActions.ISecondPlayerActions, GameInputActions.IThirdPlayerActions, GameInputActions.IFourPlayerActions, GameInputActions.IUIActions
{
    private GameInputActions _playersInputAction;

    #region Enable/Disable
    private void OnEnable()
    {
        if (_playersInputAction == null)
        {
            var joysticks = Joystick.all;

            _playersInputAction = new GameInputActions();

            _playersInputAction.FirstPlayer.SetCallbacks(this);
            _playersInputAction.SecondPlayer.SetCallbacks(this);
            _playersInputAction.ThirdPlayer.SetCallbacks(this);
            _playersInputAction.FourPlayer.SetCallbacks(this); 

            AssingDevices();

            EnableAllGameplayInput();
        }
    }

    private void AssingDevices()
    {
        var joysticks = Joystick.all;

        if (joysticks.Count < 4)
        {
            Debug.LogWarning("Less than 4 joysticks connected!");
            return;
        }

        _playersInputAction.FirstPlayer.Get().devices = new ReadOnlyArray<InputDevice>(new[] { joysticks[0] });
        _playersInputAction.SecondPlayer.Get().devices = new ReadOnlyArray<InputDevice>(new[] { joysticks[1] });
        _playersInputAction.ThirdPlayer.Get().devices = new ReadOnlyArray<InputDevice>(new[] { joysticks[2] });
        _playersInputAction.FourPlayer.Get().devices = new ReadOnlyArray<InputDevice>(new[] { joysticks[3] });

        foreach (var joystick in joysticks)
        { 
           Debug.Log(joystick.deviceId + " : " + joystick.displayName);
        }
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    private void DisableAllInput()
    {
        _playersInputAction.FirstPlayer.Disable();
        _playersInputAction.SecondPlayer.Disable();
        _playersInputAction.ThirdPlayer.Disable();
        _playersInputAction.FourPlayer.Disable();

        _playersInputAction.UI.Disable();
    }

    private void EnableAllGameplayInput()
    {
        _playersInputAction.FirstPlayer.Enable(); 
        _playersInputAction.SecondPlayer.Enable();
        _playersInputAction.ThirdPlayer.Enable();
        _playersInputAction.FourPlayer.Enable();

        _playersInputAction.UI.Disable();
    }
    #endregion

    public event Action<Vector2> fp_MoveEvent;
    public event Action<Vector2> sp_MoveEvent;
    public event Action<Vector2> tp_MoveEvent;
    public event Action<Vector2> fop_MoveEvent;

    public event Action fp_takeUpEvent;
    public event Action sp_takeUpEvent;
    public event Action tp_takeUpEvent;
    public event Action fop_takeUpEvent;

    public event Action fp_LeftOutEvent;
    public event Action sp_LeftOutEvent;
    public event Action tp_LeftOutEvent;
    public event Action fop_LeftOutEvent;

    #region First Player
    public void OnFpMove(InputAction.CallbackContext context)
    {
        Debug.Log($"Red - Phase.:{context.phase}, Value.:{context.ReadValue<Vector2>()}");
        fp_MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnFpTakeUp(InputAction.CallbackContext context)
    {
        fp_takeUpEvent?.Invoke();
    }

    public void OnFpLeftOut(InputAction.CallbackContext context)
    {
        fp_LeftOutEvent?.Invoke();
    }
    #endregion

    #region Second Player
    public void OnSpMove(InputAction.CallbackContext context)
    {
        Debug.Log($"Blue - Phase.:{context.phase}, Value.:{context.ReadValue<Vector2>()}");
        sp_MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSpTakeUp(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            sp_takeUpEvent?.Invoke();
        }
    }

    public void OnSpLeftOut(InputAction.CallbackContext context)
    {
        sp_LeftOutEvent?.Invoke();
    }
    #endregion

    #region Third Player
    public void OnTpMove(InputAction.CallbackContext context)
    {
        Debug.Log($"Green - Phase.:{context.phase}, Value.:{context.ReadValue<Vector2>()}");
        tp_MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnTpTakeUp(InputAction.CallbackContext context)
    {
        tp_takeUpEvent?.Invoke();
    }

    public void OnTpLeftOut(InputAction.CallbackContext context)
    {
        tp_LeftOutEvent?.Invoke();
    }
    #endregion

    #region Fourth player
    public void OnFopMove(InputAction.CallbackContext context)
    {
        Debug.Log($"Yellow - Phase.:{context.phase}, Value.:{context.ReadValue<Vector2>()}");
        fop_MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnFopTakeUp(InputAction.CallbackContext context)
    {
        fop_takeUpEvent?.Invoke();
    }

    public void OnFopLeftOut(InputAction.CallbackContext context)
    {
        fop_LeftOutEvent?.Invoke();
    }
    #endregion

    #region UI Input
    public void OnPause(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
    #endregion
}
