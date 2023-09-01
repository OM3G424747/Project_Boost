using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    // Sets thrust force being applied to rocket
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float turnSpeed = 100f;
    // Stores rocket's RigidBody component to apply movement to it
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        // Stores if the player hit the button for the rocket thrusters
        bool up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);

        // Thusts rocket in the direction its pointed
        if(up)
        {
            // Calculates ammount of relative upwards force to apply relative to the time elapsed since previous frame
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        } 
        // Stalls rocket and allows it to fall down again if not applying force
    }

    void ProcessRotation()
    {
        bool left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        // Turns left only if the right turn key is not pressed at the same time
        if(left && !right)
        {
            // rotates oject to the left (based on camera view)
            ApplyRotation(Vector3.back);
        } 
        // Turns right only if the left key is not pressed at the same time
        else if(right && !left)
        {
             // Rotates oject to the right (based on camera view)
            ApplyRotation(Vector3.forward);
        }

        // creates function for rotating object in a non frame dependant manner based on vector
        void ApplyRotation(Vector3 turnVector)
        {
            // Freeze physics rotation to allow manual rotation
            rb.freezeRotation = true;
            // Rotate in direction selected by player
            transform.Rotate(turnVector * turnSpeed * Time.deltaTime);
            // Unfreeze physics rotation to allow physics rotation again
            rb.freezeRotation = false;
        }

    }
}
