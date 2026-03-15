using UnityEngine;

/// <summary>
/// Spawns a random number of guests (min..max) at a spawn point and asks the Guests controller to seat them.
/// </summary>
public class GuestSpawner : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private GameObject _guestPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GuestsContoller _controller;

    [Header("Count (inclusive)")]
    [SerializeField] private int _minToSpawn = 1;
    [SerializeField] private int _maxToSpawn = 5;

    [Header("Spawn options")]
    [SerializeField] private string _guestTag = "Guest";
    [SerializeField] private float _spawnRadius = 0.5f; // random offset so they don't overlap exactly
    [SerializeField] private bool _spawnOnStart = true;

    private void Start()
    {
        if (_spawnOnStart)
            SpawnRandomBatch();
    }

    // Call this to spawn a random batch (1..5 by default)
    public void SpawnRandomBatch()
    {
        if (!ValidateSetup())
            return;

           int min = Mathf.Max(0, _minToSpawn);
        int max = Mathf.Max(min, _maxToSpawn);
        int count = Random.Range(min, max + 1);

        for (int i = 0; i < count; i++)
        {
            Vector2 offset = Random.insideUnitCircle * _spawnRadius;
            Vector3 pos = (_spawnPoint != null) ? _spawnPoint.position + (Vector3)offset : transform.position + (Vector3)offset;
            var go = Instantiate(_guestPrefab, pos, Quaternion.identity);
            go.name = $"{_guestPrefab.name}_{i}";
            go.tag = _guestTag;

            bool seated = _controller.TrySeatGuest(go);
            if (!seated)
            {
                Destroy(go);
            }
        }
    }

    private bool ValidateSetup()
    {
        if (_guestPrefab == null)
        {
            Debug.LogWarning("GuestSpawner: _guestPrefab is not assigned.");
            return false;
        }

        if (_controller == null)
        {
            Debug.LogWarning("GuestSpawner: _controller (GuestsContoller) is not assigned.");
            return false;
        }

        // spawnPoint is optional (falls back to this.transform)
        return true;
    }
}