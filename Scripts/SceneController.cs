using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour{
    [SerializeField] GameObject enemyPrefab;

    private GameObject enemy;
    //private int activeEnemies = 0;
    void Start() {
    }

    void Update() {
        if (enemy == null){
            enemy = Instantiate(enemyPrefab) as GameObject;
            enemy.transform.position = new Vector3(0, 0, 0);
            float angle = Random.Range(0, 360);
            enemy.transform.Rotate(0, angle, 0);

        }
    }
}