using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToSurface : MonoBehaviour {

    public float StickForce;
    public float SuccForce = 5;
    
    Vector3 succpoint;
    Vector3 point;
    Rigidbody body;
    GameObject player;
    bool forceApplied;
    // Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 currentPosition = transform.position;
        if (player != null)
        {
                succpoint = (player.transform.position - currentPosition) * SuccForce;
                body.AddForce(succpoint - body.velocity, ForceMode.VelocityChange);
        }
        else
        {
            if (!forceApplied)
            {
                body.AddForce(((Vector3.zero - currentPosition) * StickForce) - body.velocity, ForceMode.VelocityChange);
                forceApplied = true;
            }
            
        }
        
        if (currentPosition.magnitude > 30)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13)
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
