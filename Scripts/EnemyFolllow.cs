using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;        // NavMeshAgent to control movement
    public Transform player;          // Player's transform to follow
    [SerializeField] private float timer = 5f;  // Time between shooting
    private float bulletTime;         // Timer to track shooting intervals
    public GameObject enemybullet;    // Bullet prefab
    public GameObject spawnPoint;     // Bullet spawn point
    public float enemySpeed;          // Speed of the enemy
    public float maxHealth = 100f;    // Maximum health of the enemy
    public float currentHealth;      // Current health of the enemy

    void Start()
    {
        // Initialize current health to max health
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Move towards the player
        enemy.SetDestination(player.position);

        // Handle shooting behavior
        ShootAtPlayer();
    }

    void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;

        if (bulletTime <= 0)
        {
            bulletTime = timer;

            // Instantiate bullet and apply force
            GameObject bulletObj = Instantiate(enemybullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
            Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
            bulletRig.AddForce(spawnPoint.transform.forward * enemySpeed);

            // Destroy bullet after 5 seconds
            Destroy(bulletObj, 5f);
        }
    }

    // Method to take damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method for enemy death
    void Die()
    {
        // Destroy enemy object or disable it
        Destroy(gameObject); // Or you could use gameObject.SetActive(false);
    }
}
