using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float reloadLevelDelay = 3f;
    [SerializeField] float loadNextLevelDelay = 3f;
    [SerializeField] AudioClip crashImpact;
    [SerializeField] AudioClip levelPass;

    // Stores rocket movement component, sound and collider
    Movement movement;
    AudioSource rocketFXSound;
    Collider[] hitBox;

    // Confirms if the player has landed or crashed to prevent additional audio
    bool isTransitioning = false;

    void Start()
    {
        // Accesses component at the start of the level load.
        movement = GetComponent<Movement>();
        rocketFXSound = GetComponent<AudioSource>();
        // Gets array of colliders
        hitBox = GetComponents<Collider>();
    }


    void OnCollisionEnter(Collision other)
    {
        // Confirms players hasn't touched any other objects before
        // Skips switch statement if true.
        if (isTransitioning){return;}

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
                // Loops over colliders and dissables them
                foreach (Collider col in hitBox)
                {
                    Debug.Log("Disabled Col");
                    col.enabled = false;
                }
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
        // Movement.enabled = false;
        Invoke( "ReloadLevel", reloadLevelDelay);

        // Loops over colliders and dissables them
        foreach (Collider col in hitBox)
        {
            Debug.Log("Disabled Col");
            col.enabled = false;
        }

        // Stores ship's current movement
        Vector3 parentLinearVelocity = GetComponent<Rigidbody>().velocity;
        Vector3 parentAngularVelocity = GetComponent<Rigidbody>().angularVelocity;

        // Inform all the child cubes about the crash
        foreach (CubeHandler cubeScript in GetComponentsInChildren<CubeHandler>())
        {
            cubeScript.CrashDetected(parentLinearVelocity, parentAngularVelocity);
        }

    }

    void StartLanding()
    {
        isTransitioning = true;
        // disables movement and loads next level
        movement.notCrashed = false;
        // Stops all other audio clips, adjusts volume and plays success audio tone
        PlayOnlySelectedAudio(levelPass, 1f);
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


}
