using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyDoodleAI : MonoBehaviour
{

    public float Speed;
    public int MaxHealth;
    public MeshRenderer Body;
    public MeshRenderer Wings;
    public GameObject Fractured;
    public bool Circle;

    AudioSource source;
    float minDistance = 0.5f;
    float maxDistance = 5f;
    bool skipFrame;
    GameObject Player;
    Color initbody;
    Color initwings;
    float time = 1;
    int health;
    Rigidbody body;
    void Start()
    {
        Player = GameObject.Find("Player Parent");
        health = MaxHealth;
        initwings = Wings.material.GetColor("_BGColor");
        initbody = Body.material.GetColor("_BGColor");
        body = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (skipFrame)
        {
            skipFrame = false;
            return;
            
        }
        else
        {
            Vector3 playerPosition = Player.transform.position;
            Vector3 currentPosition = transform.position;
            Vector3 currentUp = transform.up;

            RaycastHit info;
            if (Physics.Raycast(currentPosition, -currentUp, out info, 5f, 1 << 8, QueryTriggerInteraction.UseGlobal))
            {

                Vector3 planeProjection = Vector3.ProjectOnPlane(playerPosition - currentPosition, info.normal);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(planeProjection, info.normal), .1f);

                //Vector3 transformedMove = (Player.transform.position - transform.position).normalized;

                float dist = Vector3.Distance(currentPosition, playerPosition);


                if (dist <= minDistance)
                {
                    transform.position = Vector3.Slerp(currentPosition, info.point + (currentUp) - planeProjection.normalized * Random.Range(0, 2) + Random.onUnitSphere, Speed * Time.deltaTime);

                }
                else if (dist > minDistance && dist <= maxDistance && Circle)
                {
                    transform.position = Vector3.Slerp(currentPosition, info.point + (currentUp) - planeProjection.normalized * Random.Range(0,2) + Random.onUnitSphere, Speed * Time.deltaTime);

                }
                else
                {
                    transform.position = Vector3.Slerp(currentPosition, info.point + (currentUp) + planeProjection.normalized * Random.Range(0, 2) + Random.onUnitSphere, Speed * Time.deltaTime);
                }

                Speed += Time.deltaTime;

            }
            else
            {
                DestroyImmediate(gameObject);
            }
            skipFrame = true;
            minDistance -= Time.deltaTime;
            maxDistance -= Time.deltaTime;
        }
       


    }

    void Damage()
    {

        health--;
        Body.material.SetColor("_BGColor", Color.Lerp(initbody, Color.white, health / MaxHealth));
        Wings.material.SetColor("_BGColor", Color.Lerp(initwings, Color.white, health / MaxHealth));
        if (health <= 0)
        {
            Instantiate(Fractured, transform.position, transform.rotation);
            GameObject.Find("Planet").SendMessage("RemoveEnemy", gameObject);
            Destroy(gameObject);
        }
    }
}
