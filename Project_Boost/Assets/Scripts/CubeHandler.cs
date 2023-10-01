using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {


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
            rb.solverIterations = 10;  // Default is 6
            rb.solverVelocityIterations = 10;  // Default is 1
        }

        // Activate the Rigidbody's physics interactions
        rb.isKinematic = false;

        // Set the velocities
        rb.velocity = linearVelocity;
        rb.angularVelocity = angularVelocity;
        
        rb.solverIterations = 20;  // Adjust to tweak physics checks 

        
        // Cast a ray to check for imminent collisions
        Vector3 direction = linearVelocity.normalized;
        float distance = (linearVelocity * 1.5f).magnitude * Time.fixedDeltaTime;  // Predict a bit further than one frame
        
        float cubeSize = GetComponent<Collider>().bounds.size.magnitude * scaleFactor;  // Getting cube size from collider bounds
        Vector3 frontOffset = 0.2f * cubeSize * direction;  // Calculate the offset to the cube's front
        Vector3 rayOrigin = transform.position + frontOffset;  // Set ray origin to the front of the cube

        RaycastHit hitInfo;

        Debug.DrawRay(rayOrigin, direction*distance, Color.green, 10f); 
        Debug.Log("CrashDetected method called");
        Debug.Log($"Ray Origin: {rayOrigin}, Direction: {direction}, Distance: {distance}");

        
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
                rb.velocity = linearVelocity* 0.75f;
            }
            
            // Set the Rigidbody properties
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            

        }
        
    }
}
