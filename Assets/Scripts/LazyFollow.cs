using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyFollow : MonoBehaviour {

    public Transform Player;
    public float CameraHeight;



    private void Update()
    {
        transform.position = Vector3.Slerp(transform.position, Player.position + Player.up * CameraHeight, 5f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Player.up, Player.forward), 5f * Time.deltaTime);
        Cursor.lockState = CursorLockMode.Locked;
    }
   
}


/*
   FPS Vview. Super cool, entirely unplayable
   private void Update()
    {
        transform.position = Vector3.Slerp(transform.position, Player.position, .1f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Player.rotation, .1f);
    }
*/