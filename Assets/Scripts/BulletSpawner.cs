using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {

    public GameObject BulletPrefab;
    public Transform Parent;
    public float FireRate;
    public bool PlayerControlled;
    public bool Queue;
    public Queue<GameObject> BulletQueue = new Queue<GameObject>();
    public bool Audio;

    float timer;
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        if (Queue)
        {
            for (int i = 0; i < FireRate + 1; i++)
            {
                GameObject bullet = Instantiate(BulletPrefab) as GameObject;
                bullet.GetComponent<BulletBehavior>().BulletSpawner = gameObject;
                bullet.SetActive(false);
                BulletQueue.Enqueue(bullet);
            }
        }

    }

	void Update () {
        if (PlayerControlled)
        {
            Vector3 gunsinput = new Vector3(Input.GetAxis("Fire Vertical"), 0, Input.GetAxis("Fire Horizontal"));
            if (gunsinput.magnitude > 0 && timer >= 1 / FireRate)
            {
                Fire();
                timer = 0f;
            }
        }


        timer += Time.deltaTime;
	}

    public void Fire()
    {
        //Instantiate(BulletPrefab, transform.position + transform.up, Parent.rotation * transform.parent.localRotation);
        if (BulletQueue.Count > 0 && Queue)
        {
            if (Audio)
            {
                source.Play();
            }
            GameObject currentBullet = BulletQueue.Dequeue();
            currentBullet.SetActive(true);
            currentBullet.transform.position = transform.position + transform.up;
            currentBullet.transform.rotation = Parent.rotation * transform.parent.localRotation;
        }
        else
        {

            Instantiate(BulletPrefab, transform.position, Parent.rotation * transform.localRotation);
        }
        
    }

}
