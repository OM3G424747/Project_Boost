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
    [SerializeField] float volumeStepRate = 1f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainThrustParticlesL;
    [SerializeField] ParticleSystem mainThrustParticlesR;


    // Stores rocket's RigidBody component to apply movement to it
    Rigidbody rb;
    AudioSource rocketSound;

    // Allows script to run and play effects while the player has not crashed and the value is set to true 
    public bool notCrashed = true;
    public bool hasLanded = false;
    float rocketSoundVolume = 0f;
    // Handles thruster particle states
    bool fireLeftThrusterParticles = false;
    bool fireRightThrusterParticles = false;
    bool upThrustActive = false;
    bool playThrustAudio = false;


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
            upThrustActive = ProcessThrust();
            // stores side thrust particles 
            var sideThrust = ProcessRotation();

            // updates thruster bool values
            fireRightThrusterParticles = upThrustActive || sideThrust.right;
            fireLeftThrusterParticles = upThrustActive || sideThrust.left;
            playThrustAudio = fireRightThrusterParticles || fireLeftThrusterParticles;

            // Fires active thruster values
            ThrustParticleHandler(fireLeftThrusterParticles, fireRightThrusterParticles);
            
            // Plays sound effects based on if the player's thrusters are active or not
            AdjustVolume(playThrustAudio);
        }
        // If the rocket has rashed the audio is turned down 
        else if(hasLanded)
        {
            // Adjusts volume down after landing 
            AdjustVolume(false);
        } 
    }

    // Processes thrust based on keyboard input, returns bool based on input
    bool ProcessThrust()
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

        return up;
    }

    (bool left, bool right) ProcessRotation()
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

        // returns bools based on current keyboard input
        return (left, right);
    }

    // Fires thruster particles based on bool values
    void ThrustParticleHandler(bool fireLeft, bool fireRight)
    {
        // confirms if the right booster should be fired or not
        if (fireRight){
            if (!mainThrustParticlesR.isPlaying ) 
            {
                mainThrustParticlesR.Play();
            }
        }
        else
        {
            mainThrustParticlesR.Stop();
        }
        
        // confirms if the left booster should be fired or not
        if (fireLeft){
            if (!mainThrustParticlesL.isPlaying && fireLeft) 
            {
                mainThrustParticlesL.Play();
            }
        }
        else
        {
            mainThrustParticlesL.Stop();
        }

    }


    // Adjusts volume based on current input
    void AdjustVolume(bool isActive)
    {
        // Sets ammount to increment while active or not active based on time 
        float volumeStepAmmount = volumeStepRate * Time.deltaTime;

        if(isActive)
        {
            // Checks if volume will go over the max limit to the cap it
            if (rocketSoundVolume + volumeStepAmmount > 1)
            {
                rocketSoundVolume = 1;
            }
            else
            {
                rocketSoundVolume += volumeStepAmmount;
            }
        }
        else
        {
            // Checks if volume will go under the max limit to the cap it
            if (rocketSoundVolume - volumeStepAmmount < 0)
            {
                rocketSoundVolume = 0;
            }
            else
            {
                rocketSoundVolume -= volumeStepAmmount;
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
