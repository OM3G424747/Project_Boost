using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    // Sets thrust force being applied to rocket
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float turnSpeed = 100f;
    // Set Value to How quickly the rocket audio should ramp up to max
    [SerializeField] float volumeIncrementRate = 1f;

    [SerializeField] AudioClip mainEngine;

    // Stores rocket's RigidBody component to apply movement to it
    Rigidbody rb;
    AudioSource rocketSound;

    // Allows script to run and play effects while the player has not crashed and the value is set to true 
    public bool notCrashed = true;
    public bool hasLanded = false;
    float rocketSoundVolume = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rocketSound = GetComponent<AudioSource>();

        // Sets rocket volume to 0 to avoid playing any audio at launch of game 
        rocketSound.volume = rocketSoundVolume;
    }

    // Update is called once per frame
    void Update()
    {
        // Allows the player control as long as they have not crashed
        if (notCrashed && !hasLanded)
        {
            ProcessThrust();
            ProcessRotation();
        }
        // If the rocket has rashed the audio is turned down 
        else if(hasLanded)
        {
            // Adjusts volume down after landing 
            AdjustVolume(false);

        } 


    }

    void ProcessThrust()
    {
        // Stores if the player hit the button for the rocket thrusters
        bool up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);

        // Adjusts audio effect volume based on player input
        AdjustVolume(up);

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


    void AdjustVolume(bool isActive)
    {
        // Sets ammount to increment while active or not active based on time 
        float incrementAmmount = volumeIncrementRate * Time.deltaTime;

        if(isActive)
        {
            // Checks if volume will go over the max limit to the cap it
            if(rocketSoundVolume + incrementAmmount > 1)
            {
                rocketSoundVolume = 1;
            }
            else
            {
                rocketSoundVolume += incrementAmmount;
            }
        }
        else
        {
            // Checks if volume will go under the max limit to the cap it
            if(rocketSoundVolume - incrementAmmount < 0)
            {
                rocketSoundVolume = 0;
            }
            else
            {
                rocketSoundVolume -= incrementAmmount;
            }
        }
        
        // Plays audio with new set level
        rocketSound.volume = rocketSoundVolume;
        
        // Pauses audio if it's no longer loud enough 
        // Alternatively confirms if audio is not playing to play it
        if(rocketSoundVolume != 0 && !rocketSound.isPlaying)
        {
            rocketSound.PlayOneShot(mainEngine);
        }
        else if (rocketSoundVolume == 0 && rocketSound.isPlaying)
        {
            rocketSound.Pause();
        }        

    }
}
