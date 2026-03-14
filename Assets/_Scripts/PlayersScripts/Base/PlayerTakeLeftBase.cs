using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeLeftBase : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private Transform _holdPoint;
    [SerializeField] private string _pickupTag = "Pickup";
    [SerializeField] private float _dropPushForce = 2f;

    protected GameObject _nearbyPickup;
    protected GameObject _heldItem;

    // Ensure a valid hold point exists at runtime (Reset only runs in editor when adding/resetting component)
    private void Awake()
    {
        if (_holdPoint == null)
        {
            var hp = new GameObject("HoldPoint");
            hp.transform.SetParent(transform, false);
            hp.transform.localPosition = Vector3.right * 0.5f;
            _holdPoint = hp.transform;
        }

    }
    private void HandlePickupInput()
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

    protected void PickUp(GameObject item)
    {
        if (item == null)
            return;

        if (_holdPoint == null)
        {
            _holdPoint = transform;
        }

        _heldItem = item;

        var rb2d = _heldItem.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0f;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
        }
    
        var col2d = _heldItem.GetComponent<Collider2D>();
        if (col2d != null)
            col2d.enabled = false;

        _heldItem.transform.SetParent(_holdPoint, worldPositionStays: false);
        _heldItem.transform.localPosition = Vector3.zero;
        _heldItem.transform.localRotation = Quaternion.identity;

        Debug.Log($"Picked up {_heldItem.name}");
    }

    protected void Drop()
    {
        if (_heldItem == null)
            return;

        _heldItem.transform.SetParent(null, worldPositionStays: true);

        var rb2d = _heldItem.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            // Restore dynamic body so physics applies, but stop any existing velocity
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0f;

            // REMOVE impulse so the item does not "fly" on drop
            // If you want the item to stay exactly where you drop it (no gravity), consider:
            // rb2d.gravityScale = 0;
            // or set constraints: rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        var col2d = _heldItem.GetComponent<Collider2D>();
        if (col2d != null)
            col2d.enabled = true;

        Debug.Log($"Dropped {_heldItem.name}");
        _heldItem = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_pickupTag))
        {
            _nearbyPickup = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(_pickupTag) && other.gameObject == _nearbyPickup)
            _nearbyPickup = null;
    }

    public bool IsHolding() => _heldItem != null;
    public GameObject GetHeldItem() => _heldItem;
}