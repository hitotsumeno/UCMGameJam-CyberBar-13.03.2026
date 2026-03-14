using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthPlayerTakeLeft : PlayerTakeLeftBase
{
    [SerializeField] private InputReaderScript _playerInput;

    private void Awake()
    {
        _playerInput.fop_takeUpEvent += HandelTakeUp;
        _playerInput.fop_LeftOutEvent += HandleLeftOut;
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
        _playerInput.fop_takeUpEvent += HandelTakeUp;
        _playerInput.fop_LeftOutEvent += HandleLeftOut;
    }
    private void OnDisable()
    {
        _playerInput.fop_takeUpEvent -= HandelTakeUp;
        _playerInput.fop_LeftOutEvent -= HandleLeftOut;

    }
    #endregion
}
