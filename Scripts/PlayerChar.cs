using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : MonoBehaviour
{
    private int health;
    
    // Start is called before the first frame update
    void Start()
    {
        health = 5;
    }

    public void Hurt(int dmg)
    { 
        health -= dmg;
        UnityEngine.Debug.Log("Health: " + health);
    }
      

}
