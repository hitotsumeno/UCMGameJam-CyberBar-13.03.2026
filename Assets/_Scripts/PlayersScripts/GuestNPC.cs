using System.Collections;
using UnityEngine;

public class GuestNPC : MonoBehaviour
{
    [SerializeField] private Transform _entranceTarget; // set to GuestRoom transform (optional)
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _arriveThreshold = 0.05f;

    private Coroutine _moveRoutine;

    private void Reset()
    {
        if (_entranceTarget == null)
        {
            var go = GameObject.Find("GuestRoom");
            if (go != null) _entranceTarget = go.transform;
        }
    }

    private void Update()
    {
        // If not currently moving to a table and not parented (not seated), move toward entrance
        if (_moveRoutine == null && transform.parent == null && _entranceTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _entranceTarget.position, _moveSpeed * Time.deltaTime);
        }
    }

    // Called by GuestsContoller to instruct this NPC to walk to a table, then optionally parent it.
    public void MoveToTable(Transform table, Vector3 seatOffset, bool parentOnArrival)
    {
        if (_moveRoutine != null) StopCoroutine(_moveRoutine);
        _moveRoutine = StartCoroutine(MoveAndSeat(table, seatOffset, parentOnArrival));
    }

    private IEnumerator MoveAndSeat(Transform table, Vector3 seatOffset, bool parentOnArrival)
    {
        // disable physics simulation so we move transform directly
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        Vector3 targetPos = table.position + seatOffset;
        while (Vector3.Distance(transform.position, targetPos) > _arriveThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = table.rotation;

        if (parentOnArrival) transform.SetParent(table, worldPositionStays: true);

        _moveRoutine = null;
        Debug.Log($"{name} seated at {table.name}");
    }
}
