using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour{
    public void ReactToHit(){
        WanderingAI behavior = GetComponent<WanderingAI>();
        if(behavior != null){
            behavior.SetAlive(false);
        }

        StartCoroutine(Die());
    }
    public IEnumerator Die(){
        // Object Lay to side as if fainting
        this.transform.Rotate(-75, 0, 0);
        //Wait
        yield return new WaitForSeconds(1.5f);
        // Object disappears
        Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start(){

    }
    //update once per frame
    void Update(){
    
    }

}