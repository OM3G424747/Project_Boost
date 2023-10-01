using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    Rigidbody rb;

    private bool hasCollided = false;

    // Start is called before the first frame update
    void Start()
    {


    }


    // Update is called once per frame
    void Update()
    {


    }

    public void CrashDetected(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        // Check if a Rigidbody exists, if not, add one
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Set the velocities
        rb.velocity = linearVelocity;
        rb.angularVelocity = angularVelocity;

        // Activate the Rigidbody's physics interactions
        rb.isKinematic = false;
    }

}
