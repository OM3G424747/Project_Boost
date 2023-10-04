using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    Rigidbody rb;
    Collider collide;
    float cubeSize; 

    // Start is called before the first frame update
    void Start()
    {
        collide = GetComponent<Collider>();
    }


    // Update is called once per frame
    void Update()
    {
        

    }

    public void CrashDetected(Vector3 linearVelocity, Vector3 angularVelocity, float scaleFactor)
    {
        // Check if a Rigidbody exists, if not, add one
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Activate the Rigidbody's physics interactions
        rb.isKinematic = false;

        // Set the velocities
        rb.velocity = linearVelocity;
        rb.angularVelocity = angularVelocity;
        
        // Cast a ray to check for imminent collisions
        Vector3 direction = linearVelocity.normalized;
        float distance = (linearVelocity * 1.5f).magnitude * Time.fixedDeltaTime;  // Predict a bit further than one frame
        
        cubeSize = collide.bounds.size.magnitude * scaleFactor;  // Getting cube size from collider bounds
        Vector3 frontOffset = 0.2f * cubeSize * direction;  // Calculate the offset to the cube's front
        Vector3 rayOrigin = transform.position + frontOffset;  // Set ray origin to the front of the cube
        RaycastHit hitInfo;

        if (Physics.Raycast(rayOrigin, direction, out hitInfo, distance))
        {   
            
            // Check if the hit object is untagged
            if (hitInfo.collider.gameObject.CompareTag("Untagged"))
            {
                transform.position = hitInfo.point - frontOffset;
                // Handle the imminent collision by halving the velocity
                rb.velocity = Vector3.zero;
            }
            else
            {
                // Halfs velocity if an impact is imminent
                rb.velocity = linearVelocity* 0.90f;
            }
            
            // Set the Rigidbody properties
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        }
        
    }
}
