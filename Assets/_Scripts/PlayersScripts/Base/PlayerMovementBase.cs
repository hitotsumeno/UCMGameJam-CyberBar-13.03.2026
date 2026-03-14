using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBase : MonoBehaviour
{
    // Referance for Input reader
    // Reference for Stats
    protected PlayerMoveStats MoveStats;

    [SerializeField] private Collider2D _bodyColl;
    [SerializeField] private Rigidbody2D _rb;

    protected Vector2 _moveDirections; // Protected variable for storing directions from reader.
    private Vector2 _moveVelocity;
    private bool _isFacingRight;

    void Start()
    {
        _isFacingRight = true;
    }

    void FixedUpdate()
    {
        Move( _moveDirections,MoveStats.Acceleration,MoveStats.Deceleration);
    }

    private void Move(Vector2 moveDirections, float acceleration, float deceleration)
    {
        if (moveDirections != Vector2.zero)
        {
            // check if players needs to turn around
            TurnCheck(moveDirections);

            //Players moves
            Vector2 targetVelocity = Vector2.zero;
            targetVelocity = moveDirections * MoveStats.maxSpeed;

            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            gameObject.transform.position += new Vector3(_moveVelocity.x, _moveVelocity.y, 0);
        }

        if (moveDirections == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _rb.velocity = _moveVelocity.normalized;
        }
    }
    #region Turn
    private void TurnCheck(Vector2 moveInput)
    {
        if (_isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if (!_isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }
    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            _isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }
    #endregion
}
