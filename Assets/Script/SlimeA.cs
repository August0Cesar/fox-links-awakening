using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeA : MonoBehaviour
{
    public int Hp;
    public Transform HitBox;
    [Range(0.2f, 1)]
    public float HitRange = 3;
    public EnemyState enemyState;
    public LayerMask PlayerViewLayer;

    public Collider[] colliders;
    private Animator animator;
    private GameManager _gameManager;

    public bool isPlayerVisible;
    private bool isDie;
    private bool isWalk;
    public bool isAlert;

    //AI
    private NavMeshAgent agent;
    private Vector3 destination;

    void Start()
    {
        _gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // ChangeState(enemyState);
    }

    void Update()
    {
        StateManager();
        applyWalk();
        playerInFieldOfVision();
        animator.SetBool("isAlert", isAlert);
    }

    void applyWalk()
    {

        if (agent.velocity.magnitude > 0.1f)
        {
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }
        animator.SetBool("isWalk", isWalk);
    }
    IEnumerator Died()
    {
        isDie = true;
        yield return new WaitForSeconds(2.3f);
        Destroy(this.gameObject);
    }

    private void playerInFieldOfVision()
    {
        colliders = Physics.OverlapSphere(HitBox.position, HitRange);
        foreach (Collider c in colliders)
        {

            if (c.gameObject.tag == "Player" && (enemyState == EnemyState.IDLE || enemyState == EnemyState.PATROL) && !isPlayerVisible)
            {
                isPlayerVisible = true;
                ChangeState(EnemyState.ALERT);
            }
            else
            {
                isPlayerVisible = false;
                Debug.Log("Longe do Player");
            }
        }
    }

    public void GetHit(int amountDamge)
    {
        if (isDie) { return; }

        Hp -= amountDamge;
        if (Hp > 0)
        {
            ChangeState(EnemyState.FURY);
            animator.SetTrigger("GetHit");
        }
        else
        {
            animator.SetTrigger("Die");
            StartCoroutine("Died");
        }
    }

    void StateManager()
    {
        switch (enemyState)
        {
            case EnemyState.FURY:
                destination = _gameManager.player.position;
                agent.destination = destination;
                break;

            case EnemyState.FOLLOW:
                destination = _gameManager.player.position;
                agent.destination = destination;
                break;

            case EnemyState.IDLE:
                break;

            case EnemyState.PATROL:
                break;

            case EnemyState.ALERT:
                break;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        StopAllCoroutines();

        enemyState = newState;

        // isAlert = false;
        // Debug.Log("my current state" + enemyState);
        
        switch (enemyState)
        {
            case EnemyState.FOLLOW:
                Debug.Log("Estou no FOLLOW");
                break;

            case EnemyState.ALERT:
                agent.stoppingDistance = 0;
                destination = transform.position;
                agent.destination = destination;

                isAlert = true;
                StartCoroutine("ALERT");
                break;

            case EnemyState.IDLE:
                agent.stoppingDistance = 0;
                destination = transform.position;
                agent.destination = destination;

                StartCoroutine("IDLE");
                break;

            case EnemyState.PATROL:
                agent.stoppingDistance = 0;
                int indexSlimeWayPoint = Random.Range(0, _gameManager.slimeWayPoints.Length);
                destination = _gameManager.slimeWayPoints[indexSlimeWayPoint].position;
                agent.destination = destination;

                StartCoroutine("PATROL");
                agent.stoppingDistance = _gameManager.slimeDistanceToAttack;
                break;

            case EnemyState.FURY:
                agent.stoppingDistance = _gameManager.slimeDistanceToAttack;
                break;

        }
    }

    IEnumerator ALERT()
    {
        yield return new WaitForSeconds(_gameManager.alertWatingTime);
        if (isPlayerVisible)
        {
            Debug.Log("Chamdando metodo ChangeState");
            ChangeState(EnemyState.FOLLOW);
        }
        else
        {
            DecideAndPatrolOrIdle(60);
        }
    }
    IEnumerator IDLE()
    {
        yield return new WaitForSeconds(_gameManager.idleWatingTime);
        DecideAndPatrolOrIdle(50);
    }

    IEnumerator PATROL()
    {
        yield return new WaitUntil(() => agent.remainingDistance <= 0);
        DecideAndPatrolOrIdle(50);
    }

    public void DecideAndPatrolOrIdle(int percentageBackToIdle)
    {
        int rand = Random.Range(0, 100);

        if (rand >= percentageBackToIdle)
        {
            ChangeState(EnemyState.IDLE);
        }
        else
        {
            ChangeState(EnemyState.PATROL);
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(HitBox.position, HitRange);
    }
}
