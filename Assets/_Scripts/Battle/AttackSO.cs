using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class AttackSO : ScriptableObject
{
    public float liveTime = 30f;
    public float damage = 10f;
    public float kb = 0f;
    public bool breaksOnWall = false;
    public bool breaksOnHit = true;
    public bool ownerImmune = true;
}
