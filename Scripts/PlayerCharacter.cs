using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCharacter : MonoBehaviour{
    private int health;
    public NavMeshAgent agent;

    void Start(){
        agent.updateRotation = false;
        health = 5;
    }
    public void Hurt(int damage){
        health -= damage;
        Debug.Log($"Health: {health}");
    }
    void Update(){
        if(health <= 0){
            Die();
        }

    }
    void Die(){
        Debug.Log("Player is dead");
        Destroy(gameObject);
        // Add game over logic here
        // Example: Load a new scene or show a game over screen
    }
    
}