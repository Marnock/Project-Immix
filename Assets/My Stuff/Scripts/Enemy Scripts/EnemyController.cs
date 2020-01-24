using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL, CHASE, ATTACK
}

public class EnemyController : MonoBehaviour
{
    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;
    private EnemyState enemy_State;
    public float walk_Speed = 0.5f;
    public float run_Speed = 4f;
    public float chase_Distance = 7f;
    private float current_Chase_Distance;
    public float attack_Distance = 1.8f;
    public float chase_After_Attack_Distance = 2f;
    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_For_This_Time = 15f;
    private float patrol_Timer;
    public float wait_Before_Attack = 2f;
    private float attack_Timer;

    private Transform target;

    public GameObject attack_Point;
    private EnemyAudio enemy_Audio;

    


    void Awake()
    {
        enemy_Anim = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;
        enemy_Audio = GetComponentInChildren<EnemyAudio>();
    }

    void Start()
    {
        enemy_State = EnemyState.PATROL;

        patrol_Timer = patrol_For_This_Time;

        //when enemy gets to player, attack without timer
        attack_Timer = wait_Before_Attack;

        //know chase distance so we can put it back
        current_Chase_Distance = chase_Distance;

        
       

    }

    void Update()
    {
        if(enemy_State == EnemyState.PATROL)
        {
            Patrol();
        }
        if(enemy_State == EnemyState.CHASE)
        {
            Chase();
        }
        if(enemy_State == EnemyState.ATTACK)
        {
            Attack();
        }
    }

    void Patrol()
    {
        navAgent.isStopped = false;
        navAgent.speed = walk_Speed;

        patrol_Timer += Time.deltaTime;

        if(patrol_Timer > patrol_For_This_Time)
        {
            SetNewRandomDestination();
            patrol_Timer = 0f;
        }

        if(navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Walk(true);
        }
        else
        {
            enemy_Anim.Walk(false);
        }

        //checks if enemy is in chase range
        if(Vector3.Distance(transform.position, target.position) <= chase_Distance)
        {
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.CHASE;
            enemy_Audio.PlayScreamSound();
        }
    }

    void Chase()
    {
        navAgent.isStopped = false;
        navAgent.speed = run_Speed;
        navAgent.SetDestination(target.position);
        

        if(navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Run(true);
        }
        else
        {
            enemy_Anim.Run(false);
        }
        //if target is in attack range
        if(Vector3.Distance(transform.position, target.position) <= attack_Distance)
        {
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.ATTACK;

            //reset the chase distance to previous value
            if(chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }
        }
        //if target runs away
        else if(Vector3.Distance(transform.position, target.position) > chase_Distance) 
        {
            enemy_Anim.Run(false);
            enemy_State = EnemyState.PATROL;
            patrol_Timer = patrol_For_This_Time;

            if(chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }
        }
    }

    void Attack()
    {
        
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;
        attack_Timer += Time.deltaTime;

        if(attack_Timer > wait_Before_Attack)
        {
            transform.LookAt(target);
            enemy_Anim.Attack();
            attack_Timer = 0;
            enemy_Audio.PlayAttackSound();


        }

        if(Vector3.Distance(transform.position, target.position) > attack_Distance + chase_After_Attack_Distance)
        {
            enemy_State = EnemyState.CHASE;
        }
    }

    void SetNewRandomDestination()
    {
        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);
        
        Vector3 randDir = Random.insideUnitCircle * rand_Radius;
        randDir += transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1);
        navAgent.SetDestination(navHit.position);
    }

     void TurnOnAttackPoint()
    {
        attack_Point.SetActive(true);
    }

    void TurnOffAttackPoint() 
    {
        if(attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }

    public EnemyState Enemy_State
    {
       get; set;
    }


  

}
