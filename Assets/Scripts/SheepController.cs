using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour {

    public float speed;
    private Rigidbody sheep;

    // Use this for initialization
    void Start () {
        sheep = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update ()
    {
        float moveHorizontal = 0;
        float moveVertical = 1;

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        sheep.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Change Direction"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
