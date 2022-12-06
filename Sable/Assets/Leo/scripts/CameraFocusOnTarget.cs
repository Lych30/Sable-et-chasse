using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusOnTarget : MonoBehaviour
{
    public Transform target;
    private Transform PlayerTransform;
    float MinY;
    RaycastHit hit;
    private void Awake()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = (target.position - PlayerTransform.position).normalized;
        transform.position = Vector3.Lerp(transform.position, PlayerTransform.position - (direction * 20),0.2f);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 3))
        {
            MinY = hit.point.y + 3;
            Debug.Log("hit");
        }
        else
        {
            MinY = PlayerTransform.position.y;
        }
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 3, Color.yellow);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, MinY, Mathf.Infinity), transform.position.z);
        transform.LookAt(target);
    }
}
