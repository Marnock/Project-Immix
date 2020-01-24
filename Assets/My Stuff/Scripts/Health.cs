using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;
    private EnemyController enemy_Contrller;
    public float health = 100f;
    public bool is_Player, is_Wolf;
    private bool is_Dead;

    private EnemyAudio enemyAudio;
  

   
    void Awake()
    {
        if(is_Wolf)
        {
            enemy_Anim = GetComponent<EnemyAnimator>();
            enemy_Contrller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();
            enemyAudio = GetComponentInChildren<EnemyAudio>();

        }
        if(is_Player)
        {
            //TODO have some form of player stats
        }
        
    }

    public void ApplyDamage(float damage)
    {
        if(is_Dead) return;

        health -= damage;

        
        if(is_Player)
        {
  
            //TODO have some form of player stats

        }
        if(is_Wolf)
        {
            if(enemy_Contrller.Enemy_State == EnemyState.PATROL)
            {
                enemy_Contrller.chase_Distance = 50f;
            }
        }
        if(health <= 0f)
        {
           TriggerDeath(); 
           is_Dead = true;
        }
        
    }

    void TriggerDeath()
    {
        if(is_Wolf)
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemy_Contrller.enabled = false;
            enemy_Anim.Dead();
    
        }
        if(is_Player)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);
            for(int i = 0; i< enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }
          

            GetComponent<PlayerMovement>().enabled = false;

        }

        if(tag == Tags.PLAYER_TAG)
        {
            Invoke("RestartGame", 3f);
        }
        else
        {
            Invoke("TurnOffGameObject", 20f);
        }
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    }

    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }

}
