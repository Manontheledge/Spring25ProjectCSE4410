using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour{
    [SerializeField] GameObject fireballPrefab;
    private GameObject fireball;

    //Speed of object
    public float speed = 3.0f;
    //range of objects
    public float obstacleRange = 5.0f;
    //Objects living statuse
    private bool isAlive;
    private void Start() {
        // setting all starting objects to alive
        isAlive = true;
    }
    
    //update once per frame
    void Update() {

        // Move Forward
        if(isAlive) {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        // create ray fpr wandering object
        Ray ray = new Ray(transform.position, transform.forward);
        // create hit info for raycast
        RaycastHit hit;
        // if ray hits an object within range
        if (Physics.SphereCast(ray, 0.75f, out hit)) {
            GameObject hitObject = hit.transform.gameObject;

            // if object hits player then make fireball
            if(hitObject.GetComponent<PlayerCharacter>()){
                if(fireball == null){
                    fireball = Instantiate(fireballPrefab) as GameObject;
                    fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f );
                    fireball.transform.rotation = transform.rotation;

                }
            // turn around
                else if (hit.distance < obstacleRange){
                    float angle = Random.Range(-110, 110);
                    transform.Rotate(0, angle, 0);
                }
            }
        }        
    }
    // seting alive state outside of script
    public void SetAlive(bool alive){
        isAlive = alive;
    }
}