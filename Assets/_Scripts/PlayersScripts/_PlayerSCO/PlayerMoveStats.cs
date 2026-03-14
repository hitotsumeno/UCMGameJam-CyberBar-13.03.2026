using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMoveStats")]
public class PlayerMoveStats : ScriptableObject
{
    [Header("Walk")]
     public float maxSpeed = 12.5f;
    [Range(0.25f, 50f)] public float Acceleration = 5f;
    [Range(0.25f, 50f)] public float Deceleration = 20f;
}
