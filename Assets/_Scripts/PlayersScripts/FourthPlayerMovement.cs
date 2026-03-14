using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthPlayerMovement : PlayerMovementBase
{
    [SerializeField] private InputReaderScript _playerInput;
    [SerializeField] private PlayerMoveStats thisPlayerStats;

    private void Awake()
    {
        _playerInput.fop_MoveEvent += HandleMove;
        MoveStats = thisPlayerStats;
    }

    private void HandleMove(Vector2 dir)
    {
        _moveDirections = dir;
    }

    # region Handle Enable/Disable
    private void OnEnable()
    {
        _playerInput.fop_MoveEvent += HandleMove;
    }
    private void OnDisable()
    {
        _playerInput.fop_MoveEvent -= HandleMove;

    }
    #endregion
}
