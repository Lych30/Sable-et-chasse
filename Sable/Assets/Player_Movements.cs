using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movements : MonoBehaviour
{
  
    public Rigidbody rb;
    RaycastHit hit;
    float TargetY = 0;
    public float maxSpeed;
    bool canJump = false;
    bool isJumping = false;
    [Range(1, 3)]
    public float accelerationForce;
    [Range(0,5)]
    public float JumpForce;
    public float JumpCheck;
    Vector3 forward;
    ParticleSystem part;

    private void Start()
    {
        part = GetComponentInChildren<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        forward = Camera.main.transform.forward;
        forward = new Vector3(forward.x, 0, forward.z);
        if (Input.GetAxis("Vertical") != 0)
        {
            if((rb.velocity+(-forward * accelerationForce * Input.GetAxis("Vertical"))).magnitude< maxSpeed)
                rb.AddForce(-forward * Input.GetAxis("Vertical")* accelerationForce);
            
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            if ((rb.velocity + (Quaternion.AngleAxis(90, Vector3.up) * forward* accelerationForce)).magnitude < maxSpeed)
            {
                Vector3 right = Quaternion.AngleAxis(90, Vector3.up) * forward;
                rb.AddForce(right* accelerationForce);
            }

        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if ((rb.velocity + (Quaternion.AngleAxis(-90, Vector3.up) * forward* accelerationForce)).magnitude < maxSpeed)
            {
                Vector3 left = Quaternion.AngleAxis(-90, Vector3.up) * forward;
                rb.AddForce(left* accelerationForce);
            }

        }
        if (Input.GetButtonDown("Jump")&& canJump)
        {
            StartCoroutine(JumpCoroutine(1));
        }
        
        if (Physics.Raycast(transform.position + Vector3.down, transform.TransformDirection(Vector3.down), out hit, JumpCheck) && rb.velocity.magnitude>1.0f)
        {
            if (!isJumping)
            {
                part.enableEmission = true;
                canJump = true;
                TargetY = hit.point.y;
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, TargetY + 1.1f, transform.position.z), 0.05f);;
                //transform.position = new Vector3(transform.position.x, TargetY+1, transform.position.z);
                Debug.Log(hit.transform.position.y);
            }
            else
            {
                canJump = false;
            }

        }
        


        Debug.DrawRay(transform.position + Vector3.down, transform.TransformDirection(Vector3.down) * JumpCheck, Color.yellow);

    }
    IEnumerator JumpCoroutine(float time)
    {
        part.enableEmission = false;
        isJumping = true;
        rb.AddForce(((Quaternion.AngleAxis(Input.GetAxis("Vertical")*90, Vector3.up) * forward * Input.GetAxis("Vertical")) + new Vector3(0, JumpForce, 0)),ForceMode.Impulse);
        yield return new WaitForSeconds(time);
        isJumping = false;

    }
   
}
