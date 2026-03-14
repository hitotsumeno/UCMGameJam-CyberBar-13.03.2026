using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPlayerMovement : PlayerMovementBase
{ 
    [SerializeField] private InputReaderScript _playerInput;
    [SerializeField] private PlayerMoveStats thisPlayerStats;
    
    private void Awake()
    {
        _playerInput.sp_MoveEvent += HandleMove;
        MoveStats = thisPlayerStats;
    }
    
    private void HandleMove(Vector2 dir)
    {
        _moveDirections = dir;
    }
    
    #region Handle Enable/Disable
    private void OnEnable()
    {
        _playerInput.sp_MoveEvent += HandleMove;
    }
    private void OnDisable()
    {
        _playerInput.sp_MoveEvent -= HandleMove;
    
    }
    #endregion
}

  
