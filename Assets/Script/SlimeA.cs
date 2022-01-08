using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeA : MonoBehaviour
{
    public int Hp;
    public EnemyState enemyState;
    public const float idleWatingTime = 3f;
    public const float patrolWatingTime = 6f;
    private Animator animator;
    private GameManager _gameManager;
    private bool isDie;

    //AI
    private NavMeshAgent agent;
    private Vector3 destination;

    void Start()
    {
        _gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        ChangeState(enemyState);
    }

    IEnumerator Died(){
        isDie = true;
        yield return new WaitForSeconds(2.3f);
        Destroy(this.gameObject);
    }
    
    public void GetHit(int amountDamge)
    {
        if(isDie){return;}

        Hp -= amountDamge;
        if(Hp > 0){
            animator.SetTrigger("GetHit");
        }else{
            animator.SetTrigger("Die");
            StartCoroutine("Died");
        }
    }

    private void StateManager(){
        switch(enemyState){
            case EnemyState.IDLE:
                break;
            
            case EnemyState.ALERT:
                break;
            
            case EnemyState.PATROL:
                break;
            
            case EnemyState.FURY:
                break;
            
            case EnemyState.FOLLOW:
                break;
            
            case EnemyState.EXPLORE:
                break;

        }
    }

    private void ChangeState(EnemyState newState){
        StopAllCoroutines();

        enemyState = newState;

        switch(enemyState){
            case EnemyState.IDLE:
                destination = transform.position;
                agent.destination = destination;

                StartCoroutine("IDLE");
                break;
            
            case EnemyState.PATROL:
                int indexSlimeWayPoint = Random.Range(0, _gameManager.slimeWayPoints.Length);
                destination = _gameManager.slimeWayPoints[indexSlimeWayPoint].position;
                agent.destination = destination;

                StartCoroutine("PATROL");
                break;

        }
    }

    IEnumerator IDLE()
    {
         yield return new WaitForSeconds(idleWatingTime);
         DecideAndPatrolOrIdle(50);
    }

    IEnumerator PATROL()
    {
         yield return new WaitForSeconds(patrolWatingTime);
        DecideAndPatrolOrIdle(50);
    }

    public void DecideAndPatrolOrIdle(int percentageBackToIdle){
        int rand = Random.Range(0, 100);

        if(rand >= percentageBackToIdle){
             ChangeState(EnemyState.IDLE);
         }else{
             ChangeState(EnemyState.PATROL);
         }
    }
}
