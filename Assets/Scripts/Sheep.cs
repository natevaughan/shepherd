using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Sheep : MonoBehaviour {


    ///////////////////////////
    // NavMesh related
    ///////////////////////////
    public int MeshArea;
    protected int layerMask;
    public float navMeshRadius;
    protected NavMeshHit nmHit;
    public NavMeshAgent navMeshAgent;

    ///////////////////////////////////
    /// Navigation Points of Interest
    ///////////////////////////////////
    public Transform pointOfInterest;
    public Vector3 heading;
    public bool repel;


    // Use this for initialization
    void Start () {
        repel = false;
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        navMeshAgent.speed = Random.Range(1.0f, 3.0f); 
        // navMeshAgent.avoidancePriority = Random.Range(0, 100);  // Randomly set the avoidance priority
        navMeshRadius = navMeshAgent.radius;
        MeshArea = 1 << NavMesh.GetAreaFromName("Walkable");

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Destination");
        foreach(GameObject t in gameObjects) {
            this.pointOfInterest = t.transform;    
        }
        Debug.Log(this.pointOfInterest);
    }
    
    public void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag( "BorderWall" ))
            Object.Destroy(this.gameObject);
        else
            repel = !repel;
    }

    // Update is called once per frame
    void Update () 
    {

        if (Input.GetKeyDown(KeyCode.Space)) repel = !repel;
        
        if (!repel) {
            this.navMeshAgent.acceleration = 8;
            this.navMeshAgent.SetDestination(this.pointOfInterest.position); 
            // Debug.DrawRay(this.transform.position, this.pointOfInterest.position.normalized, Color.red, 1.0f);
        } else {
            RunAway();
        }
    }

    void RunAway()
    {
        // Debug.Log(this.ID.ToString() + "RUUUUUNN!!!! " + this.activePredator.ID.ToString());
        Vector3 direction = (this.transform.position - this.pointOfInterest.transform.position).normalized;

        Vector3 destination = this.transform.position + direction;

        // Sample the provided Mesh Area and get the nearest point
        if (NavMesh.SamplePosition( destination, out this.nmHit, this.navMeshAgent.height*2, this.MeshArea )) {
            this.navMeshAgent.SetDestination( this.nmHit.position );
            // Debug.DrawRay(this.nmHit.position, direction, Color.yellow, 1.0f);
        }

        float safety = Vector3.Distance(this.transform.position, this.pointOfInterest.transform.position);
        if ( this.navMeshAgent.remainingDistance < 0.05f) {
            repel = !repel;
        }
        
    }


}
