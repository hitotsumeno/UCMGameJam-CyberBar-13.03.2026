Assets\_Scripts\PlayersScripts\GuestsContoller.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Teleports incoming guest GameObjects to the first available table.
/// - Attach this component to an entrance GameObject that has a 3D collider with Is Trigger = true.
/// - Assign the table Transforms in the inspector (5 tables) or tag table GameObjects as "Table" to auto-find them.
/// - Tag guest cube(s) as "Guest".
/// </summary>
public class GuestsContoller : MonoBehaviour
{
    [Header("Tables")]
    [SerializeField] private Transform[] _tables;                // assign in inspector (preferred)
    [SerializeField] private string _tableTag = "Table";         // optional: used to auto-find tables if none assigned

    [Header("Guests")]
    [SerializeField] private string _guestTag = "Guest";         // tag guests with this

    [Header("Seating")]
    [SerializeField] private Vector3 _seatOffset = Vector3.zero; // offset from table transform where guest should be placed
    [SerializeField] private bool _parentToTable = true;         // parent guest to table after teleporting

    private bool[] _occupied;

    private void Start()
    {
        if (_tables == null || _tables.Length == 0)
        {
            var found = GameObject.FindGameObjectsWithTag(_tableTag);
            if (found != null && found.Length > 0)
            {
                _tables = new Transform[found.Length];
                for (int i = 0; i < found.Length; i++)
                    _tables[i] = found[i].transform;
            }
        }

        if (_tables == null)
            _tables = new Transform[0];

        _occupied = new bool[_tables.Length];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_guestTag))
            return;

        int tableIndex = GetFirstFreeTableIndex();
        if (tableIndex == -1)
        {
            Debug.Log("GuestsController: No free tables available.");
            return;
        }

        SeatGuestAtTable(other.gameObject, tableIndex);
    }

    private int GetFirstFreeTableIndex()
    {
        for (int i = 0; i < _occupied.Length; i++)
            if (!_occupied[i])
                return i;
        return -1;
    }

    private void SeatGuestAtTable(GameObject guest, int tableIndex)
    {
        var table = _tables[tableIndex];
        if (table == null)
        {
            Debug.LogWarning($"GuestsController: Table at index {tableIndex} is null.");
            return;
        }

        // Teleport guest to table + offset
        guest.transform.position = table.position + _seatOffset;
        guest.transform.rotation = table.rotation;

        if (_parentToTable)
            guest.transform.SetParent(table, worldPositionStays: true);

        _occupied[tableIndex] = true;

        Debug.Log($"GuestsController: Seated {guest.name} at table #{tableIndex}");
    }

    // Call this to free a table (e.g. when guest leaves). You can call this from guest logic.
    public void FreeTable(Transform table)
    {
        for (int i = 0; i < _tables.Length; i++)
        {
            if (_tables[i] == table)
            {
                _occupied[i] = false;
                Debug.Log($"GuestsController: Freed table #{i}");
                return;
            }
        }
    }

    // Convenience: free by guest GameObject (unparents and marks table free)
    public void FreeTableByGuest(GameObject guest)
    {
        var parent = guest.transform.parent;
        if (parent == null) return;
        for (int i = 0; i < _tables.Length; i++)
        {
            if (_tables[i] == parent)
            {
                _occupied[i] = false;
                if (_parentToTable) guest.transform.SetParent(null, worldPositionStays: true);
                Debug.Log($"GuestsController: Freed table #{i} from guest {guest.name}");
                return;
            }
        }
    }
}