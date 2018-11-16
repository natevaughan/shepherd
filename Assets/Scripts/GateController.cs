using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour {

    private bool animate = false;
    private bool open = true;
    private int xDistance = 0;
    private int xDistanceMin = 0;
    public int xDistanceMax = 3; // from 60
    private int speed = 5;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (animate)
        {
            if (!open && xDistance >= xDistanceMin)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                xDistance--;
            }
            else if (open && xDistance <= xDistanceMax)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                xDistance++;
            }
            if (xDistance == xDistanceMin || xDistance == xDistanceMax)
            {
                open = !open;
                animate = !animate;
            }
        }
    }

    private void OnMouseDown()
    {
        animate = !animate;
    }
}
