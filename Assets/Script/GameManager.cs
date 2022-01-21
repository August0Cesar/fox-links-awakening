using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    IDLE, ALERT, PATROL, FURY, FOLLOW, EXPLORE
}

public class GameManager : MonoBehaviour
{
    public Transform player;
    public float slimeDistanceToAttack = 2.3f;

    [Header("SlimeAI")]
    public float idleWatingTime = 3f;
    public float alertWatingTime = 2f;
    public Transform[] slimeWayPoints;
}
