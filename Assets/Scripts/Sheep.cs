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
    public Vector3 destination;
    public bool repel;


    //Time out stuff//
    bool falling = false;
    private float FallDuration = 2.5f;
    private float FallTime = 0.0f;

    bool wander = false;


    // Use this for initialization
    void Start () {
        repel = false;
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        navMeshAgent.speed = Random.Range(0.1f, 1.0f); 
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
        if (other.gameObject.CompareTag( "BorderWall" )) {
            // Vector3 direction = (Vector3.Reflect(CurrentPosition(), Vector3.right) * 3f).normalized;
            // this.destination = CurrentPosition() + direction;
            // this.navMeshAgent.SetDestination(this.destination);
            // Debug.DrawRay(CurrentPosition(), this.destination, Color.red, 1.0f);
            // Debug.Log("Hit!");
            repel =!repel;
        }
        
        if (other.gameObject.CompareTag( "PitFall")) {
            ToFallingState();
        }
    }

    // Update is called once per frame
    void Update () 
    {

        if (Input.GetKeyDown(KeyCode.Space)) repel = !repel;
        
        if (!repel && IsNavMeshEnabled()) {
            this.navMeshAgent.SetDestination(PointOfInterest()); 
        } else if (falling) {
            Falling();
            
        } else if (repel){
            RunAway();
        }
    }

    void RunAway()
    {
        // Debug.Log(this.ID.ToString() + "RUUUUUNN!!!! " + this.activePredator.ID.ToString());
        Vector3 direction = (CurrentPosition() - PointOfInterest()).normalized;
        this.destination = CurrentPosition() + direction;

        // // Sample the provided Mesh Area and get the nearest point
        if (IsNavMeshEnabled() && NavMesh.SamplePosition( this.destination, out this.nmHit, this.navMeshAgent.height*2, this.MeshArea)) {
            this.navMeshAgent.SetDestination( this.nmHit.position );
            // Debug.DrawRay(this.nmHit.position, direction, Color.yellow, 1.0f);
        }

        // Meander();

        // float safety = Vector3.Distance(CurrentPosition(), PointOfInterest());
        if ( IsNavMeshEnabled() && this.navMeshAgent.remainingDistance < 0.05f) {
            Wander();
        }   
    }


    void ToFallingState()
    {
        var rb = GetComponent<Rigidbody>();
        this.navMeshAgent.enabled = false;
        rb.isKinematic = false;
        rb.detectCollisions = false;
        falling = true;
    }

    void Falling()
    {
        if (FallTime >= FallDuration) {
            Object.Destroy(this.gameObject);
        } else {
            FallTime += Time.deltaTime;
        }
    }
    
    void Meander() 
    {
        float distance = Random.Range(1, 3);
        Vector3 direction = Random.insideUnitCircle;
        direction.z = direction.y;
        direction.y = 0;
        direction += PointOfInterest();

        if (NavMesh.SamplePosition( direction, out this.nmHit, distance, this.MeshArea)) {
            this.navMeshAgent.SetDestination( this.nmHit.position );
            // Debug.DrawRay(this.nmHit.position, direction, Color.yellow, 1.0f);
        }

    }

    void Wander() {
        if ( IsNavMeshEnabled() && this.navMeshAgent.remainingDistance < 0.05f) {
            float distance = Random.Range(1f, 4f);
            Vector3 direction = Random.insideUnitCircle * distance;
            direction.z = direction.y;
            direction.y = 0;
            direction += CurrentPosition();
            NavMesh.SamplePosition( direction, out this.nmHit, distance, this.MeshArea  );
            // Debug.Log("Am I alive? " + this.navMeshAgent.enabled);
            if (IsNavMeshEnabled()) {
                this.navMeshAgent.SetDestination( this.nmHit.position );
            }
        }
    }

    void ExecuteCurrentState()
    {
        if (falling) Falling();
        if (wander) Wander();
    }


    bool IsNavMeshEnabled()
    {
        return this.navMeshAgent.enabled;
    }

    Vector3 CurrentPosition() 
    {
        return this.transform.position;
    }


    Vector3 PointOfInterest()
    {
        return this.pointOfInterest.transform.position;
    }
}
