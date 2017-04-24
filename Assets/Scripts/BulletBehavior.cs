using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {

    public float Lifetime;
    public float Speed;
    public float SurfaceOffset;
    public GameObject BulletSpawner;
    public bool Queue;
    public bool EnemyFire;

    ColliderBuilder builder;
	void OnEnable () {
        builder = GetComponent<ColliderBuilder>();
        StartCoroutine("TimedDisable");
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit info;
        Vector3 currentUp = transform.up;

        if (Physics.Raycast(transform.position, -currentUp, out info, 5, 1 << 8, QueryTriggerInteraction.UseGlobal))
        {
            Vector3 projectedPlane = Vector3.ProjectOnPlane(transform.forward, info.normal);

            transform.rotation = Quaternion.LookRotation(projectedPlane, info.normal);

            transform.position = info.point + projectedPlane * Speed * Time.deltaTime + currentUp * SurfaceOffset;

        }
        else if (Queue)
        {
            DisableAndEnqueue();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!EnemyFire)
        {
            BulletSpawner.SendMessageUpwards("PlaySound");
            if (other.gameObject.layer == 10 || other.gameObject.layer == 9)
            {
                return;
            }
            else if (Queue)
            {
                other.gameObject.SendMessage("Damage");
                DisableAndEnqueue();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator TimedDisable()
    {
        yield return new WaitForSeconds(Lifetime);

        if (Queue)
        {
            DisableAndEnqueue();
        }
        else
        {
            if (builder != null)
            {
                builder.DestroyColliders();
            }
            Destroy(gameObject);
        }
    }

    void DisableAndEnqueue()
    {
        StopCoroutine("TimedDisable");

        BulletSpawner.GetComponent<BulletSpawner>().BulletQueue.Enqueue(gameObject);
        gameObject.SetActive(false);
    }
}
