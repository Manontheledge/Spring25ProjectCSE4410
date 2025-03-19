using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWander : MonoBehaviour
{
    //Projectile to shoot
    [SerializeField] GameObject fbprefab;
    private GameObject fball;
    
    //The speed of the enemy and its detection range
    public float speed = 3.0f;
    public const float baseSpeed = 3f;
    public float OBrange = 5.0f;

    //Game Object State
    private bool Living;

    
    
    private void OnEnable()
    {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    private void OnDisable()
    {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    private void OnSpeedChanged(float value)
    {
        speed = baseSpeed * value;
    }

    void Start () 
    { 
        //Set the state to "Alive"
        Living = true; 
    }
    // Update is called once per frame
    void Update()
    {
        //Move forward ONLY when "alive"
        if (Living)
            transform.Translate(0, 0, speed * Time.deltaTime);

        //Generate rays to scan what's in front
        Ray ray =  new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.75f, out hit))
        {
            //Get ref to game object hit
            GameObject hitOB = hit.transform.gameObject;

            //If obj hit was player, shoot fireball
            if (hitOB.GetComponent <PlayerChar>())
            {
                if (fball == null)
                {
                    //Create a fireball the moves forward relative to enemy rotation
                    fball = Instantiate(fbprefab) as GameObject;
                    fball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);   
                    fball.transform.rotation = transform.rotation;
                }
            }
            
            //Otherwise, if the obj is an obstacle AND is too close...
            else if (hit.distance < OBrange)
            {
                //Turn around  at a random angle
                transform.Rotate(0, Random.Range(-110, 110), 0);
            }
        }

    }

    //Public method to set Living state
    public void SetLive(bool live)
    {
        Living = live;
    }
}
