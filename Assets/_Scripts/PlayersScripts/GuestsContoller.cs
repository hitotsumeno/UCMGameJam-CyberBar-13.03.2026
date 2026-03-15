using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestsContoller : MonoBehaviour
{
    [Header("Tables")]
    [SerializeField] private Transform[] _tables;                // assign in inspector (preferred)
    [SerializeField] private string _tableTag = "Table";         // optional: used to auto-find tables if none assigned

    [Header("Accepted entrants (tags)")]
    [SerializeField] private string[] _acceptedTags = new[] { "Guest", "PlayerNPC" };

    [Header("Seating")]
    [SerializeField] private Vector3 _seatOffset = Vector3.zero; // offset from table transform where guest should be placed
    [SerializeField] private bool _parentToTable = true;         // parent guest to table after seating
    [SerializeField] private bool _teleportOnSeat = true;        // if false, controller will tell GuestNPC to walk to table

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

    // 2D trigger handler
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsAcceptedTag(other.tag))
            return;

        int tableIndex = GetRandomFreeTableIndex();
        if (tableIndex == -1)
        {
            Debug.Log("GuestsController: No free tables available.");
            return;
        }

        // If we want guests/players to move to table smoothly, prefer telling the GuestNPC to move.
        var guestObj = other.gameObject;
        if (!_teleportOnSeat)
        {
            var npc = guestObj.GetComponent<GuestNPC>();
            if (npc != null)
            {
                // give npc the table target and let it move there
                npc.MoveToTable(_tables[tableIndex], _seatOffset, _parentToTable);
                _occupied[tableIndex] = true;
                Debug.Log($"GuestsController: Instructed {guestObj.name} to move to table #{tableIndex}");
                return;
            }
        }

        // Default: teleport and parent
        SeatGuestAtTable(guestObj, tableIndex);
    }

    private bool IsAcceptedTag(string tag)
    {
        for (int i = 0; i < _acceptedTags.Length; i++)
            if (_acceptedTags[i] == tag) return true;
        return false;
    }

    // return a random free table index, or -1 if none
    private int GetRandomFreeTableIndex()
    {
        var free = new List<int>();
        for (int i = 0; i < _occupied.Length; i++)
            if (!_occupied[i])
                free.Add(i);

        if (free.Count == 0)
            return -1;

        int pick = free[Random.Range(0, free.Count)];
        return pick;
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

        // stop physics on seated guest so they stay put
        var rb = guest.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        _occupied[tableIndex] = true;

        Debug.Log($"GuestsController: Seated {guest.name} at table #{tableIndex}");
    }

    /// <summary>
    /// Public helper to seat a guest programmatically.
    /// Returns true if the guest was assigned a table.
    /// </summary>
    public bool TrySeatGuest(GameObject guest)
    {
        if (guest == null) return false;

        int tableIndex = GetRandomFreeTableIndex();
        if (tableIndex == -1)
        {
            Debug.Log("GuestsController: No free tables available.");
            return false;
        }

        // If teleporting is disabled, prefer telling GuestNPC to walk to the table
        if (!_teleportOnSeat)
        {
            var npc = guest.GetComponent<GuestNPC>();
            if (npc != null)
            {
                npc.MoveToTable(_tables[tableIndex], _seatOffset, _parentToTable);
                _occupied[tableIndex] = true;
                Debug.Log($"GuestsController: Instructed {guest.name} to move to table #{tableIndex}");
                return true;
            }
        }

        // Default: teleport and seat
        SeatGuestAtTable(guest, tableIndex);
        return true;
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

