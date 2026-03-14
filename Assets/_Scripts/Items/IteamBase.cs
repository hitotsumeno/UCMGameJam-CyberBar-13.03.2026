using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class IteamBase : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _coll;
    [SerializeField] private string _pickupTag = "Pickup";

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<BoxCollider2D>();

        SetupRigidbody();
        SetupCollider();

        gameObject.tag = _pickupTag;
    }

    private void SetupCollider()
    {
        _coll.isTrigger = true;
    }

    private void SetupRigidbody()
    {
        _rb.gravityScale = 0;
    }
}
