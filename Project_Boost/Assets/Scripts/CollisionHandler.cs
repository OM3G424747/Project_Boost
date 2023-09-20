using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Finish":
                LoadNextLevel();
                break;

            case "Friendly":
                Debug.Log("Standing on friendly platform");
                break;
            
            // Defaults to the player hitting a differe object
            default:
                ReloadLevel();
                break;

        }
    }

    // Reloads Scene after hitting an object 
    void ReloadLevel()
    {
        // Sets index of current level in build
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Reloads the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Loads next level after hitting the finish
    void LoadNextLevel()
    {
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
