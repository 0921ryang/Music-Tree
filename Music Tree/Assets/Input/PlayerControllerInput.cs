using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerControllerInput : MonoBehaviour
{
    private PlayerInput _playerInput;
    private IObservable<Vector2> _move;
    private IObservable<Vector2> _look;
    private ReadOnlyReactiveProperty<bool> _run;
    private ReadOnlyReactiveProperty<bool> _jump;
    private ReadOnlyReactiveProperty<bool> _crouch;
    public ReactiveProperty<bool> hasUI;

    public PlayerInput playerInput => _playerInput;
    public ReadOnlyReactiveProperty<bool> crouch => _crouch;
    public IObservable<Vector2> move => _move;
    public IObservable<Vector2> look => _look;
    public ReadOnlyReactiveProperty<bool> run => _run;
    public ReadOnlyReactiveProperty<bool> jump => _jump;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        
        _move = this.UpdateAsObservable()
            .Select(_ => _playerInput.Player.Move.ReadValue<Vector2>());
        
        _look = this.UpdateAsObservable()
            .Select(_ => _playerInput.Player.Look.ReadValue<Vector2>());
        
        _run = this.UpdateAsObservable()
            .Select(_ => _playerInput.Player.Run.ReadValueAsObject() != null)
            .ToReadOnlyReactiveProperty();

        _jump = this.UpdateAsObservable()
            .Select(_ => _playerInput.Player.Jump.ReadValueAsObject() != null)
            .ToReadOnlyReactiveProperty();

        _crouch = this.UpdateAsObservable()
            .Select(_ => _playerInput.Player.Crouch.ReadValueAsObject() != null)
            .ToReadOnlyReactiveProperty();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }
}
