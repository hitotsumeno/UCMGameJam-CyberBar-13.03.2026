using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrafting : MonoBehaviour
{
    [SerializeField] private RecipeDatabase _recipeDatabase;
    [SerializeField] private Transform _holdPoint;

    private PlayerTakeLeftBase _pickupSystem;

    private void Awake()
    {
        _pickupSystem = GetComponent<PlayerTakeLeftBase>();
    }

    // Call this from PlayerTakeLeft instead of plain PickUp
    public void TryPickupAndCraft(GameObject newItem)
    {
        GameObject heldItem = _pickupSystem.GetHeldItem();

        // Nothing held yet — just pick up normally
        if (heldItem == null)
            return;

        // Already holding something — try to craft
        GameObject resultPrefab = _recipeDatabase.FindResult(heldItem, newItem);

        if (resultPrefab == null)
        {
            Debug.Log("No recipe found for these two ingredients.");
            return;
        }

        // Destroy both ingredients
        Object.Destroy(heldItem);
        Object.Destroy(newItem);

        // Spawn result at holdPoint
        GameObject result = Instantiate(resultPrefab, _holdPoint.position, Quaternion.identity);
        result.transform.SetParent(_holdPoint, worldPositionStays: false);
        result.transform.localPosition = Vector3.zero;
        result.transform.localRotation = Quaternion.identity;

        // Disable physics on result while held
        var rb = result.GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;

        var col = result.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Debug.Log($"Crafted: {result.name}");
        }
    }
