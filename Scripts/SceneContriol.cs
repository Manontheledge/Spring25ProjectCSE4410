using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneContriol : MonoBehaviour
{
    //Specify the spawned prefab
    [SerializeField] GameObject prefab;

    //Priv field to track one enemy instance
    private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If there is no enemy, spanw 1 enemy
        if (enemy == null)
        {
            enemy = Instantiate(prefab) as GameObject;
            enemy.transform.position = new Vector3 (0,1,0);
            enemy.transform.Rotate(0, Random.Range(0,360),0);
        }
    }
}
