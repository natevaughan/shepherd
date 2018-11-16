using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herder : MonoBehaviour {


    public GameObject sheepPrefab;

    public Transform spawnPoint;

    public int spawnCount = 10;

    public float spawnRadius = 4.0f;

    [Range(0.1f, 20.0f)]
    public float velocity = 6.0f;

    [Range(0.0f, 0.9f)]
    public float velocityVariation = 0.5f;

    [Range(0.1f, 20.0f)]
    public float rotationCoeff = 4.0f;

    [Range(0.1f, 10.0f)]
    public float neighborDist = 2.0f;

    public LayerMask searchLayer;

    void Start()
    {
        // GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");


        // foreach(GameObject t in gameObjects) {
        //     this.spawnPoint = t.transform;    
        // }


        for (var i = 0; i < this.spawnCount; i++) {
            this.Spawn();
        }
    }


    public GameObject Spawn()
    {
        return Spawn((spawnPoint.position + Get2DPosition(Random.insideUnitSphere) * spawnRadius));
    }

    public GameObject Spawn(Vector3 position)
    {
        var rotation = Quaternion.Slerp(transform.rotation, Random.rotation, 0.3f);
        var sheep = Instantiate(sheepPrefab, position, rotation) as GameObject;
        return sheep;
    }

    Vector3 Get2DPosition(Vector3 pos) {
        return new Vector3(pos.x, 0.0f, pos.z);
    }
}
