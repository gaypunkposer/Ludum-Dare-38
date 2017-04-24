using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float SurfaceOffset;
    public float Speed;
    public Transform Camera;
    public Transform Guns;
    public bool Demo;

    Vector3 movementInput;
    Vector3 startLocation;
    Quaternion startRotation;

    private void Start()
    {
        startLocation = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (Demo)
        {
            movementInput = new Vector3(1, 1, 0).normalized;
        }
        else
        {
            movementInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;
        }
        
        RaycastHit info;
        if (Physics.Raycast(transform.position, -transform.up, out info, Mathf.Infinity, 1 << 8, QueryTriggerInteraction.UseGlobal))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, info.normal), info.normal), .1f);

            Vector3 transformedMove = Camera.TransformDirection(movementInput) * Speed * Time.deltaTime;

            transform.position = info.point + (transform.up * SurfaceOffset) + Vector3.ProjectOnPlane(transformedMove, info.normal);

            Vector3 gunsinput = new Vector3(Input.GetAxis("Fire Vertical"), 0, Input.GetAxis("Fire Horizontal"));

            if (gunsinput.magnitude < 0.1)
            {

                movementInput.z = movementInput.y;
                movementInput.y = 0;
                if (movementInput.magnitude != 0)
                {
                    Guns.localRotation = Quaternion.Slerp(Guns.localRotation, Quaternion.LookRotation(movementInput), 35f * Time.deltaTime);
                }
            }
            else
            {
                Guns.localRotation = Quaternion.Slerp(Guns.localRotation, Quaternion.LookRotation(gunsinput), 35f * Time.deltaTime);
            }

        }
        else
        {
            Debug.LogError("Shit got fucked real good. Player isn't connected to the planet. What happened? What did you do?");
            transform.position = startLocation;
            transform.rotation = startRotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
