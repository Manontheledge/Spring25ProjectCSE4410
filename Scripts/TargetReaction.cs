using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetReaction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitReaction()
    {
        //Get ref to AI script, pass in false an instance of the script is found
        EnemyWander behavior = GetComponent<EnemyWander>();
        if (behavior != null) 
            behavior.SetLive(false);
        
        //Die
        StartCoroutine(Ded());
    }

    public IEnumerator Ded()
    {
        //Fall Backwards
        transform.Rotate(-90, 0, 0);

        yield return new WaitForSeconds(1.5f);
        
        Destroy(gameObject);
    }
}
