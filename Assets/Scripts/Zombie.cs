using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    private int health = 100;
    [SerializeField]
    private float timeBetweenAttacks = 5;
    [SerializeField]
    private int attackDamage = 10;
    
    private Animator animator;
    private NavMeshAgent nav;
    private GameObject playerObject;
    private PlayerHealth playerHealth;
    private float timer = 0;
    private bool isPlayerInRange;
    private CapsuleCollider capsuleCollider;
    private bool isDead;

    void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerHealth = playerObject.GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        capsuleCollider =  GetComponent<CapsuleCollider>();
    }


    void Update()
    {
        if (health > 0)
        {
            nav.SetDestination(playerObject.transform.position);
        }
        else
        {
            nav.enabled = false;
        }

        timer += Time.deltaTime;

        if (timer >= timeBetweenAttacks && isPlayerInRange && health > 0)
        {
            Attack();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerObject)
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerObject)
        {
            isPlayerInRange = false;
            animator.SetTrigger("Walk");
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            Death();
            
        }
    }

    private void Attack()
    {
        timer = 0;
        if (playerHealth.health > 0)
        {
            animator.SetTrigger("Attack");
            playerHealth.TakeDamage(attackDamage);
        }

    }

    private void Death()
    {
        nav.enabled = false;
        capsuleCollider.isTrigger = true;
        animator.SetTrigger("Dead");
        Destroy(gameObject, 2);
    }
}
