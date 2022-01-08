using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    IDLE, ALERT, PATROL, FURY, FOLLOW, EXPLORE
}

public class GameManager : MonoBehaviour
{
    [Header("SlimeAI")]
    public Transform[] slimeWayPoints;
    void Start()
    {

    }

    void Update()
    {

    }

    
}
