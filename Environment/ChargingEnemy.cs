using Godot;
using System;

public partial class ChargingEnemy : AnimatedSprite2D
{
    private enum EnemyState { Aiming, Charging, Overshooting }
    private EnemyState _currentState = EnemyState.Aiming;

    [Export] public float TurnSpeed { get; set; } = 4.0f;        // How fast it rotates to face you
    [Export] public float ChargeSpeed { get; set; } = 400.0f;    // Pixels per second during the dash
    [Export] public float OvershootDistance { get; set; } = 150.0f; // How far past the player it travels
    [Export] public float AimDuration { get; set; } = 1.0f;      // How long it stands still to look at you

    private Node2D _player;
    private Vector2 _chargeDirection;
    private Vector2 _targetPosition;
    private float _aimTimer = 0.0f;

    public override void _Ready()
    {
        _player = GameManager.Instance.player;

        _aimTimer = AimDuration;
        Play();
    }

    public override void _Process(double delta)
    {
        if (_player == null) return;

        if (_player.GlobalPosition.DistanceTo(GlobalPosition) < 10)
        {
            GameManager.Instance.lose();
        }

        GD.Print(_player.GlobalPosition.DistanceTo(GlobalPosition));

        float fDelta = (float)delta;

        switch (_currentState)
        {
            case EnemyState.Aiming:
                HandleAimingState(fDelta);
                break;

            case EnemyState.Charging:
            case EnemyState.Overshooting:
                HandleChargingState(fDelta);
                break;
        }
    }

    private void HandleAimingState(float delta)
    {
        // 1. Rotate smoothly to look at the player
        Vector2 dirToPlayer = (_player.GlobalPosition - GlobalPosition).Normalized();
        float targetAngle = dirToPlayer.Angle();
        GlobalRotation = Mathf.LerpAngle(GlobalRotation, targetAngle, TurnSpeed * delta);

        // 2. Countdown the aiming timer
        _aimTimer -= delta;
        if (_aimTimer <= 0.0f)
        {
            // Lock in coordinates right as the charge begins
            _chargeDirection = dirToPlayer;

            // Calculate a point that is explicitly BEHIND the player's current spot
            _targetPosition = _player.GlobalPosition + (_chargeDirection * OvershootDistance);

            _currentState = EnemyState.Charging;
        }
    }

    private void HandleChargingState(float delta)
    {
        // Move in a locked straight line, bypassing physics matrices (ignores collisions)
        GlobalPosition += _chargeDirection * ChargeSpeed * delta;

        // Check if we have arrived at or passed our destination point
        float distanceToTarget = GlobalPosition.DistanceTo(_targetPosition);

        // If we get close to the calculated overshoot spot, reset back to tracking mode
        if (distanceToTarget < 15.0f || IsPastTarget())
        {
            GlobalPosition = _targetPosition; // Snap cleanly to the end of the line
            _aimTimer = AimDuration;          // Reset timer
            _currentState = EnemyState.Aiming;
        }
    }

    // Helper logic to verify if the enemy overshot the point on its trajectory line
    private bool IsPastTarget()
    {
        Vector2 toTarget = _targetPosition - GlobalPosition;
        return toTarget.Dot(_chargeDirection) < 0;
    }
}