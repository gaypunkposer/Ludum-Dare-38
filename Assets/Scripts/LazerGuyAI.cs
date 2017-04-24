 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerGuyAI : MonoBehaviour {

    public float Speed;
    public int MaxHealth;
    public GameObject Fractured;
    public bool Circle;
    public float CooldownTime;
    public float ChargeTime;

    AudioSource source;
    Animation anim;
    MeshRenderer body;
    GameObject Player;
    Color initbody;
    float time = 1;
    int health;
    bool lazerinprogress;
    BulletSpawner spawner;
    ParticleSystem psystem;

    void Start()
    {
        Player = GameObject.Find("Player Parent");
        health = MaxHealth;
        anim = GetComponent<Animation>();
        body = GetComponent<MeshRenderer>();
        initbody = body.material.GetColor("_BGColor");
        spawner = GetComponentInChildren<BulletSpawner>();
        psystem = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(FireLazer());
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        time += Time.deltaTime;
        RaycastHit info;

        if (!lazerinprogress)
        {
            StartCoroutine(FireLazer());
        }

        if (Physics.Raycast(transform.position, -transform.up, out info, 5f, 1 << 8, QueryTriggerInteraction.UseGlobal))
        {
            Vector3 planeProjection = Vector3.ProjectOnPlane(Player.transform.position - transform.position, info.normal);

            transform.rotation = Quaternion.LookRotation(Player.transform.position - transform.position, info.normal);

            //Vector3 transformedMove = (Player.transform.position - transform.position).normalized;

            float dist = Vector3.Distance(transform.position, Player.transform.position);

            transform.position = info.point + transform.up;

            /*
            if (dist <= 10f)
            {
                transform.position = Vector3.Slerp(transform.position, info.point + (transform.up) - planeProjection.normalized * Random.Range(0, 2) + Random.onUnitSphere, Speed * Time.deltaTime);

            }
            else if (dist > 10 && dist <= 15 && Circle)
            {
                Vector3 unitSphere = Vector3.ProjectOnPlane(Random.insideUnitCircle, info.normal);
                transform.position = Vector3.Slerp(transform.position, info.point + (transform.up) - planeProjection.normalized + unitSphere + Random.onUnitSphere, Speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.Slerp(transform.position, info.point + (transform.up) + planeProjection.normalized * Random.Range(0, 2) + Random.onUnitSphere, Speed * Time.deltaTime);
            }
            */
        }
        else
        {
            DestroyImmediate(gameObject);
        }

    }

    IEnumerator FireLazer()
    {
        lazerinprogress = true;
        yield return new WaitForSeconds(CooldownTime);
        if (psystem != null)
        {
            psystem.Play();
        }
        source.Play();
        yield return new WaitForSeconds(ChargeTime);
        spawner.Fire();
        lazerinprogress = false;
    }

    void Damage()
    {

        health--;
        Color col = Color.Lerp(initbody, Color.white, 1 - ((float)health / MaxHealth));
        body.material.SetColor("_BGColor", col);
        if (health <= 0)
        {
            Instantiate(Fractured, transform.position, transform.rotation);
            GameObject.Find("Planet").SendMessage("RemoveEnemy", gameObject);
            Destroy(gameObject);
        }
    }
}
