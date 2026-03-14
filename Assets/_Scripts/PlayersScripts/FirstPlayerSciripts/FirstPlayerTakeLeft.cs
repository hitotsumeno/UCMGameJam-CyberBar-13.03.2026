using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayerTakeLeft : PlayerTakeLeftBase
{
    [SerializeField] private InputReaderScript _playerInput;

    private void Awake()
    {
        _playerInput.fp_takeUpEvent += HandelTakeUp;
        _playerInput.fp_LeftOutEvent += HandleLeftOut;
    }
    private void HandelTakeUp()
    {
        if (_heldItem == null)
        {
            if (_nearbyPickup != null)
            {
                PickUp(_nearbyPickup);
            }
        }
        else
        {
            Drop();
        }
    }

    private void HandleLeftOut()
    {
            Drop();
    }

    #region Handle Enable/Disable
    private void OnEnable()
    {
        _playerInput.fp_takeUpEvent += HandelTakeUp;
        _playerInput.fp_LeftOutEvent += HandleLeftOut;
    }
    private void OnDisable()
    {
        _playerInput.fp_takeUpEvent -= HandelTakeUp;
        _playerInput.fp_LeftOutEvent -= HandleLeftOut;

    }
    #endregion
}
