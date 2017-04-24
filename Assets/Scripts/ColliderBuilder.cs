using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBuilder : MonoBehaviour {
    public GameObject Collider;

    List<GameObject> colliders = new List<GameObject>();
    Vector3 previousPosition;
	void Start () {
        colliders.Add(Instantiate(Collider, transform.position, transform.rotation, transform));
        previousPosition = transform.position;  		
	}
	
	void Update () {
	    if (Vector3.Distance(previousPosition, transform.position) >= 1)
        {
            colliders.Add(Instantiate(Collider, transform.position, transform.rotation));
            previousPosition = transform.position;
        }
	}
    public void DestroyColliders()
    {
        foreach (GameObject g in colliders) {
            Destroy(g);
        }
    }
}
