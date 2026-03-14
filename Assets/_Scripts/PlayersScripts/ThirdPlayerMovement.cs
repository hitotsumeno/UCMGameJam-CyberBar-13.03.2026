using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPlayerMovement : PlayerMovementBase
{
    [SerializeField] private InputReaderScript _playerInput;
    [SerializeField] private PlayerMoveStats thisPlayerStats;

    private void Awake()
    {
        _playerInput.tp_MoveEvent += HandleMove;
        MoveStats = thisPlayerStats;
    }

    private void HandleMove(Vector2 dir)
    {
        _moveDirections = dir;
    }

    # region Handle Enable/Disable
    private void OnEnable()
    {
        _playerInput.tp_MoveEvent += HandleMove;
    }
    private void OnDisable()
    {
        _playerInput.tp_MoveEvent -= HandleMove;

    }
    #endregion
}

