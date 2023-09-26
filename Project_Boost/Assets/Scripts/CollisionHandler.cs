using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // Stores rocket movement component
    Movement movement;
    [SerializeField] float reloadLevelDelay = 3f;
    [SerializeField] float loadNextLevelDelay = 3f;

    void Start()
    {
        // Accesses component at the start of the level load.
        movement = GetComponent<Movement>();
    }


    void OnCollisionEnter(Collision other)
    {
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
                StartCrash();
                break;

        }
    }

    // Used to call methods associated with the player crashing the rocket
    void StartCrash()
    {
        // disables movement and loads level again
        movement.notCrashed = false;
        //movement.enabled = false;
        Invoke( "ReloadLevel", reloadLevelDelay);

    }

    void StartLanding()
    {
        
        // disables movement and loads next level
        movement.notCrashed = false;
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

}
