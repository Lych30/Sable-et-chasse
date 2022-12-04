using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movements : MonoBehaviour
{
  
    public Rigidbody rb;
    RaycastHit hit;
    float TargetY = 0;
    public float maxSpeed;
    public bool isJumping = false;
    [Range(1, 3)]
    public float accelerationForce;
    [Range(0,5)]
    public float JumpForce;
    public float JumpCheck = 5;
    Vector3 forward;
    float JumpDrag ;
    ParticleSystem part;

    private void Start()
    {
        part = GetComponentInChildren<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isJumping)
        {
            JumpDrag = 1f;
            JumpCheck = 5;
        }
        else
        {
            JumpDrag = 0.1f;
        }

        if (rb.velocity.y<1 && rb.velocity.y > -50)
        {
            rb.AddForce(Vector3.down*0.5f);
        }

        forward = Camera.main.transform.forward;
        forward = new Vector3(forward.x, 0, forward.z);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            FastMovements();
        }
        else
        {
            SlowMovements();
        }
        
            if (Input.GetButtonDown("Jump") && !isJumping)
            {
                StartCoroutine(JumpCoroutine(0.5f));

            }        
        
        if (Physics.Raycast(transform.position + Vector3.down, transform.TransformDirection(Vector3.down), out hit, JumpCheck) && rb.velocity.magnitude>1.0f)
        {

                isJumping = false;
                part.enableEmission = true;
                TargetY = hit.point.y;
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, TargetY + 1f, transform.position.z), 0.075f);
        
                //transform.position = new Vector3(transform.position.x, TargetY+1, transform.position.z);
                Debug.Log(hit.transform.position.y);
            
       

        }





        Debug.DrawRay(transform.position + Vector3.down, transform.TransformDirection(Vector3.down) * JumpCheck, Color.yellow);

    }
    void FastMovements()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            if ((rb.velocity + (-forward * accelerationForce * Input.GetAxis("Vertical"))).magnitude < maxSpeed)
            {
                rb.AddForce(-forward * Input.GetAxis("Vertical") * accelerationForce);
            }


        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            if ((rb.velocity + (Quaternion.AngleAxis(90, Vector3.up) * forward * accelerationForce)).magnitude < maxSpeed)
            {
                Vector3 right = Quaternion.AngleAxis(90, Vector3.up) * forward;
                rb.AddForce(right * accelerationForce * JumpDrag);
            }

        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if ((rb.velocity + (Quaternion.AngleAxis(-90, Vector3.up) * forward * accelerationForce)).magnitude < maxSpeed)
            {
                Vector3 left = Quaternion.AngleAxis(-90, Vector3.up) * forward;
                rb.AddForce(left * accelerationForce * JumpDrag);
            }

        }
    }

    void SlowMovements()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            if ((rb.velocity + (-forward * Input.GetAxis("Vertical"))).magnitude < 10)
            {
                rb.AddForce(-forward * Input.GetAxis("Vertical"));
            }


        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            if ((rb.velocity + (Quaternion.AngleAxis(90, Vector3.up) * forward)).magnitude < 10)
            {
                Vector3 right = Quaternion.AngleAxis(90, Vector3.up) * forward;
                rb.AddForce(right * JumpDrag);
            }

        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if ((rb.velocity + (Quaternion.AngleAxis(-90, Vector3.up) * forward)).magnitude < 10)
            {
                Vector3 left = Quaternion.AngleAxis(-90, Vector3.up) * forward;
                rb.AddForce(left * JumpDrag);
            }

        }
    }
    IEnumerator JumpCoroutine(float time)
    {
        JumpCheck = 0;
        part.enableEmission = false;
        isJumping = true;
        rb.AddForce(((Quaternion.AngleAxis(Input.GetAxis("Vertical")*90, Vector3.up) * forward * Input.GetAxis("Vertical")) + new Vector3(0, JumpForce, 0)),ForceMode.Impulse);
        yield return new WaitForSeconds(time);
        JumpCheck = 0.1f;


    }
   
}
