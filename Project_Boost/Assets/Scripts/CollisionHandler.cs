using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class CollisionHandler : MonoBehaviour
{

    [SerializeField] bool enableDebugKeys = true;
    [SerializeField] float reloadLevelDelay = 3f;
    [SerializeField] float loadNextLevelDelay = 3f;
    [SerializeField] AudioClip crashImpact;
    [SerializeField] AudioClip levelPass;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    // Stores rocket movement component, sound and collider
    Rigidbody rb;
    Movement movement;
    AudioSource rocketFXSound;
    Collider[] hitBox;

    // Stores most recent vectors to maintain velocity of cubes on impact
    Vector3 previousLinearVelocity;
    Vector3 previousAngularVelocity;
    float scaleFactor;
    bool cheatsActive;

    // Confirms if the player has landed or crashed to prevent additional audio
    bool isTransitioning = false;

    void Start()
    {
        // Gets Rigidbody for calculating velocity
        rb = GetComponent<Rigidbody>();
        // Accesses component at the start of the level load.
        movement = GetComponent<Movement>();
        rocketFXSound = GetComponent<AudioSource>();
        // Gets array of colliders
        hitBox = GetComponents<Collider>();
        // calculate sphere radius for rocket collision detection
        scaleFactor = transform.localScale.x;
    }

    void Update()
    {
        RespondToDebugKeys(enableDebugKeys);

        // Update the previous velocities with the current velocities
        previousLinearVelocity = rb.velocity;
        previousAngularVelocity = rb.angularVelocity;

    }



    void OnCollisionEnter(Collision other)
    {
        // Confirms players hasn't touched any other objects before
        // Skips switch statement if true.
        
        if (isTransitioning || cheatsActive){return;}

        switch (other.gameObject.tag)
        {
            case "Finish":
                StartLanding();
                break;

            case "Friendly":
                Debug.Log("Standing on friendly platform");
                break;
            
            // Defaults to the player hitting a differe object
            default:
                // Crash audio clip and disables player controls
                StartCrash();
                break;

        }
    }


    //TODO - Update to not play a crash or a success if one or the other has been triggered

    // Used to call methods associated with the player crashing the rocket
    void StartCrash()
    {
        // TODO - Randomize sounds and adjust according to speed of impact

        isTransitioning = true;
        // Disables movement and loads level again
        movement.notCrashed = false;
        // Stops all other audio clips, adjusts volume and plays crash audio
        PlayOnlySelectedAudio(crashImpact, 1f);
        // Plays explosion particles
        crashParticles.Play();
        // Movement.enabled = false;
        Invoke( "ReloadLevel", reloadLevelDelay);

        // Loops over colliders and dissables them for main ship body
        foreach (Collider col in hitBox)
        {
            col.enabled = false;
        }

        // Inform all the child cubes about the crash
        foreach (CubeHandler cubeScript in GetComponentsInChildren<CubeHandler>())
        {
            cubeScript.CrashDetected(previousLinearVelocity, previousAngularVelocity, scaleFactor);
        }

    }

    void StartLanding()
    {
        isTransitioning = true;
        // disables movement and loads next level
        movement.notCrashed = false;
        // Stops all other audio clips, adjusts volume and plays success audio tone
        PlayOnlySelectedAudio(levelPass, 1f);
        // Plays success particles after landing successfully
        successParticles.Play();
        //movement.enabled = false;
        Invoke( "LoadNextLevel", loadNextLevelDelay);

    }

    // Reloads Scene after hitting an object 
    void ReloadLevel()
    {
        //TODO -- Play particle effects and explosion

        // Sets index of current level in build
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Reloads the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Loads next level after hitting the finish
    void LoadNextLevel()
    {
        //TODO -- Play success sound effect and fireworks / particle effects

        // Sets index of next level in build
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Resets scene index if the max number has been reached
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        // Loads next scene
        SceneManager.LoadScene(nextSceneIndex);
    }

    // Function halts all other audio clips being played and only plays single audio clip at selected audio level
    void PlayOnlySelectedAudio(AudioClip audioClip, float audioLevel)
    {
        rocketFXSound.Stop();
        rocketFXSound.volume = audioLevel;
        rocketFXSound.PlayOneShot(audioClip, audioLevel);
    }

    // Enables or disabled debug keys for game
    void RespondToDebugKeys(bool isActive)
    {
        if(isActive)
        {
            // checks if L was pressed to load the next level
            if (Input.GetKey(KeyCode.L))
            {
                // loads next level with zero delay
                LoadNextLevel();
            }

            // checks if C was pressed to enable cheats
            if (Input.GetKey(KeyCode.C))
            {
                // Toggels cheat mode active or dissables it
                cheatsActive = !cheatsActive;
            }
        }
    }

}
